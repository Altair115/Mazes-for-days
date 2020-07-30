using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Direction Randomizer Singleton
    /// </summary>
    public static class RandomDirection
    {
        public const int North = 0;
        public const int South = 1;
        public const int West = 2;
        public const int East = 3;
        public static int Random() => UnityEngine.Random.Range(0, 4);
    }
}
