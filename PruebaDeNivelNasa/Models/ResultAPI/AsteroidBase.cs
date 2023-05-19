namespace PruebaDeNivelNasa.Models.ResultAPI
{
    public class AsteroidBase
    {
        public IList<Close_approach_data> close_approach_data { get; set; } = new List<Close_approach_data>();
        public Estimated_diameter estimated_diameter { get; set; } = new Estimated_diameter();
        public int id { get; set; }
        public bool is_potentially_hazardous_asteroid { get; set; }
        public Links links { get; set; } = new Links();
        public string name { get; set; }
    }
}