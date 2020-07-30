using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// A Data class that used for building the maze
    /// </summary>
    public struct Cell
    {
        public bool Visited;
        public GameObject WallNorthObject, WallSouthObject, WallEastObject, WallWestObject, FloorObject;
    }
}
