using UnityEngine;

namespace Assets.Scripts
{
    public class Cell
    {
        public bool Visited = false;
        public GameObject WallNorthObject, WallSouthObject, WallEastObject, WallWestObject, FloorObject;
    }
}
