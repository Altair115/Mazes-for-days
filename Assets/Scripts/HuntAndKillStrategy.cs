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

        private void Kill()
        {
            while (PathStillUseable(_currentRow, _currentColumn))
            {
                string direction = RandomDirection.GetInstance().GetDirection();

                if (direction == "North" && CellAvailable(_currentRow - 1, _currentColumn))
                {
                    DestroyWall(Cells[_currentRow, _currentColumn].WallNorthObject);
                    DestroyWall(Cells[_currentRow - 1, _currentColumn].WallSouthObject);
                    _currentRow--;
                }
                else if (direction == "South" && CellAvailable(_currentRow + 1, _currentColumn))
                {
                    DestroyWall(Cells[_currentRow, _currentColumn].WallSouthObject);
                    DestroyWall(Cells[_currentRow + 1, _currentColumn].WallNorthObject);
                    _currentRow++;
                }
                else if (direction == "East" && CellAvailable(_currentRow, _currentColumn + 1))
                {
                    DestroyWall(Cells[_currentRow, _currentColumn].WallEastObject);
                    DestroyWall(Cells[_currentRow, _currentColumn + 1].WallWestObject);
                    _currentColumn++;
                }
                else if (direction == "West" && CellAvailable(_currentRow, _currentColumn - 1))
                {
                    DestroyWall(Cells[_currentRow, _currentColumn].WallWestObject);
                    DestroyWall(Cells[_currentRow, _currentColumn - 1].WallEastObject);
                    _currentColumn--;
                }

                Cells[_currentRow, _currentColumn].Visited = true;
            }
        }

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

        private bool CellAvailable(int row, int column)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns && !Cells[row, column].Visited)
                return true;
            else
                return false;
        }

        private void DestroyWall(GameObject wall)
        {
            if(wall != null)
                GameObject.Destroy(wall);
        }

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

        private void DestroyNeighboringWall(int row, int column)
        {
            bool destroyedWall = false;

            while (!destroyedWall)
            {
                string direction = RandomDirection.GetInstance().GetDirection();

                if (direction == "North" && row > 0 && Cells[row - 1, column].Visited)
                {
                    DestroyWall(Cells[row, column].WallNorthObject);
                    DestroyWall(Cells[row - 1, column].WallSouthObject);
                    destroyedWall = true;
                }
                else if (direction == "South" && row < (Rows - 2) && Cells[row + 1, column].Visited)
                {
                    DestroyWall(Cells[row, column].WallSouthObject);
                    DestroyWall(Cells[row + 1, column].WallNorthObject);
                    destroyedWall = true;
                }
                else if (direction == "West" && column > 0 && Cells[row, column - 1].Visited)
                {
                    DestroyWall(Cells[row, column].WallWestObject);
                    DestroyWall(Cells[row, column - 1].WallEastObject);
                    destroyedWall = true;
                }
                else if (direction == "East" && column < (Columns - 2) && Cells[row, column + 1].Visited)
                {
                    DestroyWall(Cells[row, column].WallEastObject);
                    DestroyWall(Cells[row, column + 1].WallWestObject);
                    destroyedWall = true;
                }
            }
        }
    }
}