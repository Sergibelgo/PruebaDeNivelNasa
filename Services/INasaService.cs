namespace PruebaDeNivelNasa.Services
{
    public interface INasaService
    {
        Task GetInfo<ResultadoPeticionApi>(DateOnly startDate, DateOnly endDate);
    }
}
