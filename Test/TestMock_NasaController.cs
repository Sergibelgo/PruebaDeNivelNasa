using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using PruebaDeNivelNasa.Controllers;
using PruebaDeNivelNasa.Models;
using PruebaDeNivelNasa.Services;

namespace Test
{
    //TODO: separa por carpetas los test de mock de los unitarios
    [TestClass]
    public class TestMock_NasaController
    {
        private readonly IConfiguration _configuration;
        public TestMock_NasaController()
        {
            //TODO: hay que evitar el hardcodeo de urls
            var myConfiguration = new Dictionary<string, string>
            {
                {"APIURL", "https://api.nasa.gov/neo/rest/v1/feed"},
            };

            _configuration = new ConfigurationBuilder()
               .AddInMemoryCollection(myConfiguration)
               .Build();
        }
        //TODO: te falta mockear el httpclient que usa tu servicio para llamar a la api de la nasa y simular
        //que ese httpclient te devuelve un json
        [TestMethod]
        public async Task TestResponseNormal()
        {
            //Arrange
            Dictionary<string, string> DataForResponse = GetDataForResponseTest();
            var resultNormal = ResultNormal();
            var responseNormal = ResponseNormal();
            //TODO: declaras el mismo objeto en varios métodos, se puede declarar como propiedad de la clase
            Mock<INasaService> mockNasa = new();
            Mock<IJSONService> mockJson = new();
            Mock<IDateService> mockIDate = new();

            mockNasa.Setup(a => a.FetchData(It.IsAny<string>())).ReturnsAsync(DataForResponse["Normal"]);
            mockNasa.Setup(a => a.GetData(resultNormal, It.IsAny<int>())).Returns(responseNormal);

            mockJson.Setup(a => a.ConvertData<ResultApi>(It.IsAny<string>())).Returns(resultNormal);
            mockJson.Setup(a => a.GetUrl(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>())).Returns("https://api.nasa.gov/neo/rest/v1/feed?start_date=2021-12-09&end_date=2021-12-12&api_key=DEMO_KEY");
            mockJson.Setup(a => a.GetResult(It.IsAny<ResponseDTO>())).Returns(ResponseJSONNormal());

            mockIDate.Setup(a => a.GetDate(It.IsAny<DateTime>(), It.IsAny<int>())).Returns(DateTime.Now);

            //Act
            NasaController nasaController = new(mockNasa.Object, mockJson.Object, mockIDate.Object, new ConfigurationBuilder()
    .Build());
            var response = await nasaController.GetInfo(3);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)response).Value, ResponseJSONNormal());
            mockNasa.Verify(r => r.FetchData(It.IsAny<string>()));
            mockNasa.Verify(a => a.GetData(resultNormal, It.IsAny<int>()));
            mockJson.Verify(a => a.ConvertData<ResultApi>(It.IsAny<string>()));
            mockJson.Verify(a => a.GetUrl(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()));
            mockIDate.Verify(a => a.GetDate(It.IsAny<DateTime>(), It.IsAny<int>()));

        }
        [TestMethod]
        public async Task TestResponseAPICorrecta()
        {
            //Arrange
            Mock<INasaService> mockNasa = new();
            var data = "";
            using (StreamReader reader = new("../../../FileResponse01.json"))
            {
                data = reader.ReadToEnd();
            }
            mockNasa.Setup(x => x.FetchData(It.IsAny<string>())).ReturnsAsync(data);
            mockNasa.Setup(x => x.GetData(It.IsAny<ResultApi>(), It.IsAny<int>())).Returns(ResponseNormal());

            //Act
            NasaController nasaController = new(mockNasa.Object, new JSONService(), new DateService(), _configuration);
            var response = await nasaController.GetInfo(3);

            //Assert

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)response).Value, ResponseJSONNormal());

        }
        [TestMethod]
        public async Task TestResponseAPINull()
        {
            //Arrange
            Mock<INasaService> mockNasa = new();

            mockNasa.Setup(x => x.FetchData(It.IsAny<string>())).ReturnsAsync((string)null);

            //Act
            NasaController nasaController = new(mockNasa.Object, new JSONService(), new DateService(), _configuration);
            var response = await nasaController.GetInfo(3);

            //Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult));
            mockNasa.Verify(r => r.FetchData(It.IsAny<string>()));
        }
        [TestMethod]
        public async Task TestResponseAPIStatusCodeBad()
        {

            //Arrange
            Mock<INasaService> mockNasa = new();
            mockNasa.Setup(x => x.FetchData(It.IsAny<string>())).Throws(new Exception("429__Todas las claves han sido consumidas"));

            //Act
            NasaController nasaController = new(mockNasa.Object, new JSONService(), new DateService(), _configuration);
            var response = await nasaController.GetInfo(3);

            //Arrange
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(ObjectResult));
            ObjectResult result = (ObjectResult)response;
            Assert.IsTrue(result.StatusCode == 429);
            mockNasa.Verify(x => x.FetchData(It.IsAny<string>()));
        }
        //TODO: en una case de tests, solo debe haber tests, extrae los métodos que no sean tests (a una clase de utils por ejemplo)
        private ResultApi ResultNormal()
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
        private ResponseDTO ResponseNormal()
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
        private string ResponseJSONNormal()
        {
            var data = ResponseNormal();
            return JsonConvert.SerializeObject(data);
        }
        private Dictionary<string, string> GetDataForResponseTest()
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
    }
}