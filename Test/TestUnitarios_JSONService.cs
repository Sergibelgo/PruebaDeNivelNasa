using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using PruebaDeNivelNasa.Models;
using PruebaDeNivelNasa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class TestUnitarios_JSONService
    {
        private JSONService jsonService;

        public TestUnitarios_JSONService()
        {
            jsonService = new JSONService();
        }
        [TestMethod]
        public async Task JSONService_GetUrl()
        {
            string urlBase = "https://api.nasa.gov/neo/rest/v1/feed";
            JSONServiceDataGetUrl data01 = new JSONServiceDataGetUrl()
            {
                StartDate = DateTime.Now,
                Key = "asd",
                EndDate = DateTime.Now,
                Url = urlBase
            };
            JSONServiceDataGetUrl data02 = new JSONServiceDataGetUrl()
            {
                Url = "dsadasda",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            JSONServiceDataGetUrl data03 = new JSONServiceDataGetUrl()
            {
                Url = urlBase,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            var result01 = jsonService.GetUrl(data01.Url,data01.StartDate,data01.EndDate,data01.Key);
            _ = Assert.ThrowsException<Exception>(()=>jsonService.GetUrl(data02.Url, data02.StartDate, data02.EndDate));
            var result03 = jsonService.GetUrl(data03.Url,data03.StartDate,data03.EndDate);

            Assert.AreEqual(result01, $"{urlBase}?start_date={DateTime.Now:yyyy-MM-dd}&end_date={DateTime.Now:yyyy-MM-dd}&api_key={data01.Key}");
            Assert.AreEqual(result03, $"{urlBase}?start_date={DateTime.Now:yyyy-MM-dd}&end_date={DateTime.Now:yyyy-MM-dd}&api_key=DEMO_KEY");
        }
    }
}
