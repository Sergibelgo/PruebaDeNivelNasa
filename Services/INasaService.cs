using PruebaDeNivelNasa.Models;

namespace PruebaDeNivelNasa.Services
{
    public interface INasaService
    {
        ResponseDTO GetData(ResultadoPeticionApi dataAPI);
        Task<string> GetInfo(string url);
    }
}
