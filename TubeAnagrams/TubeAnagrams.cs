using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace tubeAnagrams
{
    public class TubeAnagrams
    {
        public TubeAnagrams(ITflApi tflApi)
        {
            var randomPicker = new RandomPicker<string>();
            var game = new Game(tflApi, randomPicker);
            game.Play();
        }
    }
}