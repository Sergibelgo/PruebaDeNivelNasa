using PruebaDeNivelNasa.Services.Interfaces;

namespace PruebaDeNivelNasa.Services.Classes
{
    /// <summary>
    /// Service for all date related operations
    /// </summary>
    public class DateService : IDateService
    {
        /// <summary>
        /// Method to get the date x days ha gead from the date given
        /// </summary>
        /// <param name="startDate">The date of start to add</param>
        /// <param name="days">The number of days to go haead</param>
        /// <returns>A date x days a head from the startDate</returns>
        public DateTime GetDate(DateTime startDate, int days)
        {
            return startDate.AddDays(days);
        }
    }
}
