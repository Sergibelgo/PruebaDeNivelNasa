using AutoMapper;
using Moq;
using PruebaDeNivelNasa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class TestUnitarios
    {
        [TestMethod]
        public async Task NasaSerice()
        {
            Mock<IMapper> mapperRepo = new Mock<IMapper>();
            HttpClient httpClient = new HttpClient();
            NasaService nasaService = new(httpClient, mapperRepo.Object);

            var json = await nasaService.FetchData("https://api.nasa.gov/neo/rest/v1/feed?start_date=2021-12-09&end_date=2021-12-12&api_key=DEMO_KEY");
            var json2 = await nasaService.FetchData("http://urltotalmentefalsaysinsentido.com/");
            var json3 = await nasaService.FetchData("");
            var json4 = await nasaService.FetchData("   ");
            //For the json one first check if the API still have keys remaind, because the expected result depends on it
            //Assert.IsNotNull(json);
            Assert.IsNull(json);
            Assert.IsNull(json2);
            Assert.IsNull(json3);
            Assert.IsNull(json4);

        }
    }
}
