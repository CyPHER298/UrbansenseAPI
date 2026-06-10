using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Infrastructure.External;

public interface IOpenWeatherClient
{
    Task<Weather> FetchCurrentAsync(string city, double lat, double lon);
}
