using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// A Data class that used for building the maze
    /// </summary>
    public class Cell
    {
        public bool Visited = false;
        public GameObject WallNorthObject, WallSouthObject, WallEastObject, WallWestObject, FloorObject;
    }
}
