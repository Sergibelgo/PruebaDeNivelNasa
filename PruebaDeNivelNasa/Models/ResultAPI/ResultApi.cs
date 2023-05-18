namespace PruebaDeNivelNasa.Models.ResultAPI
{
    //TODO: 1 clase por 1 archivo
    //Fixed
    public class ResultApi
    {
        public int element_count { get; set; }
        public Dictionary<DateOnly, List<Asteroid>> near_earth_objects { get; set; } = new Dictionary<DateOnly, List<Asteroid>>();

    }
}
