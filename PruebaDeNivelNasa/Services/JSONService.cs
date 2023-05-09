using AutoMapper;
using Newtonsoft.Json;
using PruebaDeNivelNasa.Models;

namespace PruebaDeNivelNasa.Services
{
    public class JSONService : IJSONService
    {
        public JSONService()
        {
        }

        public ResultadoPeticionApi ConvertData(string data)
        {
            return JsonConvert.DeserializeObject<ResultadoPeticionApi>(data);
        }

        public string GetResult(ResponseDTO responseDTO)
        {
            return JsonConvert.SerializeObject(responseDTO);
        }

        public string GetUrl(string url, DateTime startDate, DateTime endDate,string key)
        {
            string response = $"{url}?start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}&api_key=DEMO_KEY";
            return response ;
        }
    }
}
