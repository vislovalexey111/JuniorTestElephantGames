namespace JuniorTestElephantGames
{
    public enum CellSign
    {
        _ = 0,
        X = 1,
        O = 2
    }

    public sealed class Cell
    {
        private int _x;
        private int _y;
        private CellSign _sign;

        #region Properties
        public bool IsFree => Sign == CellSign._;

        public CellSign Sign
        {
            get => _sign;
            set { _sign = Enum.IsDefined(value) ? value : CellSign._; }
        }

        public int X
        {
            get => _x;
            private set { _x = (value < 0) ? 0 : value; }
        }

        public int Y
        {
            get => _y;
            private set { _y = (value < 0) ? 0 : value; }
        }
        #endregion

        public Cell(int x = 0, int y = 0, CellSign sign = CellSign._)
        {
            X = x;
            Y = y;
            Sign = sign;
        }
    }
}
