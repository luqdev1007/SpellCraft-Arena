using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class ChatPresenter : PopupPresenterBase
    {
        private readonly ChatPopupView _chatView;
        private readonly ChatService _chatService;

        protected override PopupViewBase PopupView => _chatView;

        public ChatPresenter(ChatPopupView chatView, ChatService chatService, ICoroutinesPerformer coroutinesPerformer) : base(coroutinesPerformer)
        {
            _chatView = chatView;
            _chatService = chatService;
        }

        public override void Initialize()
        {
            base.Initialize();

            _chatView.ChatButtonClicked += OnChatButtonClicked;
            _chatView.SendMessageButtonClicked += OnSendMessageButtonClicked;
        }

        public override void Dispose()
        {
            base.Dispose();

            _chatView.ChatButtonClicked -= OnChatButtonClicked;
            _chatView.SendMessageButtonClicked -= OnSendMessageButtonClicked;
        }

        private void OnSendMessageButtonClicked(string message)
        {
            _chatService.SendMessage(_chatView, message, "red", "LuQmu5");
        }

        private void OnChatButtonClicked()
        {
            _chatView.Show();
        }
    }
}
