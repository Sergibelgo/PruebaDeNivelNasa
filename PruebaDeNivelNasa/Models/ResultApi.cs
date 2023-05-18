namespace PruebaDeNivelNasa.Models
{
    //TODO: 1 clase por 1 archivo
    public class ResultApi
    {
        public int element_count { get; set; }
        public Dictionary<DateOnly, List<Asteroid>> near_earth_objects { get; set; } = new Dictionary<DateOnly, List<Asteroid>>();

    }

    public class Asteroid
    {
        public Links links { get; set; } = new Links();
        public int id { get; set; }
        public string name { get; set; }
        public Estimated_diameter estimated_diameter { get; set; } = new Estimated_diameter();
        public bool is_potentially_hazardous_asteroid { get; set; }
        public IList<Close_approach_data> close_approach_data { get; set; } = new List<Close_approach_data>();
    }

    public class Close_approach_data
    {
        public DateOnly close_approach_date { get; set; } = new DateOnly();
        public Relative_velocity relative_velocity { get; set; } = new Relative_velocity();
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
