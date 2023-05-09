using AutoMapper;
using Moq;
using PruebaDeNivelNasa.Services;

namespace Test
{
    [TestClass]
    public class TestMoq
    {
        [TestMethod]
        public void TestConnection()
        {
            Mock<IMapper> mapperRepo = new Mock<IMapper>();
            Mock<HttpClient> httpClient = new Mock<HttpClient>();
            NasaService nasaService = new(httpClient.Object,mapperRepo.Object);
            //
            var json = nasaService.GetInfo("https://api.nasa.gov/neo/rest/v1/feed?start_date=2021-12-09&end_date=2021-12-12&api_key=DEMO_KEY");
            //Comprobamos que haya recojido bien la información
            //Si la api esta caida no se puede testear
            Assert.IsNotNull(json);
        }
    }
}