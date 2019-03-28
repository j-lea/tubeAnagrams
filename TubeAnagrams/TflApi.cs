using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace tubeAnagrams
{
    public class TflApi : ITflApi
    {
        private static HttpClient Client { get; } = new HttpClient();
        private readonly string _apiUrl;
        
        public TflApi(string url)
        {
            _apiUrl = url;
        }

        public virtual string[] GetStationsForLine(string line)
        {
            Client.BaseAddress = new Uri(_apiUrl);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));     

            line = line.ToLower();
            
            var response = Client.GetAsync($"line/{line}/stoppoints").Result;

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