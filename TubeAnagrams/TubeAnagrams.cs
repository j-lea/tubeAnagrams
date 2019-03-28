using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace tubeAnagrams
{
    public class TubeAnagrams
    {
        public TubeAnagrams(ITflApi tflApi)
        {
            var game = new Game(tflApi);
            game.Play();
        }
    }
}