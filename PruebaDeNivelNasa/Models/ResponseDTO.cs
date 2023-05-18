namespace PruebaDeNivelNasa.Models
{
    //TODO: debe haber únicament euna clase por archivo
    //TODO: deben estar separados los modelos de obtención/parseo de la API de los DTOS, por ejemplo con carpetas
    public class ResponseDTO
    {
        public List<AsteroidDTO> List { get; set; } = new List<AsteroidDTO>();
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            ResponseDTO other = (ResponseDTO)obj;
            return List.SequenceEqual(other.List);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class AsteroidDTO
    {
        public string Nombre { get; set; }
        public decimal Diametro { get; set; }
        public decimal Velocidad { get; set; }
        public DateOnly Fecha { get; set; }
        public string Planeta { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            foreach (var property in obj.GetType().GetProperties())
            {
                if (!property.GetValue(obj).Equals(property.GetValue(this)))
                {
                    return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
