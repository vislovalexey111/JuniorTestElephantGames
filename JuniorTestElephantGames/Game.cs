namespace JuniorTestElephantGames
{
    public sealed class Game
    {
        private IPlayer[] _players;
        private Grid _grid;
        private bool _isGameWon;
        private int _numberOfPlayers;
        private int _maxPlayers;

        private readonly string _menu;
        private readonly string _firstPlayerSelectMenu;
        private readonly List<string> _restartAnswers;

        #region Properties
        public int NumberOfPlayers
        {
            get => _numberOfPlayers;
            private set
            {
                if (value < 2)
                    _numberOfPlayers = 2;
                else
                    _numberOfPlayers = (value > _maxPlayers) ? _maxPlayers : value;
            }
        }
        #endregion

        public Game()
        {
            CheckMaxPlayers();
            NumberOfPlayers = _maxPlayers;
            _players = new IPlayer[NumberOfPlayers];
            _grid = new Grid();

            _menu = "\n" +
                "1) Player VS CPU\n" +
                "2) Players only\n" +
                "3) CPU only (Demo)";

            _firstPlayerSelectMenu = "\n" +
                "1) Player\n" +
                "2) CPU";

            _restartAnswers = new()
            {
                "yes",
                "y",
                "ok"
            };
        }

        #region Main game functions
        public void Launch()
        {
            string? input;

            do {
                Start();

                // Updating
                for (int i = 0; i < NumberOfPlayers && !_isGameWon; i++)
                    Update(i);

                if (!_isGameWon)
                {
                    _grid.Draw();
                    Console.WriteLine("Nobody won!");
                }

                // Asking for restart
                Console.Write("\nDo you want to restart the game? (Yes / No - default)\n> ");
                input = Console.ReadLine();

            } while (!string.IsNullOrWhiteSpace(input) && _restartAnswers.Contains(input.ToLower()));
        }

        private void Start()
        {
            _isGameWon = false;

            // Setting up grid size
            int gridSize = SetupInput("Enter grid size! (default is min = 3, max = 5)", 3);
            _grid = new Grid(gridSize);
            CheckMaxPlayers();

            // For number of players bigger than 2
            /* NumberOfPlayers = SetupInput("Enter number of players! (min = 2, default is max = " + _maxPlayers, _maxPlayers);
            _players = new IPlayer[NumberOfPlayers];*/

            // Setting up game mode
            int mode = SetupInput("Enter game mode! (default is Demo)" + _menu, 3);

            // Setting up players
            if (mode == 1)
            {
                int firstPlayer = SetupInput("Who will go first? (default is CPU)" + _firstPlayerSelectMenu, 2);

                if (firstPlayer == 1)
                {
                    _players[0] = new Player(0);
                    _players[1] = new Cpu(1);
                }
                else
                {
                    _players[0] = new Cpu(0);
                    _players[1] = new Player(1);
                }

                // For number of players bigger than 2
                /*for (int i = 2; i < NumberOfPlayers; i++)
                    _players[i] = new Cpu(i);*/
            }
            else if (mode == 2)
            {
                for (int i = 0; i < NumberOfPlayers; i++)
                    _players[i] = new Player(i);
            }
            else
            {
                for (int i = 0; i < NumberOfPlayers; i++)
                    _players[i] = new Cpu(i);
            }
        }

        private void Update(int moveNumber)
        {
            int iteration = moveNumber / NumberOfPlayers + 1;
            int index = moveNumber % NumberOfPlayers;

            _grid.Draw();
            Console.WriteLine(_players[index].Name);

            Cell cell = _players[index].Move(_grid);
            _grid.Cells[cell.X, cell.Y].Sign = cell.Sign;

            // We do not want to calculate winner before we have enough moves
            if (iteration < _grid.Size) return;

            if (IsWinner(_grid, cell))
            {
                _grid.Draw();
                _isGameWon = true;
                Console.WriteLine("Congratulations!\n\n" + _players[index].Name + " wins!");
            }
        }
        #endregion

        #region Helpers
        private int SetupInput(string message, int defaultValue)
        {
            Console.Clear();
            Console.Write(message + "\n> ");

            // Checking if the input is valid number.
            if (int.TryParse(Console.ReadLine(), out int value))
                return value;

            return defaultValue;
        }

        private void CheckMaxPlayers()
        {
            _maxPlayers = Enum.GetNames(typeof(CellSign)).Length - 1;

            if (_maxPlayers < 2)
                _maxPlayers = 2;
            else if ((_grid != null) && (_maxPlayers > _grid.Size))
                _maxPlayers = _grid.Size;
        }
        #endregion

        #region Interface
        public static bool IsWinner(Grid grid, Cell cell)
        {
            int gridSize = grid.Size - 1;
            int cellX = cell.X;
            int cellY = cell.Y;
            CellSign cellSign = cell.Sign;

            // Checker for row and column of the cell and both diagonals on the grid
            bool[] winnerChecker = { true, true, true, true };

            for (int i = 0; i < grid.Size; i++)
            {
                // Checking row
                if (!grid.IsSameCellSign(i, cellY, cellSign))
                    winnerChecker[0] = false;

                // Checking column
                if (!grid.IsSameCellSign(cellX, i, cellSign))
                    winnerChecker[1] = false;

                // Checking first diagonal
                if (!grid.IsSameCellSign(i, i, cellSign))
                    winnerChecker[2] = false;

                // Checking second diagonal
                if (!grid.IsSameCellSign(gridSize - i, i, cellSign))
                    winnerChecker[3] = false;
            }

            return winnerChecker.Any(statement => statement == true);
        }
        #endregion
    }
}
