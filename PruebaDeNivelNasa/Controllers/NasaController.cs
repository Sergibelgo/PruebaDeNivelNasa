using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaDeNivelNasa.Models;
using PruebaDeNivelNasa.Services;

namespace PruebaDeNivelNasa.Controllers
{
    [Route("asteroids")]
    [ApiController]
    public class NasaController : ControllerBase
    {
        private readonly INasaService _nasaService;
        private readonly IJSONService _JSONService;
        private readonly IDateService _dateService;
        private readonly string _url;

        public NasaController(INasaService nasaService, IJSONService jSONService, IDateService dateService, IConfiguration configuration)
        {
            _nasaService = nasaService;
            _JSONService = jSONService;
            _dateService = dateService;
            _url = configuration.GetValue<string>("APIURL");
        }
        /// <summary>
        /// Endpoint to control the basic function of the API
        /// </summary>
        /// <param name="days">The number of days from today to check, required</param>
        /// <param name="key">Key to the nasa API, default value is "DEMO_KEY"</param>
        /// <param name="limit">Limit of max results to return,default value is 3</param>
        /// <returns>An object of type ResponseDTO which contains a list of hazard asteroids</returns>
        [HttpGet]
        public async Task<IActionResult> GetInfo(int? days, string key = "DEMO_KEY", int limit = 3)
        {
            if (days is null)
            {
                return BadRequest("The query parameter 'days' is necesary, please add it to use the API");
            }
            if (days < 1 || days > 7)
            {
                return BadRequest("Invalid number of days, it must be between 1 and 7");
            }
            if (limit < 1)
            {
                return BadRequest("The limit must be positive and bigger than 0");
            }
            DateTime startDate = DateTime.Now;
            DateTime endDate = _dateService.GetDate(startDate, (int)days);
            string url = _JSONService.GetUrl(_url, startDate, endDate, key);
            string data = await _nasaService.FetchData(url);
            if (data is null)
            {
                return BadRequest("The data could not be fetched from the API");
            }
            ResultApi dataAPI = _JSONService.ConvertData<ResultApi>(data);
            ResponseDTO responseDTO = _nasaService.GetData(dataAPI, limit);
            if (responseDTO is null || responseDTO.List.Count == 0)
            {
                return Ok($"There are no hazard asterois between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}");
            }
            string response = _JSONService.GetResult(responseDTO);
            return Ok(response);
        }
        /// <summary>
        /// Special endpoint for when the API has no more free uses, it needs a json with the information on a folder call "Resources"
        /// </summary>
        /// <returns>An object of type ResponseDTO which contains a list of hazard asteroids</returns>
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
            ResultApi dataAPI = _JSONService.ConvertData<ResultApi>(data);
            ResponseDTO response = _nasaService.GetData(dataAPI, 3);
            return Ok(JsonConvert.SerializeObject(response));
        }

    }
}
