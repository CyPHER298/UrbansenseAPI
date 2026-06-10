namespace UrbansenseAPI.Domain.Exceptions;

public class ExternalApiException(string service, string detail)
    : DomainException($"Falha ao comunicar com {service}: {detail}");
