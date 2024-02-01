namespace JuniorTestElephantGames
{
    public sealed class Player : IPlayer
    {
        private readonly CellSign _cellSign;
        private readonly string _name;

        public Player(int number)
        {
            int playerNumber = ((number < 0) ? 0 : number) + 1;

            _cellSign = (CellSign)playerNumber;
            _name = "Player " + playerNumber + " (Player) - " + _cellSign;

            if (!Enum.IsDefined(_cellSign))
                Utils.LogErrorExit("Invalid Player " + number);
        }

        #region IPlayer Interface
        public string Name => _name;
        public CellSign Sign => _cellSign;

        Cell IPlayer.Move(Grid grid)
        {
            PlayerInput("Enter cell coordinates in format (x y) without paranthases (first cell = 1 1)", out int x, out int y);

            while ((x < 0) || (y < 0) || (x >= grid.Size) || (y >= grid.Size) || !grid.IsCellFree(x, y))
                PlayerInput("Wrong, please, enter cell coordinates again!", out x, out y);

            return new(x, y, Sign);
        }
        #endregion

        #region Helpers
        private void PlayerInput(string message, out int x, out int y)
        {
            Console.Write(message + "\n> ");

            string? input = Console.ReadLine();
            string[] result = {""};

            if (!string.IsNullOrWhiteSpace(input))
                result = input.Trim().Split();

            x = -1;
            y = -1;

            if (result.Length == 2 && int.TryParse(result[0], out x) && int.TryParse(result[1], out y))
                --x; --y;
        }
        #endregion
    }
}
