namespace JuniorTestElephantGames
{
    public interface IPlayer
    {
        public CellSign Sign { get; }
        public string Name { get; }

        public Cell Move(Grid grid);
    }
}