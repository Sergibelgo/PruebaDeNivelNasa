namespace PruebaDeNivelNasa.Models.ResultAPI
{
    public class Asteroid
    {
        public Links links { get; set; } = new Links();
        public int id { get; set; }
        public string name { get; set; }
        public Estimated_diameter estimated_diameter { get; set; } = new Estimated_diameter();
        public bool is_potentially_hazardous_asteroid { get; set; }
        public IList<Close_approach_data> close_approach_data { get; set; } = new List<Close_approach_data>();
    }
}
