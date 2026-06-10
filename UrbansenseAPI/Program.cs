using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore;
using UrbansenseAPI.Data;
using UrbansenseAPI.Domain.Services;
using UrbansenseAPI.Infrastructure.External;
using UrbansenseAPI.Infrastructure.HealthChecks;
using UrbansenseAPI.Infrastructure.Middleware;
using UrbansenseAPI.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Tratamento global de exceções (RFC 7807 Problem Details)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
        };
    });
builder.Services.AddAuthorization();

// Swagger com suporte a Bearer token
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "UrbanSense API",
        Version     = "v1",
        Description = """
            ## Plataforma de Inteligência Urbana para São Paulo

            Conecta **dados climáticos + mobilidade urbana + IA** para prever impactos reais no dia a dia.

            ### Autenticação
            1. Crie um usuário em `POST /api/v1/auth/register`
            2. Faça login em `POST /api/v1/auth/login` e copie o `token`
            3. Clique em **Authorize 🔒** e insira: `Bearer {token}`

            ### Domínios
            | Grupo | Descrição |
            |---|---|
            | **Auth** | Registro e login com JWT |
            | **Weather** | Clima em tempo real via OpenWeather |
            | **Alerts** | Alertas por regras de negócio (chuva, UV, tempestade) |
            | **Mobility** | Impacto nas 12 linhas Metrô/CPTM de SP |
            | **Advisor** | Assistente urbano com IA (OpenAI + RAG) |
            """,
        Contact = new()
        {
            Name  = "Equipe UrbanSense — FIAP 2026",
            Email = "henrique.souza@w3gconsultoria.com.br"
        }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "Bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Insira o token JWT. Exemplo: Bearer {seu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            []
        }
    });

    var xml = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(xml)) c.IncludeXmlComments(xml);
});

// Oracle — Entity Framework Core
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("UrbansenseAPI")));

// HttpClient para OpenWeather
builder.Services.AddHttpClient<IOpenWeatherClient, OpenWeatherClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["OpenWeather:BaseUrl"] ?? "https://api.openweathermap.org");
});

// Stores com estado compartilhado — Singleton (thread-safe internamente)
builder.Services.AddSingleton<WeatherStore>();
builder.Services.AddSingleton<AlertStore>();

// Serviços de domínio — Scoped
builder.Services.AddScoped<IJwtService,      JwtService>();
builder.Services.AddScoped<IWeatherService,  WeatherService>();
builder.Services.AddScoped<IAlertService,    AlertService>();
builder.Services.AddScoped<IMobilityService, MobilityService>();
builder.Services.AddScoped<IAdvisorService,  AdvisorService>();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(
        name:        "oracle-db",
        failureStatus: HealthStatus.Unhealthy,
        tags:        ["db", "oracle"])
    .AddCheck<OpenWeatherHealthCheck>(
        name:        "openweather-api",
        failureStatus: HealthStatus.Degraded,
        tags:        ["external"]);

// CORS
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Exception handler deve ser o primeiro middleware
app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UrbanSense API v1");
    c.RoutePrefix = string.Empty;
});

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Health Check endpoints (públicos — não exigem JWT)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true
});

app.MapHealthChecks("/health/details", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = async (ctx, report) =>
    {
        ctx.Response.ContentType = "application/json";
        var result = new
        {
            status  = report.Status.ToString(),
            checks  = report.Entries.Select(e => new
            {
                name        = e.Key,
                status      = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration    = e.Value.Duration.TotalMilliseconds + "ms",
                error       = e.Value.Exception?.Message
            }),
            totalDuration = report.TotalDuration.TotalMilliseconds + "ms"
        };
        await ctx.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
});

app.Run();