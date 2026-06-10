# UrbanSense API

Plataforma de inteligência urbana para São Paulo que conecta dados climáticos em tempo real, mobilidade (Metrô/CPTM) e IA generativa para prever impactos no dia a dia dos cidadãos.

## Integrantes
| Nome | RM |
|---|---|
| Adao Yuri | RM559223 |
| João Vitor Santana | RM560781 |
| Julia Rodrigues | RM559781 |
| Felipe Soares | RM559175 |
| Henrique Souza | RM99742 |

---

## Sumário

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Diagramas](#diagramas)
- [Stack](#stack)
- [Desenvolvimento](#desenvolvimento)
- [Endpoints](#endpoints)
- [Testes](#testes)
- [Health Checks](#health-checks)

---

## Visão Geral

A API monitora 5 cidades da Grande São Paulo (São Paulo, Guarulhos, Osasco, Santo André e Campinas), avalia condições climáticas em tempo real e emite alertas automáticos baseados em regras de negócio. Integra IA (OpenAI) para fornecer respostas contextualizadas sobre mobilidade urbana e clima.

---

## Arquitetura

O projeto segue **Clean Architecture** com separação em camadas:

```
UrbansenseAPI/
├── Controllers/              # Camada de apresentação (HTTP)
├── Domain/
│   ├── Enums/               # Enumerações de domínio
│   ├── Exceptions/          # Exceções customizadas + GlobalExceptionHandler
│   ├── IService/            # Interfaces dos serviços
│   ├── Models/              # Entidades e modelos de domínio
│   └── Services/            # Regras de negócio
├── DTOs/                    # Objetos de transferência de dados
├── Infrastructure/
│   ├── External/            # Cliente OpenWeather (IOpenWeatherClient)
│   ├── HealthChecks/        # Health checks customizados
│   ├── Middleware/          # Tratamento global de exceções (RFC 7807)
│   └── Persistence/
│       ├── Mapping/         # Configurações EF Core (Oracle)
│       └── Store/           # Stores em memória (WeatherStore, AlertStore)
├── Data/                    # AppDbContext
└── Migrations/              # Migrations EF Core
```

**Princípios aplicados:** SRP, DIP (injeção via interfaces), ISP, inversão de dependência nos testes.

---

## Diagramas

### Fluxo de uma requisição autenticada

```
Cliente
  │
  ▼
[JWT Bearer Middleware]
  │  token inválido → 401
  ▼
[Controller]
  │
  ▼
[Service] ──── IOpenWeatherClient ──── OpenWeather API
  │
  ├── WeatherStore (Singleton, in-memory)
  │
  └── AlertStore  (Singleton, in-memory)
  │
  ▼
[DTO Response] → Cliente
```

### Fluxo de avaliação de alertas (`POST /api/v1/alerts/evaluate`)

```
EvaluateAlertsAsync()
  │
  ├── DeactivateExpired()         ← remove alertas vencidos
  │
  └── para cada cidade monitorada:
        │
        ├── GetCurrentWeatherAsync()
        │
        └── ApplyBusinessRules(weather)
              │
              ├── RainMm ≥ 40  → Flooding    (Critical)
              ├── RainMm ≥ 20  → Flooding    (High)
              ├── RainMm ≥ 10  → HeavyRain   (Medium)
              ├── UvIndex ≥ 11 → HighUv      (High)
              ├── UvIndex ≥ 8  → HighUv      (Medium)
              └── Storm        → HeavyRain   (Critical)
```

### Fluxo de cálculo de risco de chuva

```
RainRisk.Calculate(city, hour, avgRain, events)
  │
  ├── rainScore  = Min(avgRain / 20.0, 1.0)   (peso 60%)
  ├── eventScore = Min(events  / 15.0, 1.0)   (peso 40%)
  │
  ├── riskScore = (rainScore × 0.6) + (eventScore × 0.4)
  │
  └── Level:
        riskScore < 0.4  → Low
        riskScore < 0.7  → Medium
        riskScore ≥ 0.7  → High
```

### Diagrama de camadas (dependências)

```
┌─────────────────────────────────────┐
│           Controllers               │  ← depende de IService
├─────────────────────────────────────┤
│     Services (regras de negócio)    │  ← depende de IOpenWeatherClient
├─────────────────────────────────────┤
│    Domain Models / Enums / DTOs     │  ← sem dependências externas
├─────────────────────────────────────┤
│  Infrastructure (EF Core, HTTP,     │  ← implementa interfaces do domínio
│  Stores, HealthChecks)              │
└─────────────────────────────────────┘
```

---

## Stack

| Tecnologia | Versão | Uso |
|---|---|---|
| .NET / ASP.NET Core | 9.0 | Framework principal |
| Entity Framework Core | 9.0 | ORM |
| Oracle.EntityFrameworkCore | 9.23.80 | Driver Oracle |
| BCrypt.Net-Next | 4.0.3 | Hash de senhas |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0 | Autenticação JWT |
| Swashbuckle (Swagger) | 6.9.0 | Documentação OpenAPI |
| OpenAI SDK | 2.2.0 | IA generativa (Advisor) |
| xUnit | 2.9.2 | Testes unitários |
| NSubstitute | 5.3.0 | Mocks nos testes |
| FluentAssertions | 6.12.2 | Asserções expressivas |

---

## Desenvolvimento

### Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- Acesso ao Oracle FIAP (`oracle.fiap.com.br`)
- Chave de API [OpenWeather](https://openweathermap.org/api)
- Chave de API [OpenAI](https://platform.openai.com/api-keys) (opcional — Advisor usa fallback sem ela)

### Configuração local

1. Clone o repositório:
```bash
git clone https://github.com/CyPHER298/UrbansenseAPI.git
cd UrbansenseAPI
```

2. Copie o arquivo de variáveis de ambiente:
```bash
cp .env.example .env
```

3. Preencha o `.env` com suas credenciais:
```env
ConnectionStrings__UrbansenseAPI=Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=SEU_RM;Password=SUA_SENHA;
Jwt__Secret=sua-chave-secreta-minimo-32-caracteres
OpenWeather__ApiKey=sua_chave_openweather
OpenAI__ApiKey=sua_chave_openai
```

4. Aplique as migrations:
```bash
dotnet ef database update --project UrbansenseAPI
```

5. Execute a API:
```bash
dotnet run --project UrbansenseAPI
```

A API estará disponível em `https://localhost:7xxx` e o Swagger em `https://localhost:7xxx/index.html`.

---

## Endpoints

### Autenticação

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| POST | `/api/v1/auth/register` | Não | Cria novo usuário |
| POST | `/api/v1/auth/login` | Não | Retorna JWT |

### Clima

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| GET | `/api/v1/weather/current` | JWT | Clima atual via OpenWeather |
| GET | `/api/v1/weather/rain-risk` | JWT | Análise histórica de risco de chuva |
| GET | `/api/v1/weather/heavy-rain` | JWT | Cidades com chuva acima do limiar |

### Alertas

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| GET | `/api/v1/alerts/city/{city}` | JWT | Alertas ativos por cidade |
| GET | `/api/v1/alerts/type/{type}` | JWT | Alertas por tipo |
| POST | `/api/v1/alerts/evaluate` | ADMIN | Dispara avaliação de alertas |

### Mobilidade

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| GET | `/api/v1/mobility/lines-at-risk` | JWT | Linhas Metrô/CPTM em risco |

### Advisor (IA)

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| POST | `/api/v1/advisor/ask` | JWT | Pergunta ao assistente urbano |
| GET | `/api/v1/advisor/daily-summary/{city}` | JWT | Resumo diário da cidade |
| GET | `/api/v1/advisor/transit/{lineName}` | JWT | Análise de impacto em linha de trem |

### Infra

| Método | Rota | Auth | Descrição |
|---|---|---|---|
| GET | `/health` | Não | Status geral |
| GET | `/health/details` | Não | Detalhes por serviço (DB + OpenWeather) |

---

## Testes

Os testes seguem o padrão **AAA (Arrange, Act, Assert)** e usam NSubstitute para mocks e FluentAssertions para asserções.

### Executar todos os testes

```bash
dotnet test UrbansenseAPI.Tests
```

### Executar com cobertura detalhada

```bash
dotnet test UrbansenseAPI.Tests --logger "console;verbosity=detailed"
```

### Cobertura por arquivo

| Arquivo | Testes | O que cobre |
|---|---|---|
| `Domain/RainRiskTests.cs` | 7 | Cálculo de risco de chuva (Low / Medium / High / cap / teoria) |
| `Services/AlertServiceTests.cs` | 7 | Regras de negócio: flooding, UV, tempestade, tipo inválido, falha silenciosa |
| `Services/WeatherServiceTests.cs` | 5 | Mapeamento DTO, persistência na store, wrap de exceção, filtro de chuva |
| `Services/JwtServiceTests.cs` | 6 | Geração de token, claims, issuer/audience, Jti único |

**Total: 25 testes**

### Exemplos de testes

#### Chuva ≥ 40mm gera alerta Flooding Critical

```csharp
[Fact]
public async Task EvaluateAlertsAsync_WhenRainAbove40mm_ShouldCreateCriticalFloodingAlert()
{
    // Arrange
    var weather = new WeatherResponse(
        "São Paulo", -23.5505, -46.6333,
        22, 21, 95, 8, 45.0, null, "Rain", "chuva muito forte", DateTime.UtcNow);

    _weatherService
        .GetCurrentWeatherAsync(Arg.Any<string>(), Arg.Any<double>(), Arg.Any<double>())
        .Returns(weather);

    // Act
    await _sut.EvaluateAlertsAsync();
    var alerts = await _sut.GetActiveByCityAsync("São Paulo");

    // Assert
    alerts.Should().Contain(a => a.Type == "Flooding" && a.Severity == "Critical");
}
```

#### Token JWT contém as claims corretas

```csharp
[Fact]
public void GenerateToken_ShouldEmbedUsernameAndRoleClaims()
{
    // Arrange
    var user = new AppUser { Id = 7, Username = "pedro", Role = "USER" };

    // Act
    var result = _sut.GenerateToken(user);

    // Assert
    var handler = new JwtSecurityTokenHandler();
    var jwt     = handler.ReadJwtToken(result.Token);

    jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "pedro");
    jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == "USER");
}
```

#### Score de risco é capado em 1.0

```csharp
[Fact]
public void Calculate_WhenRainExceedsMaxThreshold_ShouldCapScoreAt1()
{
    // Arrange
    double? avgRain = 100.0;
    var events = 20;

    // Act
    var result = RainRisk.Calculate("SP", 18, avgRain, events);

    // Assert
    result.RiskScore.Should().BeApproximately(1.0, 0.01);
    result.Level.Should().Be(RiskLevel.High);
}
```

### Acesso via Swagger (passo a passo)

1. Suba a API com `dotnet run`
2. Acesse `https://localhost:{porta}` no navegador
3. Registre um usuário:

```json
POST /api/v1/auth/register
{
  "username": "meuusuario",
  "password": "minhasenha123",
  "role": "USER"
}
```

4. Faça login e copie o `token` da resposta:

```json
POST /api/v1/auth/login
{
  "username": "meuusuario",
  "password": "minhasenha123"
}
```

5. Clique em **Authorize 🔒** no Swagger e insira:
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

6. Agora todas as rotas protegidas estão disponíveis.

### Exemplos de requisições

**Clima atual em São Paulo:**
```
GET /api/v1/weather/current?city=São Paulo&lat=-23.5505&lon=-46.6333
```

**Alertas ativos em Guarulhos:**
```
GET /api/v1/alerts/city/Guarulhos
```

**Linhas de metrô em risco:**
```
GET /api/v1/mobility/lines-at-risk?city=São Paulo&lat=-23.5505&lon=-46.6333
```

**Pergunta ao assistente:**
```json
POST /api/v1/advisor/ask
{
  "question": "Devo pegar a Linha 9 hoje com essa chuva?",
  "city": "São Paulo"
}
```

**Disparar avaliação de alertas (requer role ADMIN):**
```
POST /api/v1/alerts/evaluate
```

---

## Health Checks

```
GET /health
→ { "status": "Healthy" }

GET /health/details
→ {
    "status": "Healthy",
    "checks": [
      { "name": "oracle-db",       "status": "Healthy", "duration": "120ms" },
      { "name": "openweather-api", "status": "Healthy", "duration": "85ms"  }
    ],
    "totalDuration": "205ms"
  }
```

## Video Demonstrativo
**[Assista a demonstração completa no YouTube](https://youtu.be/9OqdGz207TU)**
