using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace tubeAnagrams
{
    public class TflApi : ITflApi
    {
        private static HttpClient Client { get; } = new HttpClient();

        public virtual string[] GetStationsForLine(string line)
        {
            line = line.ToLower();
            
            Client.BaseAddress = new Uri($"https://api.tfl.gov.uk/");
            
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));     

            var response = Client.GetAsync($"line/{line}/stoppoints").Result;  // Blocking call!    

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var tubeStations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<dynamic>>(result);
                
                var stationNames = new List<string>();
                foreach (var tubeStation in tubeStations)
                {
                    string stationName = tubeStation["commonName"].ToString().Replace("Underground Station", "").Trim();
                    stationNames.Add(stationName);
                }

                return stationNames.ToArray();
            }
            else
            {
                throw new InvalidDataException();
            }
        }
    }
}