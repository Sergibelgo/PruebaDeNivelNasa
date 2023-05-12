using PruebaDeNivelNasa.Models;

namespace PruebaDeNivelNasa.Services
{
    public interface INasaService
    {
        ResponseDTO GetData(ResultApi dataAPI, int limit);
        Task<string> FetchData(string url);
    }
}
