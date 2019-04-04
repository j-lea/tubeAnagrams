using System.Collections.Generic;
using System.Threading.Tasks;

namespace tubeAnagrams
{
    public interface ITflApi
    {
        string[] GetStationsForLine(string line);
    }
}