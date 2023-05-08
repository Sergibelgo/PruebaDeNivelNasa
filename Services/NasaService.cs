using AutoMapper;
using PruebaDeNivelNasa.Models;

namespace PruebaDeNivelNasa.Services
{
    public class NasaService:INasaService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public NasaService(HttpClient httpClient,IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public ResponseDTO GetData(ResultadoPeticionApi dataAPI)
        {
            ResponseDTO responseDTO = new ();
            responseDTO.List = new();
            foreach (List<Asteroid> asteroids in dataAPI.near_earth_objects.Values)
            {
                foreach (Asteroid asteroid in asteroids)
                {
                    responseDTO.List.Add(_mapper.Map<AsteroidDTO>(asteroid));
                }
            }
            responseDTO.List=responseDTO.List.OrderBy(a=>a.Diametro).Take(3).ToList();
            return responseDTO;
        }

        public async Task<string> GetInfo(string url)
        {
           var json = await _httpClient.GetAsync(url);
            if (json.IsSuccessStatusCode)
            {
                return await json.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
