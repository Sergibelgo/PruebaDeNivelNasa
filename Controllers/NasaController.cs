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

        public NasaController( INasaService nasaService,IJSONService jSONService,IDateService dateService,IMapper mapper)
        {
            _nasaService = nasaService;
            _JSONService = jSONService;
            _dateService = dateService;
            _mapper = mapper;
        }
        [HttpGet("days:int")]
        public async Task<IActionResult> GetInfo(int days)
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate;
            try
            {
                endDate = await _dateService.GetDate(startDate, days);
            }
            catch
            {
                return BadRequest("Invalid number of days, it must be between 1 and 7");
            }
            ResultadoPeticionApi data = await _nasaService.GetInfo(startDate, endDate);
            if (data is null)
            {
                return BadRequest("The data could not be fetched from the API");
            }
            string response = _JSONService.ConvertData(data);
            return Ok(response);
        }
        [HttpGet("APiCaida")]
        public async Task<IActionResult> GetFromJson()
        {
            string data;
            try {
                using StreamReader reader = new StreamReader("./Resources/testJson.json");
                data = reader.ReadToEnd();
            }
            catch(FileNotFoundException)
            {
                return BadRequest("The file was not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            string response = _JSONService.ConvertData(data);
            return Ok(response);
        }

    }
}
