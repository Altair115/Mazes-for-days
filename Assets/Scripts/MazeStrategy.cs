using UnityEditor;

namespace Assets.Scripts
{
    public abstract class MazeStrategy
    {
        protected Cell[,] Cells;
        protected int Rows, Columns;

        protected MazeStrategy(Cell[,] cells) : base()
        {
            this.Cells = cells;
            Rows = cells.GetLength(0);
            Columns = cells.GetLength(1);
        }

        public abstract void Create();
    }
}
