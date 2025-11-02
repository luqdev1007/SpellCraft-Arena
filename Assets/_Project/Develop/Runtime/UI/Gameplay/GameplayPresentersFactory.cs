using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayPresentersFactory
    {
        private readonly DIContainer _container;

        public GameplayPresentersFactory(DIContainer container)
        {
            _container = container;
        }

        public ChatPresenter CreateChatView(ChatView view)
        {
            return new ChatPresenter(view);
        }

        public EndOfBattlePresenter CreateEndOfBattleView(EndOfBattleView view)
        {
            return new EndOfBattlePresenter(
                view,
                _container.Resolve<WalletService>(),
                _container.Resolve<GameStatsService>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>()
                );
        }
    }
}