namespace PruebaDeNivelNasa.Models.ResultAPI
{
    public class Close_approach_data
    {
        public DateOnly close_approach_date { get; set; } = new DateOnly();
        public Relative_velocity relative_velocity { get; set; } = new Relative_velocity();
        public string orbiting_body { get; set; }
    }
}
