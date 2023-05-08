namespace PruebaDeNivelNasa.Services
{
    public class NasaService:INasaService
    {
        private readonly HttpClient _httpClient;

        public NasaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task GetInfo<ResultadoPeticionApi>(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}
