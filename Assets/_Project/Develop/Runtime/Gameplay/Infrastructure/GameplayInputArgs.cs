namespace Assets._Project.Develop.Runtime.Utilites.SceneManagement
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public GameplayInputArgs(string allowedSymbols = "", int levelNumber = 0)
        {
            AllowedSymbols = allowedSymbols;
            LevelNumber = levelNumber;
        }

        public string AllowedSymbols { get; }
        public int LevelNumber { get; }
    }
}
