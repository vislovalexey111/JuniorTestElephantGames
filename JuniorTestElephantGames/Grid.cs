namespace JuniorTestElephantGames
{
    public class Grid
    {
        private readonly Cell[,] _cells;
        private int _size;

        #region Properties
        public Cell[,] Cells => _cells;

        public int Size
        {
            get { return _size; }
            private set
            {
                if( value < 3)
                    _size = 3;
                else if(value > 5)
                    _size = 5;
                else
                    _size = value;
            }
        }
        #endregion

        public Grid (int gridSize = 3)
        {
            Size = gridSize;
            _cells = new Cell[Size, Size];

            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                    _cells[i, j] = new Cell(i, j);
            }
        }

        #region Interface
        public void Draw()
        {
            Console.Clear();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                    Console.Write("|" + Cells[j, i].Sign);

                Console.WriteLine("|");
            }

            Console.WriteLine();
        }

        public bool IsCellFree(int row, int column) => Cells[row, column].IsFree;
        public bool IsSameCellSign(int row, int column, CellSign cellSign) => Cells[row, column].Sign == cellSign;
        #endregion
    }
}
