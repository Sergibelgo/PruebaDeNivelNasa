namespace PruebaDeNivelNasa.Models.DTOS
{
    //TODO: debe haber únicament euna clase por archivo
    //TODO: deben estar separados los modelos de obtención/parseo de la API de los DTOS, por ejemplo con carpetas
    //Fixed
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
}
