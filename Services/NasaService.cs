namespace PruebaDeNivelNasa.Services
{
    public class NasaService:INasaService
    {
        private readonly HttpClient _httpClient;

        public NasaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
