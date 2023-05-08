using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaDeNivelNasa.Models;
using PruebaDeNivelNasa.Services;

namespace PruebaDeNivelNasa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NasaController : ControllerBase
    {
        private readonly INasaService _nasaService;
        private readonly IJSONService _JSONService;
        private readonly IDateService _dateService;
        private readonly IMapper _mapper;
        private readonly string _url;

        public NasaController(INasaService nasaService, IJSONService jSONService, IDateService dateService, IMapper mapper,IConfiguration configuration)
        {
            _nasaService = nasaService;
            _JSONService = jSONService;
            _dateService = dateService;
            _mapper = mapper;
            _url = configuration.GetValue<string>("APIURL");
        }
        [HttpGet("days:int")]
        public async Task<IActionResult> GetInfo(int days)
        {
            DateTime startDate = DateTime.Now;
            if (days < 1 || days > 7)
            {
                return BadRequest("Invalid number of days, it must be between 1 and 7");
            }
            DateTime endDate = _dateService.GetDate(startDate, days);
            string url = _JSONService.GetUrl(_url, startDate, endDate);
            string data = await _nasaService.GetInfo(url);
            if (data is null)
            {
                return BadRequest("The data could not be fetched from the API");
            }
            ResultadoPeticionApi dataAPI = _JSONService.ConvertData(data);
            ResponseDTO responseDTO = _nasaService.GetData(dataAPI);
            string response = _JSONService.GetResult(responseDTO);
            return Ok(response);
        }
        [HttpGet("APiCaida")]
        public IActionResult GetFromJson()
        {
            string data;
            try
            {
                using StreamReader reader = new("./Resources/testJson.json");
                data = reader.ReadToEnd();
            }
            catch (FileNotFoundException)
            {
                return BadRequest("The file was not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            ResultadoPeticionApi dataAPI = _JSONService.ConvertData(data);
            ResponseDTO response = _nasaService.GetData(dataAPI);
            return Ok(JsonConvert.SerializeObject(response));
        }

    }
}
