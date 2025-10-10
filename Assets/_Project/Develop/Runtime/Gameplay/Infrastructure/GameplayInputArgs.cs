namespace Assets._Project.Develop.Runtime.Utilites.SceneManagement
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public int LevelNumber { get; }

        public GameplayInputArgs(int levelNumber)
        {
            LevelNumber = levelNumber;
        }
    }
}
