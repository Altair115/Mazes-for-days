using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class MazeManager : MonoBehaviour
{
    public int Rows, Columns;
    public GameObject aWall;
    public GameObject aFloor;
    public float Size = 1f; //Should be the same size as the walls and or floor

    public Camera Camera;

    private Cell[,] _cells;
    private GameObject _maze;


    void Start()
    {
        _maze = new GameObject();
        _maze.name = "Maze";

        InitializeMaze();

        Camera.transform.position = new Vector3((Rows / 2 * Size),(Rows + Columns) / 2 * Size,(Columns / 2 * Size) - 2.5f);
        
        MazeStrategy mazeStrategy = new HuntAndKillStrategy(_cells);
        mazeStrategy.Create();
    }

    /// <summary>
    /// Initialization of the maze
    /// </summary>
    private void InitializeMaze()
    {
        _cells = new Cell[Rows, Columns];

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                _cells[r, c] = new Cell();

                _cells[r, c].FloorObject = Instantiate(aFloor, new Vector3(r * Size, -(Size / 2f), c *Size), Quaternion.identity) as GameObject;
                _cells[r, c].FloorObject.name = "Floor " + r + "," + c;
                _cells[r, c].FloorObject.transform.parent = _maze.transform;

                if (c == 0)
                {
                    _cells[r, c].WallWestObject = Instantiate(aWall, new Vector3(r * Size, 0, (c * Size) - (Size / 2f)), Quaternion.identity) as GameObject;
                    _cells[r, c].WallWestObject.name = "West Wall " + r + "," + c;
                    _cells[r, c].WallWestObject.transform.parent = _maze.transform;
                }

                _cells[r, c].WallEastObject = Instantiate(aWall, new Vector3(r * Size, 0, (c * Size) + (Size / 2f)), Quaternion.identity) as GameObject;
                _cells[r, c].WallEastObject.name = "East Wall " + r + "," + c;
                _cells[r, c].WallEastObject.transform.parent = _maze.transform;

                if (r == 0)
                {
                    _cells[r, c].WallNorthObject = Instantiate(aWall, new Vector3((r * Size) - (Size / 2f), 0, c * Size), Quaternion.identity) as GameObject;
                    _cells[r, c].WallNorthObject.name = "North Wall " + r + "," + c;
                    _cells[r, c].WallNorthObject.transform.parent = _maze.transform;
                    _cells[r, c].WallNorthObject.transform.Rotate(Vector3.up * 90f);
                }

                _cells[r, c].WallSouthObject = Instantiate(aWall, new Vector3((r * Size) + (Size / 2f), 0, c * Size), Quaternion.identity) as GameObject;
                _cells[r, c].WallSouthObject.name = "South Wall " + r + "," + c;
                _cells[r, c].WallSouthObject.transform.parent = _maze.transform;
                _cells[r, c].WallSouthObject.transform.Rotate(Vector3.up * 90f);
            }
        }
    }
}
