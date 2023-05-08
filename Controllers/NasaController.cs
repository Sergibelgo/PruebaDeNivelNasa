using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaDeNivelNasa.Services;

namespace PruebaDeNivelNasa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NasaController : ControllerBase
    {
        private readonly string _url= "https://api.nasa.gov/neo/rest/v1/feed";
        private readonly INasaService _nasaService;

        public NasaController( INasaService nasaService)
        {
            _nasaService = nasaService;
        }
    }
}
