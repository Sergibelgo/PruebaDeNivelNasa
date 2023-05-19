using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PruebaDeNivelNasa.Models.DTOS;
using PruebaDeNivelNasa.Models.ResultAPI;
using System.Net;

namespace Test.Utils
{
    public static class Utils
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
        public static AsteroidDTO AsteroidDTOGenerator(string name, decimal diameter, decimal speed, string orbit, DateOnly fecha)
        {
            return new()
            {
                Nombre = name,
                Diametro = diameter,
                Velocidad = speed,
                Planeta = orbit,
                Fecha = fecha
            };
        }
        public static ResultApi ResultNormal()
        {
            return new ResultApi()
            {
                element_count = 4,
                near_earth_objects = new Dictionary<DateOnly, List<Asteroid>>
                {
                    {DateOnly.MaxValue, new List<Asteroid> {
                        new Asteroid()
                        {
                            name="4341 Poseidon (1987 KF)",
                            id=2004341,
                            close_approach_data=new List<Close_approach_data>(){
                                new Close_approach_data(){
                                    close_approach_date=DateOnly.MaxValue,
                                    relative_velocity=new Relative_velocity()
                                    {
                                        kilometers_per_hour=64181.5940497522M
                                    },
                                    orbiting_body="Earth"
                                }
                            },
                            estimated_diameter= new Estimated_diameter()
                            {
                                kilometers=new Estimated()
                                {
                                    estimated_diameter_max=3.6144313359M,
                                    estimated_diameter_min=1.6164228334M
                                }
                            },
                            is_potentially_hazardous_asteroid=true,
                            links=null

                        },new Asteroid()
                        {
                            name="4341 Poseidon (1987 KF)",
                            id=2004341,
                            close_approach_data=new List<Close_approach_data>(){
                                new Close_approach_data(){
                                    close_approach_date=DateOnly.MaxValue,
                                    relative_velocity=new Relative_velocity()
                                    {
                                        kilometers_per_hour=64181.5940497522M
                                    }
                                }
                            },
                            estimated_diameter= new Estimated_diameter()
                            {
                                kilometers=new Estimated()
                                {
                                    estimated_diameter_max=10,
                                    estimated_diameter_min=10
                                }
                            },
                            is_potentially_hazardous_asteroid=false,
                            links=null
                        },new Asteroid()
                        {
                            name="4341 Poseidon (1987 KF)",
                            id=2004341,
                            close_approach_data=new List<Close_approach_data>(){
                                new Close_approach_data(){
                                    close_approach_date=DateOnly.MaxValue,
                                    relative_velocity=new Relative_velocity()
                                    {
                                        kilometers_per_hour=64181.5940497522M
                                    }
                                }
                            },
                            estimated_diameter= new Estimated_diameter()
                            {
                                kilometers=new Estimated()
                                {
                                    estimated_diameter_max=10,
                                    estimated_diameter_min=10
                                }
                            },
                            is_potentially_hazardous_asteroid=false,
                            links=null
                        },new Asteroid()
                        {
                            name="4341 Poseidon (1987 KF)",
                            id=2004341,
                            close_approach_data=new List<Close_approach_data>(){
                                new Close_approach_data(){
                                    close_approach_date=DateOnly.MaxValue,
                                    relative_velocity=new Relative_velocity()
                                    {
                                        kilometers_per_hour=64181.5940497522M
                                    }
                                }
                            },
                            estimated_diameter= new Estimated_diameter()
                            {
                                kilometers=new Estimated()
                                {
                                    estimated_diameter_max=10,
                                    estimated_diameter_min=10
                                }
                            },
                            is_potentially_hazardous_asteroid=false,
                            links=null
                        }
                    }}
                }
            };
        }
        public static ResponseDTO ResponseNormal()
        {
            return new ResponseDTO()
            {
                List = new List<AsteroidDTO>()
                {
                    new AsteroidDTO()
                    {
                        Nombre="4341 Poseidon (1987 KF)",
                        Fecha=DateOnly.MaxValue,
                        Diametro=2.61542708465M,
                        Planeta="Earth",
                        Velocidad=64181.5940497522M
                    }
                }
            };
        }
        public static string ResponseJSONNormal()
        {
            var data = ResponseNormal();
            return JsonConvert.SerializeObject(data);
        }
        public static Dictionary<string, string> GetDataForResponseTest()
        {
            var dictionary = new Dictionary<string, string>();
            string[] fileArray = { "../../../FileResponse01.json" };
            foreach (string item in fileArray)
            {
                using (StreamReader reader = new StreamReader(item))
                {
                    dictionary["Normal"] = reader.ReadToEnd();
                }
            }
            return dictionary;

        }
        public static Mock<HttpMessageHandler> ConfigureMoqHTTP(string urlJson)
        {
            Mock<HttpMessageHandler> mockHttpClientHandler = new();
            //Arrange
            string data = "";

            try
            {
                using StreamReader reader = new(urlJson);
                data = reader.ReadToEnd().Replace("\\n", "").Replace("\\t", "");
            }
            catch
            {
                data = "";
            }

            HttpResponseMessage response = new()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(data)
            };
            mockHttpClientHandler = new();
            mockHttpClientHandler
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(response);
            return mockHttpClientHandler;
        }

    }
}
