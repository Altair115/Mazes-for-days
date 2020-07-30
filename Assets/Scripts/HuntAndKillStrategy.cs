using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// The Hunt & Kill Algorithm
    /// </summary>
    public class HuntAndKillStrategy : MazeStrategy
    {
        private int _currentRow = 0, _currentColumn = 0;
        private bool _creationComplete = false;

        public HuntAndKillStrategy(Cell[,] cells) : base(cells)
        {
        }

        public override void Create()
        {
            HuntAndKill();
        }

        /// <summary>
        /// Finds Paths to create a Maze
        /// </summary>
        private void HuntAndKill()
        {
            Cells[_currentRow, _currentColumn].Visited = true;

            while (!_creationComplete)
            {
                Kill(); //Runs until it hits a dead end
                Hunt(); //Finds the next unvisited cell with an adjacent visited cell.
            }
        }

        /// <summary>
        /// Drunken walk or Kill until its not possible anymore
        /// </summary>
        private void Kill()
        {
            while (PathStillUseable(_currentRow, _currentColumn))
            {
                int direction = RandomDirection.Random();

                if (direction == 0 && CellAvailable(_currentRow - 1, _currentColumn)) //North
                {
                    DestroyWall(Cells[_currentRow, _currentColumn].WallNorthObject);
                    DestroyWall(Cells[_currentRow - 1, _currentColumn].WallSouthObject);
                    _currentRow--;
                }
                else if (direction == 1 && CellAvailable(_currentRow + 1, _currentColumn)) //South
                {
                    DestroyWall(Cells[_currentRow, _currentColumn].WallSouthObject);
                    DestroyWall(Cells[_currentRow + 1, _currentColumn].WallNorthObject);
                    _currentRow++;
                }
                else if (direction == 3 && CellAvailable(_currentRow, _currentColumn + 1)) //East
                {
                    DestroyWall(Cells[_currentRow, _currentColumn].WallEastObject);
                    DestroyWall(Cells[_currentRow, _currentColumn + 1].WallWestObject);
                    _currentColumn++;
                }
                else if (direction == 2 && CellAvailable(_currentRow, _currentColumn - 1)) //West
                {
                    DestroyWall(Cells[_currentRow, _currentColumn].WallWestObject);
                    DestroyWall(Cells[_currentRow, _currentColumn - 1].WallEastObject);
                    _currentColumn--;
                }

                Cells[_currentRow, _currentColumn].Visited = true;
            }
        }

        /// <summary>
        /// Finds the next unvisited cell with an adjacent visited cell to start killing or walking again
        /// True until proven other wise by it
        /// </summary>
        private void Hunt()
        {
            _creationComplete = true;

            for (var r = 0; r < Rows; r++)
            {
                for (var c = 0; c < Columns; c++)
                {
                    if (!Cells[r, c].Visited && CellHasVisitedNeighborCell(r, c))
                    {
                        _creationComplete = false; //We found something so, do another Kill cycle.
                        _currentRow = r;
                        _currentColumn = c;
                        DestroyNeighboringWall(_currentRow, _currentColumn);
                        Cells[_currentRow, _currentColumn].Visited = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Checks if the path is still useable or not by checking if the tiles around it are visited
        /// </summary>
        /// <param name="row">used for checking around the seeker on the row axis</param>
        /// <param name="column">used for checking around the seeker on the column axis</param>
        /// <returns></returns>
        private bool PathStillUseable(int row, int column)
        {
            var useablePaths = 0;

            if (row > 0 && !Cells[row - 1, column].Visited)
                useablePaths++;

            if (row < Rows - 1 && !Cells[row + 1, column].Visited)
                useablePaths++;

            if (column > 0 && !Cells[row, column - 1].Visited)
                useablePaths++;

            if (column < Columns - 1 && !Cells[row, column + 1].Visited)
                useablePaths++;

            return useablePaths > 0;
        }

        /// <summary>
        /// check if the cell is available
        /// </summary>
        /// <param name="row">the row position of the cell</param>
        /// <param name="column">the column position of the cell</param>
        /// <returns></returns>
        private bool CellAvailable(int row, int column)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns && !Cells[row, column].Visited)
                return true;
            else
                return false;
        }

        /// <summary>
        /// destroys a wall when building new paths to reach new cells
        /// </summary>
        /// <param name="wall">the wall in question</param>
        private void DestroyWall(GameObject wall)
        {
            if(wall != null)
                GameObject.Destroy(wall);
        }

        /// <summary>
        /// check if cell has visited neigbors
        /// </summary>
        /// <param name="row">the row position of the cell</param>
        /// <param name="column">the column position of the cell</param>
        /// <returns></returns>
        private bool CellHasVisitedNeighborCell(int row, int column)
        {
            var neighborsVisited = 0;

            // Look 1 row up (north) if we're on row 1 or greater
            if (row > 0 && Cells[row - 1, column].Visited)
                neighborsVisited++;

            // Look one row down (south) if we're the second-to-last row (or less)
            if (row < (Rows - 2) && Cells[row + 1, column].Visited)
                neighborsVisited++;

            // Look one row left (west) if we're column 1 or greater
            if (column > 0 && Cells[row, column - 1].Visited)
                neighborsVisited++;

            // Look one row right (east) if we're the second-to-last column (or less)
            if (column < (Columns - 2) && Cells[row, column + 1].Visited)
                neighborsVisited++;

            // return true if there are any adjacent visited cells to this one
            return neighborsVisited > 0;
        }

        /// <summary>
        /// destroys the neighboring wall to continue building the maze
        /// </summary>
        /// <param name="row">the row position of the cell</param>
        /// <param name="column">the column position of the cell</param>
        private void DestroyNeighboringWall(int row, int column)
        {
            bool destroyedWall = false;

            while (!destroyedWall)
            {
                int direction = RandomDirection.Random();

                if (direction == 0 && row > 0 && Cells[row - 1, column].Visited) //North
                {
                    DestroyWall(Cells[row, column].WallNorthObject);
                    DestroyWall(Cells[row - 1, column].WallSouthObject);
                    destroyedWall = true;
                }
                else if (direction == 1 && row < (Rows - 2) && Cells[row + 1, column].Visited) //South
                {
                    DestroyWall(Cells[row, column].WallSouthObject);
                    DestroyWall(Cells[row + 1, column].WallNorthObject);
                    destroyedWall = true;
                }
                else if (direction == 3 && column > 0 && Cells[row, column - 1].Visited) //West
                {
                    DestroyWall(Cells[row, column].WallWestObject);
                    DestroyWall(Cells[row, column - 1].WallEastObject);
                    destroyedWall = true;
                }
                else if (direction == 2 && column < (Columns - 2) && Cells[row, column + 1].Visited) //East
                {
                    DestroyWall(Cells[row, column].WallEastObject);
                    DestroyWall(Cells[row, column + 1].WallWestObject);
                    destroyedWall = true;
                }
            }
        }
    }
}