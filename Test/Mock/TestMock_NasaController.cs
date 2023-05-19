using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using PruebaDeNivelNasa.Controllers;
using PruebaDeNivelNasa.Models.DTOS;
using PruebaDeNivelNasa.Models.ResultAPI;
using PruebaDeNivelNasa.Services.Classes;
using PruebaDeNivelNasa.Services.Interfaces;
using static Test.Utils.Utils;

namespace Test.Mock
{
    //TODO: separa por carpetas los test de mock de los unitarios
    //Fixed
    [TestClass]
    public class TestMock_NasaController
    {
        private readonly IConfiguration _configuration;
        public Mock<INasaService> mockNasa;
        public Mock<IJSONService> mockJson;
        public Mock<IDateService> mockIDate;
        public MapperConfiguration config = new(cfg =>
        {
            cfg.CreateMap<Asteroid, AsteroidDTO>()
            .ForMember(dto => dto.Nombre, ent => ent.MapFrom(x => x.name))
            .ForMember(dto => dto.Fecha, ent => ent.MapFrom(x => x.close_approach_data[0].close_approach_date))
            .ForMember(dto => dto.Planeta, ent => ent.MapFrom(x => x.close_approach_data[0].orbiting_body))
            .ForMember(dto => dto.Velocidad, ent => ent.MapFrom(x => x.close_approach_data[0].relative_velocity.kilometers_per_hour))
            .ForMember(dto => dto.Diametro, ent => ent.MapFrom(x => (x.estimated_diameter.kilometers.estimated_diameter_max + x.estimated_diameter.kilometers.estimated_diameter_min) / 2));
        });
        public TestMock_NasaController()
        {
            //TODO: hay que evitar el hardcodeo de urls
            //Este es necesario por que estoy creando el configuration que simulara el controlador
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
        //Fixed
        [TestMethod]
        public async Task TestResponseNormal()
        {
            //Arrange
            Dictionary<string, string> DataForResponse = GetDataForResponseTest();
            var resultNormal = ResultNormal();
            var responseNormal = ResponseNormal();
            //TODO: declaras el mismo objeto en varios métodos, se puede declarar como propiedad de la clase
            mockNasa = new();
            mockJson = new();
            mockIDate = new();

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
            mockNasa = new();
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
            mockNasa = new();

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
            mockNasa = new();
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
        //Fixed
        [TestMethod]
        public async Task TestHttpClient_Normal()
        {
            //Arrange
            var mockHttpClientHandler = ConfigureMoqHTTP("../../../FileResponse01.json");
            var nasaService = new NasaService(new HttpClient(mockHttpClientHandler.Object), new Mapper(config));

            //Act
            NasaController nasaController = new(nasaService, new JSONService(), new DateService(), _configuration);
            var result1 = await nasaController.GetInfo(3);
            //Assert
            Assert.IsNotNull(result1);
            Assert.IsInstanceOfType(result1, typeof(OkObjectResult));

        }
        [TestMethod]
        public async Task TestHttpClient_Null()
        {
            //Arrange
            var mockHttpClientHandler = ConfigureMoqHTTP(null);
            var nasaService = new NasaService(new HttpClient(mockHttpClientHandler.Object), new Mapper(config));
            //Act
            NasaController nasaController = new(nasaService, new JSONService(), new DateService(), _configuration);
            var result2 = await nasaController.GetInfo(3);
            //Assert
            Assert.IsNotNull(result2);
            Assert.IsInstanceOfType(result2, typeof(BadRequestObjectResult));

        }
        [TestMethod]
        public async Task TestHttpClient_InvalidJSON()
        {
            //Arrange
            var mockHttpClientHandler = ConfigureMoqHTTP("../../../FileResponse02.json");
            var nasaService = new NasaService(new HttpClient(mockHttpClientHandler.Object), new Mapper(config));

            //Act
            NasaController nasaController = new(nasaService, new JSONService(), new DateService(), _configuration);
            var result3 = await nasaController.GetInfo(3);
            //Assert
            Assert.IsNotNull(result3);
            Assert.IsInstanceOfType(result3, typeof(UnprocessableEntityObjectResult));

        }

    }
}