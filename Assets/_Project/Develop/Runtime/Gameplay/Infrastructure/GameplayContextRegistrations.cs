using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilites.AssetsManagment;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayContextRegistrations
    {
        private static GameplayInputArgs _inputArgs;
        private static ChatView _chatView;

        public static void Process(DIContainer container, GameplayInputArgs inputArgs)
        {
            Debug.Log("Process registrations on gameplay scene");

            _inputArgs = inputArgs;

            container.RegisterAsSingle(CreateGameplayUIRoot).NonLazy();

            // ?
            _chatView = container.Resolve<ViewsFactory>()
                                 .Create<ChatView>(ViewIDs.ChatView, container.Resolve<GameplayUIRoot>().HUDLayer);

            container.RegisterAsSingle(CreateTypeModeGeneratorService);
            container.RegisterAsSingle(CreateTypeModeHandler);
            container.RegisterAsSingle(CreateGameResultService);


            container.RegisterAsSingle(CreateGameplayPresentersFactory);

            container.RegisterAsSingle(CreateChatPresenter).NonLazy();
            container.RegisterAsSingle(CreateEndOfBattlePresenter).NonLazy();
        }

        private static ChatPresenter CreateChatPresenter(DIContainer container)
        {
            GameplayUIRoot uiRoot = container.Resolve<GameplayUIRoot>();

            ChatPresenter presenter = container.Resolve<GameplayPresentersFactory>().CreateChatView(_chatView);

            return presenter;
        }

        private static EndOfBattlePresenter CreateEndOfBattlePresenter(DIContainer container)
        {
            GameplayUIRoot uiRoot = container.Resolve<GameplayUIRoot>();

            EndOfBattleView view = container
                .Resolve<ViewsFactory>()
                .Create<EndOfBattleView>(ViewIDs.EndOfBattleView, uiRoot.HUDLayer);

            EndOfBattlePresenter presenter = container.Resolve<GameplayPresentersFactory>().CreateEndOfBattleView(view);

            return presenter;
        }

        private static GameplayPresentersFactory CreateGameplayPresentersFactory(DIContainer container)
        {
            return new GameplayPresentersFactory(container);
        }

        private static GameplayUIRoot CreateGameplayUIRoot(DIContainer container)
        {
            ResourcesAssetsLoader resourcesAssetsLoader = container.Resolve<ResourcesAssetsLoader>();

            GameplayUIRoot gameplayUIRoot = resourcesAssetsLoader
                .Load<GameplayUIRoot>("UI/Gameplay/GameplayUIRoot");

            return Object.Instantiate(gameplayUIRoot);
        }

        private static GameResultService CreateGameResultService(DIContainer container)
        {
            return new GameResultService(
                container.Resolve<ICoroutinesPerformer>(),
                container.Resolve<GameStatsService>(),
                container.Resolve<PlayerDataProvider>(),
                container.Resolve<WalletService>(),
                container.Resolve<ConfigsProviderService>());
        }

        private static TypeModeCombinationGeneratorService CreateTypeModeGeneratorService(DIContainer container)
        {
            return new TypeModeCombinationGeneratorService();
        }

        private static TypeModeHandler CreateTypeModeHandler(DIContainer container)
        {
            return new TypeModeHandler(_inputArgs,
                container.Resolve<TypeModeCombinationGeneratorService>(),
                container.Resolve<GameResultService>(),
                container.Resolve<ICoroutinesPerformer>(),
                container.Resolve<ChatService>(),
                _chatView
                );
        }
    }
}
