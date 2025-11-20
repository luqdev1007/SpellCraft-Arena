using Assets._Project.Develop.Runtime.UI.Core;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class ChatPresenter : IPresenter
    {
        private readonly ChatPopupView _chatView;
        private readonly ChatService _chatService;

        public ChatPresenter(ChatPopupView chatView, ChatService chatService)
        {
            _chatView = chatView;
            _chatService = chatService;
        }

        public void Initialize()
        {
            _chatView.ChatButtonClicked += OnChatButtonClicked;
            _chatView.SendMessageButtonClicked += OnSendMessageButtonClicked;
        }

        public void Dispose()
        {
            _chatView.ChatButtonClicked -= OnChatButtonClicked;
            _chatView.SendMessageButtonClicked -= OnSendMessageButtonClicked;
        }

        private void OnSendMessageButtonClicked(string message)
        {
            _chatService.SendMessage(_chatView, message, "red", "LuQmu5");
        }

        private void OnChatButtonClicked()
        {
            if (_chatView.IsClosed)
                _chatView.ShowChat();
            else
                _chatView.CloseChat();
        }
    }
}
