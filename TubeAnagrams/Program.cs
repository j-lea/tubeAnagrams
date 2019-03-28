using Microsoft.Extensions.DependencyInjection;

namespace tubeAnagrams
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            const string url = "https://api.tfl.gov.uk/";
            
            var tflApi = new TflApi(url);
            var tubeAnagrams = new TubeAnagrams(tflApi);
        }
    }
}