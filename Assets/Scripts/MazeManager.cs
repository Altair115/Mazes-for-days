using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Manager Class that manages the creation of the maze
/// </summary>
public class MazeManager : MonoBehaviour
{
    public int Rows, Columns;
    public GameObject[] Walls;
    public GameObject aFloor;
    public float Size = 1f; //Should be the same size as the walls and or floor

    public GameObject Player;
    public Camera Camera;
    public GameObject Menu;
    public InputField WidthField;
    public InputField HeightField;

    private Cell[,] _cells;
    private GameObject _maze;
    private float _cameraOffset = 2.5f;

    void Start()
    {
        _maze = new GameObject();
        _maze.name = "Maze";

        WidthField.characterValidation = InputField.CharacterValidation.Integer;
        HeightField.characterValidation = InputField.CharacterValidation.Integer;
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
                    if (Random.value > 0.8) //%20 percent chance
                        _cells[r, c].WallWestObject = Instantiate(Walls[1], new Vector3(r * Size, 0, (c * Size) - (Size / 2f)), Quaternion.identity) as GameObject;
                    else
                        _cells[r, c].WallWestObject = Instantiate(Walls[0], new Vector3(r * Size, 0, (c * Size) - (Size / 2f)), Quaternion.identity) as GameObject;
                    _cells[r, c].WallWestObject.name = "West Wall " + r + "," + c;
                    _cells[r, c].WallWestObject.transform.parent = _maze.transform;
                }

                if (Random.value > 0.8) //%20 percent chance
                    _cells[r, c].WallEastObject = Instantiate(Walls[1], new Vector3(r * Size, 0, (c * Size) + (Size / 2f)), Quaternion.identity) as GameObject;
                else
                    _cells[r, c].WallEastObject = Instantiate(Walls[0], new Vector3(r * Size, 0, (c * Size) + (Size / 2f)), Quaternion.identity) as GameObject;
                _cells[r, c].WallEastObject.name = "East Wall " + r + "," + c;
                _cells[r, c].WallEastObject.transform.parent = _maze.transform;
                _cells[r, c].WallEastObject.transform.Rotate(Vector3.up * 180f);

                if (r == 0)
                {
                    if (Random.value > 0.8) //%20 percent chance
                        _cells[r, c].WallNorthObject = Instantiate(Walls[1], new Vector3((r * Size) - (Size / 2f), 0, c * Size), Quaternion.identity) as GameObject;
                    else
                        _cells[r, c].WallNorthObject = Instantiate(Walls[0], new Vector3((r * Size) - (Size / 2f), 0, c * Size), Quaternion.identity) as GameObject;
                    _cells[r, c].WallNorthObject.name = "North Wall " + r + "," + c;
                    _cells[r, c].WallNorthObject.transform.parent = _maze.transform;
                    _cells[r, c].WallNorthObject.transform.Rotate(Vector3.up * 90f);
                }
                if (Random.value > 0.8) //%20 percent chance
                    _cells[r, c].WallSouthObject = Instantiate(Walls[1], new Vector3((r * Size) + (Size / 2f), 0, c * Size), Quaternion.identity) as GameObject;
                else
                    _cells[r, c].WallSouthObject = Instantiate(Walls[0], new Vector3((r * Size) + (Size / 2f), 0, c * Size), Quaternion.identity) as GameObject;
                _cells[r, c].WallSouthObject.name = "South Wall " + r + "," + c;
                _cells[r, c].WallSouthObject.transform.parent = _maze.transform;
                _cells[r, c].WallSouthObject.transform.Rotate(Vector3.up * 270f);
            }
        }
    }

    /// <summary>
    /// Simple Clearing function for the maze
    /// </summary>
    private void PurgeMaze()
    {
        foreach (Transform child in _maze.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// (Re)generates the maze with given parameters
    /// </summary>
    public void RegenerateMaze()
    {
        PurgeMaze();

        Rows = int.Parse(WidthField.text);
        Columns = int.Parse(HeightField.text);

        InitializeMaze();

        Camera.transform.position = new Vector3(((Rows / 2) * Size) - _cameraOffset, ((Rows + Columns) / 3) * Size, ((Columns / 2) * Size) - _cameraOffset);
        Camera.orthographicSize = (((Rows + Columns) / 3) * Size);

        MazeStrategy mazeStrategy = new HuntAndKillStrategy(_cells);
        mazeStrategy.Create();
    }

    /// <summary>
    /// Summon Playable Character into the maze
    /// </summary>
    public void SummonPlayer()
    {
        if (Player.activeInHierarchy)
        {
            Player.transform.position = new Vector3(0, 1, 0);
        }
        else
        {
            Player = Instantiate(Player, new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
        }
    }

    /// <summary>
    /// Toggles the Maze menu with an animation
    /// </summary>
    public void ToggleMenu()
    {
        if (Menu != null)
        {
            Animator animator = Menu.GetComponent<Animator>();
            if (animator != null)
            {
                bool IsOpen = animator.GetBool("IsOpen");
                animator.SetBool("IsOpen", !IsOpen);
            }
        }
    }
}
