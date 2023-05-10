using PruebaDeNivelNasa.Models;

namespace PruebaDeNivelNasa.Services
{
    public interface IJSONService
    {
        T ConvertData<T>(string data);
        string GetResult<T>(T model);
        string GetUrl(string url,DateTime startDate,DateTime endDate,string key);
    }
}
