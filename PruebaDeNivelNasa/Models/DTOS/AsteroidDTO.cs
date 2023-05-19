namespace PruebaDeNivelNasa.Models.DTOS
{
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
