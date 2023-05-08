namespace PruebaDeNivelNasa.Services
{
    public class DateService : IDateService
    {
        public DateTime GetDate(DateTime startDate, int days)
        {
            return startDate.AddDays(days);
        }
    }
}
