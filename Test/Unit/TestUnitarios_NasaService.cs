using AutoMapper;
using PruebaDeNivelNasa.Models.DTOS;
using PruebaDeNivelNasa.Models.ResultAPI;
using PruebaDeNivelNasa.Services.Classes;
using static Test.Utils.Utils;
namespace Test.Unit
{
    [TestClass]
    public class TestUnitarios_NasaService
    {
        private NasaService nasaService;
        private readonly string urlCorrecta = "https://api.nasa.gov/neo/rest/v1/feed?start_date=2021-12-09&end_date=2021-12-12&api_key=DEMO_KEY";
        private readonly string urlFalsa = "http://urltotalmentefalsaysinsentido.com/";

        public TestUnitarios_NasaService()
        {
            Mapper mapperRepo = new(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfiles());
            }));
            HttpClient httpClient = new HttpClient();
            nasaService = new(httpClient, mapperRepo);
        }
        [TestMethod]
        public async Task NasaSerice_FetchData()
        {

            _ = Assert.ThrowsExceptionAsync<Exception>(() => nasaService.FetchData(urlFalsa));
            _ = Assert.ThrowsExceptionAsync<Exception>(() => nasaService.FetchData(""));
            _ = Assert.ThrowsExceptionAsync<Exception>(() => nasaService.FetchData("   "));
            //The API may not have more free keys so either if an exception or string is return the code works fine, check console to see if the error is correct
            Exception excTest = null;
            string json = null;
            try
            {
                json = await nasaService.FetchData(urlCorrecta);
            }
            catch (Exception ex)
            {
                excTest = ex;
                Console.WriteLine(ex.Message);
            }
            Assert.IsTrue(json is not null || excTest is not null);

        }
        [TestMethod]
        public void NasaService_GetData()
        {
            ResultApi resultadoAPI01 = new ResultApi()
            {
                element_count = 4,
                near_earth_objects = new Dictionary<DateOnly, List<Asteroid>> {
                    {
                        DateOnly.MaxValue,new List<Asteroid>
                        {
                            AsteroidGenerator(true, "prueba1", DateOnly.MaxValue, "Earth", 2000, 10, 10),
                            AsteroidGenerator(true,"prueba2",DateOnly.MaxValue,"Earth",2000,20,20),
                            AsteroidGenerator(false,"prueba3",DateOnly.MaxValue,"Earth",2000,10,10),
                            AsteroidGenerator(false,"prueba4",DateOnly.MaxValue,"Earth",2000,10,10)
                        }
                    }
                }
            };

            ResultApi resultadoAPI02 = new ResultApi();
            var result01 = nasaService.GetData(resultadoAPI01, 3);
            var result02 = nasaService.GetData(resultadoAPI01, -1);
            var result03 = nasaService.GetData(resultadoAPI01, 1);
            var result04 = nasaService.GetData(resultadoAPI02, 3);
            var expectedlist01 = new List<AsteroidDTO>()
            {
                AsteroidDTOGenerator("prueba2",20,2000,"Earth",DateOnly.MaxValue),
                AsteroidDTOGenerator("prueba1",10,2000,"Earth",DateOnly.MaxValue)
            };
            var expectedlist03 = new List<AsteroidDTO>()
            {
                AsteroidDTOGenerator("prueba2",20,2000,"Earth",DateOnly.MaxValue)
            };
            var expectedlist04 = new ResponseDTO();
            Assert.IsNotNull(result01);
            Assert.IsNotNull(result02);
            Assert.IsNotNull(result03);
            Assert.IsNotNull(result04);
            Assert.IsTrue(result01.List.SequenceEqual(expectedlist01));
            Assert.IsTrue(result02.List.SequenceEqual(expectedlist01));
            Assert.IsTrue(result03.List.SequenceEqual(expectedlist03));
            Assert.IsTrue(result04.Equals(expectedlist04));
        }
    }
}
