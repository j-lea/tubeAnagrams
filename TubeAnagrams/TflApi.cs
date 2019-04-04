using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace tubeAnagrams
{
    public class TflApi : ITflApi
    {
//        private HttpClient Client { get; } = new HttpClient();
        private readonly string _apiUrl;
        
        public TflApi(string url)
        {
            _apiUrl = url;
        }

        public string[] GetStationsForLine(string line)
        {
            var client = new HttpClient {BaseAddress = new Uri(_apiUrl)};
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));     

            line = line.ToLower();
            
            var response = client.GetAsync($"line/{line}/stoppoints").Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidDataException();
            }
            
            var result = response.Content.ReadAsStringAsync().Result;
            var tubeStations = JsonConvert.DeserializeObject<List<dynamic>>(result);

//            var stationNames = tubeStations.Select(x => 
//                x["commonName"].ToString().Replace("Underground Station", "").Trim());

            var stationNames = new List<string>();
            foreach (var tubeStation in tubeStations)
            {
                string stationName = tubeStation["commonName"].ToString().Replace("Underground Station", "").Trim();
                stationNames.Add(stationName);
            }

            return stationNames.ToArray();
        }
    }
}