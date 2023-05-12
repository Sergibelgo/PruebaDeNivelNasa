using AutoMapper;
using PruebaDeNivelNasa.Models;

namespace PruebaDeNivelNasa.Services
{
    public class AutoMapperProfiles : Profile
    {
        /// <summary>
        /// Profiles for the automapper
        /// </summary>
        public AutoMapperProfiles()
        {
            CreateMap<Asteroid, AsteroidDTO>()
                .ForMember(dto => dto.Nombre, ent => ent.MapFrom(x => x.name))
                .ForMember(dto => dto.Fecha, ent => ent.MapFrom(x => x.close_approach_data[0].close_approach_date))
                .ForMember(dto => dto.Planeta, ent => ent.MapFrom(x => x.close_approach_data[0].orbiting_body))
                .ForMember(dto => dto.Velocidad, ent => ent.MapFrom(x => x.close_approach_data[0].relative_velocity.kilometers_per_hour))
                .ForMember(dto => dto.Diametro, ent => ent.MapFrom(x => (x.estimated_diameter.kilometers.estimated_diameter_max + x.estimated_diameter.kilometers.estimated_diameter_min) / 2));
        }
    }
}
