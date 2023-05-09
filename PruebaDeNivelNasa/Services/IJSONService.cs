using PruebaDeNivelNasa.Models;

namespace PruebaDeNivelNasa.Services
{
    public interface IJSONService
    {
        ResultadoPeticionApi ConvertData(string data);
        string GetResult(ResponseDTO responseDTO);
        string GetUrl(string url,DateTime startDate,DateTime endDate,string key);
    }
}
