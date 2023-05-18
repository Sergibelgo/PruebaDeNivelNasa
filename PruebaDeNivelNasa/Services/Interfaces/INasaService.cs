using PruebaDeNivelNasa.Models.DTOS;
using PruebaDeNivelNasa.Models.ResultAPI;

namespace PruebaDeNivelNasa.Services.Interfaces
{
    public interface INasaService
    {
        ResponseDTO GetData(ResultApi dataAPI, int limit);
        Task<string> FetchData(string url);
    }
}
