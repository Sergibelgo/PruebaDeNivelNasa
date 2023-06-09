﻿using AutoMapper;
using PruebaDeNivelNasa.Models.DTOS;
using PruebaDeNivelNasa.Models.ResultAPI;
using PruebaDeNivelNasa.Services.Interfaces;

namespace PruebaDeNivelNasa.Services.Classes
{
    /// <summary>
    /// Service for the procedures realated to the API Nasa buissness
    /// </summary>
    public class NasaService : INasaService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;

        public NasaService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }
        /// <summary>
        /// Turns the result of the API (Saved on the object type "ResultAPI") into an objecto of tipe "ResponseDTO"
        /// </summary>
        /// <param name="dataAPI">The data fetched from the api</param>
        /// <param name="limit">The limit of asteroids to take</param>
        /// <returns>An object of type ResponseDTO</returns>

        public ResponseDTO GetData(ResultApi dataAPI, int limit = 3)
        {
            if (dataAPI is null)
            {
                return null;
            }
            int validLimit = limit < 1 ? 3 : limit;
            limit = validLimit;
            ResponseDTO responseDTO = new()
            {
                List = new()
            };
            foreach (List<Asteroid> asteroids in dataAPI.near_earth_objects.Values)
            {
                var hazarOnes = GetHazardOnes(asteroids);
                foreach (Asteroid asteroid in hazarOnes)
                {
                    responseDTO.List.Add(_mapper.Map<AsteroidDTO>(asteroid));
                }
            }
            LimitList(limit, responseDTO);
            return responseDTO;
        }
        /// <summary>
        /// Method to get the object of type Asteroid from a list which have the attribute "is_potentially_hazardous_asteroid" on true
        /// </summary>
        /// <param name="asteroids">The list of object of type "Asteroid"</param>
        /// <returns>An IEnumerable of objects type "Asteroid"</returns>
        private IEnumerable<Asteroid> GetHazardOnes(List<Asteroid> asteroids)
        {
            var list = asteroids.Where(a => a.is_potentially_hazardous_asteroid == true);
            return list;
        }
        /// <summary>
        /// Method to fetch the data from the url given
        /// </summary>
        /// <param name="url">Url where the data will be fetch</param>
        /// <returns>String content of the url if the status code is success or null if the url was not valid</returns>
        /// <exception cref="Exception">Throw if the api returns a non success status code</exception>
        public async Task<string> FetchData(string url)
        {
            if (url is null || url == string.Empty)
            {
                throw new Exception("400__{error:'The URL could not be resolved'}");
            }
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(url);
            }
            catch
            {
                throw new Exception("400__{error:'The URL could not be resolved'}");
            }
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                int errorCode = (int)response.StatusCode;
                string message = await response.Content.ReadAsStringAsync();
                throw new Exception($"{errorCode}__{message}");
            }
        }
        /// <summary>
        /// Method to limit and order the list given by the number given
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="response">The object of type ResponseDTO to order and limit</param>
        private void LimitList(int limit, ResponseDTO response)
        {
            response.List = response.List.OrderByDescending(a => a.Diametro).Take(limit).ToList();
        }
    }
}
