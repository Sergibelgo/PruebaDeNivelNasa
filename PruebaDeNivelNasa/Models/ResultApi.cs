namespace PruebaDeNivelNasa.Models
{
    public class ResultApi
    {
        public int element_count { get; set; }
        public Dictionary<DateOnly, List<Asteroid>> near_earth_objects { get; set; }

    }

    public class Asteroid
    {
        public Links links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Estimated_diameter estimated_diameter { get; set; }
        public bool is_potentially_hazardous_asteroid { get; set; }
        public IList<Close_approach_data> close_approach_data { get; set; }
    }

    public class Close_approach_data
    {
        public DateOnly close_approach_date { get; set; }
        public Relative_velocity relative_velocity;
        public string orbiting_body { get; set; }
    }

    public class Relative_velocity
    {
        public decimal kilometers_per_hour { get; set; }
    }

    public class Estimated_diameter
    {
        public Estimated kilometers { get; set; }
    }

    public class Estimated
    {
        public decimal estimated_diameter_min { get; set; }
        public decimal estimated_diameter_max { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
    }
}
