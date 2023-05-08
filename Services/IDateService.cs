namespace PruebaDeNivelNasa.Services
{
    public interface IDateService
    {
        Task<DateTime> GetDate(DateTime startDate, int days);
    }
}
