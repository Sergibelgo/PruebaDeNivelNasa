using PruebaDeNivelNasa.Models;

namespace PruebaDeNivelNasa.Services
{
    public interface INasaService
    {
        ResponseDTO GetData(ResultadoPeticionApi dataAPI,int limit);
        Task<string> GetInfo(string url);
    }
}
