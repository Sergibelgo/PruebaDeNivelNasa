using AutoMapper;
using Moq;
using PruebaDeNivelNasa.Services;

namespace Test
{
    [TestClass]
    public class TestMoq
    {
        private readonly NasaService _nasaService;
        public TestMoq()
        {
            Mock<IMapper> mapperRepo = new Mock<IMapper>();
            HttpClient httpClient = new HttpClient();
            _nasaService = new(httpClient, mapperRepo.Object);
        }
        [TestMethod]
        public void TestConnection()
        {
            var json = _nasaService.GetInfo("https://api.nasa.gov/neo/rest/v1/feed?start_date=2021-12-09&end_date=2021-12-12&api_key=DEMO_KEY");
            Assert.IsNotNull(json);
        }
        [TestMethod]
        public void TestResponse()
        {
            Mock<IMapper> mapperRepo = new Mock<IMapper>();
            HttpClient httpClient = new HttpClient();
            NasaService nasaService = new(httpClient, mapperRepo.Object);
            
        }
    }
}