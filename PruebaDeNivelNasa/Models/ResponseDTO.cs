namespace PruebaDeNivelNasa.Models
{
    public class ResponseDTO
    {
        public List<AsteroidDTO> List { get; set; }
        
    }

    public class AsteroidDTO
    {
        public string Nombre { get; set; }
        public decimal Diametro { get; set; }
        public decimal Velocidad { get; set; }
        public DateOnly Fecha { get; set; }
        public string Planeta { get; set; }
    }
}
