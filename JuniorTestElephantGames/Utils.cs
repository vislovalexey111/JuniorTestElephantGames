namespace JuniorTestElephantGames
{
    internal static class Utils
    {
        public static void LogError(string message)
        {
            Console.Error.WriteLine(message);
            Console.ReadLine();
        }

        public static void LogErrorExit(string message)
        {
            LogError(message);
            Environment.Exit(0);
        }
    }
}
