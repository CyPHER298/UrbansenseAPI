using OpenAI;
using OpenAI.Chat;
using UrbansenseAPI.DTOs;

namespace UrbansenseAPI.Domain.Services;

public class AdvisorService(
    IWeatherService weatherService,
    IAlertService alertService,
    IConfiguration configuration,
    ILogger<AdvisorService> logger) : IAdvisorService
{
    private static readonly (string City, double Lat, double Lon) DefaultCity =
        ("São Paulo", -23.5505, -46.6333);

    // Base de conhecimento sobre SP (RAG simples em memória)
    private static readonly string[] KnowledgeBase =
    [
        "A Linha 9 - Esmeralda é a mais vulnerável a chuvas fortes, com histórico de alagamentos na região de Osasco.",
        "A Linha 7 - Rubi opera em superfície e sofre atrasos significativos com chuvas acima de 8mm/h.",
        "A Zona Leste de São Paulo, especialmente Itaquera e Guaianases, tem histórico de alagamentos em chuvas intensas.",
        "O viaduto do Chá e a Av. Paulista têm tendência a engarrafamentos severos em dias de chuva forte.",
        "Índice UV acima de 8 é considerado alto pela OMS; protetor solar FPS 30+ é recomendado.",
        "Tempestades em SP costumam ser mais frequentes entre novembro e março (período chuvoso).",
        "A região do ABC Paulista (Santo André, São Bernardo, São Caetano) tem histórico de inundações no Rio Tamanduateí.",
        "Campinas tem padrão climático parecido com SP mas com chuvas mais intensas no verão.",
        "Guarulhos, por sua altitude e proximidade ao aeroporto, tem microclima que pode diferir de SP.",
        "Em dias de chuva forte, o tempo médio no trânsito de SP pode aumentar 60-80% nos horários de pico."
    ];

    public async Task<AdvisorResponse> AskAsync(AskRequest request)
    {
        var context = await BuildContextAsync(request.City);
        var relevantKnowledge = GetRelevantKnowledge(request.Question);
        var answer = await GenerateAnswerAsync(request.Question, context, relevantKnowledge);

        return new AdvisorResponse(answer, request.City, DateTime.UtcNow);
    }

    public async Task<AdvisorResponse> GetDailySummaryAsync(string city)
    {
        var context = await BuildContextAsync(city);
        var question = $"Gere um resumo diário completo para {city}, incluindo condições climáticas, alertas ativos e impacto na mobilidade.";
        var answer = await GenerateAnswerAsync(question, context, KnowledgeBase);

        return new AdvisorResponse(answer, city, DateTime.UtcNow);
    }

    public async Task<AdvisorResponse> AnalyzeTransitLineAsync(string lineName)
    {
        var context = await BuildContextAsync(DefaultCity.City);
        var relevant = KnowledgeBase.Where(k =>
            k.Contains(lineName, StringComparison.OrdinalIgnoreCase)).ToArray();
        var question = $"Analise o impacto climático atual na {lineName} e dê recomendações aos passageiros.";
        var answer = await GenerateAnswerAsync(question, context, relevant.Length > 0 ? relevant : KnowledgeBase);

        return new AdvisorResponse(answer, DefaultCity.City, DateTime.UtcNow);
    }

    private async Task<string> BuildContextAsync(string city)
    {
        var (lat, lon) = GetCityCoords(city);
        WeatherResponse? weather = null;
        List<AlertResponse> alerts = [];

        try { weather = await weatherService.GetCurrentWeatherAsync(city, lat, lon); } catch (Exception ex) { logger.LogWarning(ex, "Falha ao obter clima para {City}", city); }
        try { alerts = await alertService.GetActiveByCityAsync(city); } catch (Exception ex) { logger.LogWarning(ex, "Falha ao obter alertas para {City}", city); }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"=== Contexto Urbano: {city} ===");

        if (weather is not null)
        {
            sb.AppendLine($"Clima: {weather.Condition}, {weather.Temperature:F1}°C (sensação {weather.FeelsLike:F1}°C)");
            sb.AppendLine($"Chuva: {weather.RainMm:F1}mm/h | Umidade: {weather.Humidity}% | Vento: {weather.WindSpeed:F1}m/s");
            if (weather.UvIndex.HasValue) sb.AppendLine($"Índice UV: {weather.UvIndex}");
        }

        if (alerts.Any())
        {
            sb.AppendLine($"Alertas ativos ({alerts.Count}):");
            foreach (var a in alerts)
                sb.AppendLine($"  [{a.Severity}] {a.Type}: {a.Message}");
        }
        else
        {
            sb.AppendLine("Sem alertas ativos no momento.");
        }

        return sb.ToString();
    }

    private async Task<string> GenerateAnswerAsync(
        string question, string context, IEnumerable<string> knowledge)
    {
        var apiKey = configuration["OpenAI:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
            return GenerateFallbackAnswer(question, context);

        try
        {
            var client = new OpenAIClient(apiKey);
            var chatClient = client.GetChatClient("gpt-4o-mini");

            var systemPrompt = $"""
                Você é o UrbanSense, assistente de inteligência urbana de São Paulo.
                Responda de forma clara, prática e em português.
                Foque em impactos reais no dia a dia das pessoas.

                Base de conhecimento relevante:
                {string.Join("\n", knowledge)}

                {context}
                """;

            var response = await chatClient.CompleteChatAsync(
            [
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(question)
            ]);

            return response.Value.Content[0].Text;
        }
        catch
        {
            return GenerateFallbackAnswer(question, context);
        }
    }

    private static string GenerateFallbackAnswer(string question, string context)
    {
        return $"Com base nas condições atuais:\n\n{context}\n\n" +
               "Para respostas mais detalhadas, configure a chave da API OpenAI.";
    }

    private static string[] GetRelevantKnowledge(string question) =>
        KnowledgeBase.Where(k =>
            question.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Any(word => k.Contains(word, StringComparison.OrdinalIgnoreCase)))
        .DefaultIfEmpty(KnowledgeBase[0])
        .ToArray();

    private static (double Lat, double Lon) GetCityCoords(string city) => city.ToLower() switch
    {
        "guarulhos"    => (-23.4538, -46.5333),
        "osasco"       => (-23.5322, -46.7919),
        "santo andré"  => (-23.6639, -46.5383),
        "campinas"     => (-22.9068, -47.0626),
        _              => (-23.5505, -46.6333) // São Paulo default
    };
}
