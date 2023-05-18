namespace PruebaDeNivelNasa.Services
{
    //TODO: deberías separar o bien unificar las interfaces de los servicios, bien sea carpeta que contiene
    //todas las inrefaces y otra todos los servicios o carpeta por cada pack de interfaz/servicio (aplicabel a todos los servicios)
    public interface IDateService
    {
        DateTime GetDate(DateTime startDate, int days);
    }
}
