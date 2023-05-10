using Newtonsoft.Json;

namespace PruebaDeNivelNasa.Services
{
    /// <summary>
    /// Service for all text related procedures
    /// </summary>
    public class JSONService : IJSONService
    {
        /// <summary>
        /// Method to convert a json string into an object of type "T"
        /// </summary>
        /// <param name="data">String with json format to convert</param>
        /// <returns>An objecto of type ResultApi or null if the string could not be converted</returns>
        public T ConvertData<T>(string data)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(data);
                return result;
            }
            catch
            {
                return default;
            }

        }
        /// <summary>
        /// Method to serialize an object into a json
        /// </summary>
        /// <param name="model">The object to convert</param>
        /// <returns>A JSON type string of the object</returns>
        public string GetResult<T>(T model)
        {
            return JsonConvert.SerializeObject(model);
        }
        /// <summary>
        /// Method to construct the url for the API
        /// </summary>
        /// <param name="url">Basic url to the api</param>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="key">Key for the api, if is not a valid key the api will return 400</param>
        /// <returns>String with the full url with parameters to the API</returns>
        public string GetUrl(string url, DateTime startDate, DateTime endDate, string key)
        {
            string response = $"{url}?start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}&api_key={key}";
            return response;
        }
    }
}
