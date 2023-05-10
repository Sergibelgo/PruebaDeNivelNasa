using PruebaDeNivelNasa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Utils
    {
        public static Asteroid AsteroidGenerator(bool hazard, string name, DateOnly date, string orbiting, decimal velocity, decimal diameter_max, decimal diameter_min)
        {
            var random = Random.Shared;
            return new Asteroid()
            {
                id = random.Next(),
                name = name,
                links = null,
                is_potentially_hazardous_asteroid = hazard,
                close_approach_data = new List<Close_approach_data> {
                    new Close_approach_data() {
                        close_approach_date = date,
                        orbiting_body=orbiting,
                        relative_velocity=new Relative_velocity()
                        {
                            kilometers_per_hour=velocity
                        }
                    }
                },
                estimated_diameter = new Estimated_diameter()
                {
                    kilometers = new Estimated()
                    {
                        estimated_diameter_max = diameter_max,
                        estimated_diameter_min = diameter_min
                    }
                }
            };
        }
        public static AsteroidDTO AsteroidDTOGenerator(string name, decimal diameter, decimal speed, string orbit)
        {
            return new AsteroidDTO()
            {
                Nombre=name,
                Diametro=diameter,
                Velocidad=speed,
                Planeta=orbit,

            }
        }
    }
}
