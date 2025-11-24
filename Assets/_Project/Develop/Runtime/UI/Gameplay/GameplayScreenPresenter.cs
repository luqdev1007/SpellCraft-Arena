using Assets._Project.Develop.Runtime.UI.Core;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenPresenter : IPresenter
    {
        private readonly GameplayScreenView _view;
        private readonly GameplayPopupService _popupService;

        public GameplayScreenPresenter(
            GameplayScreenView view, 
            GameplayPopupService popupService
            )
        {
            _view = view;
            _popupService = popupService;
        }

        public void Initialize()
        {
            _view.OpenChatButtonClicked += OnOpenChatButtonClicked;
        }

        public void Dispose()
        {
            _view.OpenChatButtonClicked -= OnOpenChatButtonClicked;
        }

        private void OnOpenChatButtonClicked()
        {
            _popupService.OpenChatPopup();
        }
    }
}
