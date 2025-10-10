using Assets._Project.Develop.Runtime.Gameplay.TypeMode;

namespace Assets._Project.Develop.Runtime.Utilites.SceneManagement
{
    public class GameplayInputArgs : IInputSceneArgs
    {
        public GameplayInputArgs(TypeSymbolsGameMode typeSymbolsGameMode)
        {
            TypeSymbolsGameMode = typeSymbolsGameMode;
        }

        public TypeSymbolsGameMode TypeSymbolsGameMode { get; }
    }
}
