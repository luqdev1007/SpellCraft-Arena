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

        public ChatPresenter CreateChatView(ChatPopupView view)
        {
            return new ChatPresenter(
                view,
                _container.Resolve<ChatService>(),
                _container.Resolve<ICoroutinesPerformer>());
        }

        public EndOfBattlePresenter CreateEndOfBattleView(EndOfBattleView view)
        {
            return new EndOfBattlePresenter(
                view,
                _container.Resolve<WalletService>(),
                _container.Resolve<GameStatsService>(),
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<GameResultService>(),
                _container.Resolve<TypeModeHandler>()
                );
        }
    }
}