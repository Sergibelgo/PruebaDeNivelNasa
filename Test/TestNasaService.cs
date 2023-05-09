using AutoMapper;
using Moq;
using PruebaDeNivelNasa.Services;

namespace Test
{
    [TestClass]
    public class TestNasaService
    {
        private readonly NasaService _nasaService;
        public TestNasaService()
        {
            Mock<IMapper> mapperRepo = new Mock<IMapper>();
            HttpClient httpClient = new HttpClient();
            _nasaService = new(httpClient, mapperRepo.Object);
        }
        [TestMethod]
         //Test conexion a la api de la nasa
        public void TestConnection()
        {
            var json = _nasaService.GetInfo("https://api.nasa.gov/neo/rest/v1/feed?start_date=2021-12-09&end_date=2021-12-12&api_key=DEMO_KEY");
            var json2 = _nasaService.GetInfo("");
            Assert.IsNotNull(json);
            Assert.IsNull(json2);
        }
        [TestMethod]
        public void TestResponse()
        {
            
            
        }
    }
}