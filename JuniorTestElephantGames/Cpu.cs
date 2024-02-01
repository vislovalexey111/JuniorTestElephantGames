namespace JuniorTestElephantGames
{
    public sealed class Cpu : IPlayer
    {
        private readonly CellSign _sign;
        private readonly CellSign[] _opponents;
        private readonly string _name;

        public Cpu(int number)
        {
            int playerNumber = ((number < 0) ? 0 : number) + 1;
            _sign = (CellSign)playerNumber;
            _name = "Player " + playerNumber + " (CPU)";
            _opponents = Enum.GetValues<CellSign>().Where(sign => (sign != CellSign._) && (sign != Sign)).ToArray();

            if (!Enum.IsDefined(_sign))
                Utils.LogErrorExit("Invalid Enemy " + number);
        }

        #region IPlayer Interface
        public string Name => _name;
        public CellSign Sign => _sign;

        Cell IPlayer.Move(Grid grid)
        {
            Cell cell = AICellSelector(grid);

            return new(cell.X, cell.Y, Sign);
        }
        #endregion

        #region Helpers
        private Cell AICellSelector(Grid grid)
        {
            int[] bestRow = new int[2] { 0, grid.Size };
            int[] bestColumn = new int[2] { 0, grid.Size };
            int rowCounter;
            int columnCounter;

            // If we can win by moving to the cell - returning this cell, if another player can win by moving to the cell - blocking this cell.
            // By moving through the grid - checking the shortest row and shortest column - this will be our best decision.
            for (int i = 0; i < grid.Size; i++)
            {
                rowCounter = 0;
                columnCounter = 0;

                for (int j = 0; j < grid.Size; j++)
                {
                    if (grid.IsCellFree(j, i))
                    {
                        ++rowCounter;

                        if (CanAnotherPlayerWin(grid, grid.Cells[j, i]) || IsPredictedWinner(grid, new(j, i, Sign)))
                            return grid.Cells[j, i];
                    }

                    if (grid.IsCellFree(i, j))
                        ++columnCounter;
                }

                if (rowCounter > 0 && rowCounter < bestRow[1])
                {
                    bestRow[0] = i;
                    bestRow[1] = rowCounter;
                }

                if (columnCounter > 0 && columnCounter < bestColumn[1])
                {
                    bestColumn[0] = i;
                    bestColumn[1] = columnCounter;
                }
            }

            // If we can't win in this move - searching for the best solution
            int center = grid.Size / 2;

            if (grid.IsCellFree(center, center))
                return grid.Cells[center, center];
            else if (bestRow[1] < bestColumn[1])
                return (from Cell cell in grid.Cells where (cell.Y == bestRow[0]) && cell.IsFree select cell).ToArray()[0];
            else
                return (from Cell cell in grid.Cells where (cell.X == bestColumn[0]) && cell.IsFree select cell).ToArray()[0];
        }

        private bool CanAnotherPlayerWin(Grid grid, Cell cell)
        {
            Cell cellChecker = new(cell.X, cell.Y);

            foreach (CellSign opponent in _opponents)
            {
                cellChecker.Sign = opponent;

                if (IsPredictedWinner(grid, cellChecker))
                    return true;
            }

            return false;
        }

        private bool IsPredictedWinner(Grid grid, Cell cell)
        {
            bool result;

            // Temporarily changing the grid to se, if the cell we interested in will win by assigning correspondend sign
            grid.Cells[cell.X, cell.Y].Sign = cell.Sign;
            result = Game.IsWinner(grid, cell);

            // Changing back and returning result.
            grid.Cells[cell.X, cell.Y].Sign = CellSign._;
            return result;
        }
        #endregion
    }
}