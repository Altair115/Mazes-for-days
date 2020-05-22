using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class RandomDirection
    {
        private static readonly RandomDirection Instance = new RandomDirection();

        private RandomDirection(){}

        private readonly List<string> _directions = new List<string>(){"North", "South", "West", "East"};

        public static RandomDirection GetInstance()
        {
            return Instance;
        }

        public string GetDirection()
        {
            return _directions[Random.Range(0, _directions.Count)];
        }
    }
}
