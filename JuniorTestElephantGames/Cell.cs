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

        public CellSign Sign;

        public Cell(int x = 0, int y = 0, CellSign sign = CellSign._)
        {
            X = x;
            Y = y;
            Sign = sign;
        }

        public int X
        {
            get { return _x; }
            private set { _x = (value < 0) ? 0 : value; }
        }

        public int Y
        {
            get { return _y; }
            private set { _y = (value < 0) ? 0 : value; }
        }
    }
}
