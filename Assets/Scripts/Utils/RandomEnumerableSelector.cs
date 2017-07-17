using System;
using System.Collections.Generic;
using System.Linq;

namespace BlindWizard.Utils
{
    public static class RandomEnumerableSelector
    {
        private static Random _random;
        private static Random Rand => _random ?? (_random = new Random());
        
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable.ToList();
            return list[Rand.Next(0, list.Count)];
        }
    }
}