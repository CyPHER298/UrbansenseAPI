namespace UrbansenseAPI.Domain.Exceptions;

public class WeatherNotFoundException(string city)
    : DomainException($"Dados climáticos não encontrados para: {city}");
