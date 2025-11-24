using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayPopupService : PopupService
    {
        private readonly GameplayUIRoot _uiRoot;
        private readonly GameplayPresentersFactory _gameplayPresentersFactory;

        public GameplayPopupService(
            ViewsFactory viewsFactory,
            ProjectPresentersFactory projectPresentersFactory,
            GameplayUIRoot uiRoot,
            GameplayPresentersFactory gameplayPresentersFactory)
            : base(viewsFactory, projectPresentersFactory)
        {
            _uiRoot = uiRoot;
            _gameplayPresentersFactory = gameplayPresentersFactory;
        }

        protected override Transform PopupLayer => _uiRoot.PopupsLayer;

        public EndOfBattlePresenter OpenEndOfBattlePopup()
        {
            EndOfBattleView view = ViewsFactory.Create<EndOfBattleView>(ViewIDs.EndOfBattleView, PopupLayer);

            EndOfBattlePresenter popup = _gameplayPresentersFactory.CreateEndOfBattleView(view);

            OnPopupCreated(popup, view);

            return popup;
        }
    }
}
