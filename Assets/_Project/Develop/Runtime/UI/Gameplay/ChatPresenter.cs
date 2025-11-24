using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class ChatPresenter : PopupPresenterBase
    {
        private readonly ChatPopupView _chatView;
        private readonly ChatService _chatService;

        protected override PopupViewBase PopupView => _chatView;

        public ChatPresenter(
            ChatPopupView chatView, 
            ChatService chatService, 
            ICoroutinesPerformer coroutinesPerformer) : base(coroutinesPerformer)
        {
            _chatView = chatView;
            _chatService = chatService;
        }

        public override void Initialize()
        {
            base.Initialize();

            _chatView.SendMessageButtonClicked += OnSendMessageButtonClicked;
            _chatService.MessageAddedInHistory += OnMessageAddedInHistory;

            foreach (string message in _chatService.History)
                AddMessageToChatView(message);
        }

        public override void Dispose()
        {
            base.Dispose();

            _chatView.SendMessageButtonClicked -= OnSendMessageButtonClicked;
            _chatService.MessageAddedInHistory -= OnMessageAddedInHistory;
        }

        private void OnSendMessageButtonClicked(string message)
        {
            _chatService.AddMessage(message, "LuQmu5");
        }

        private void OnMessageAddedInHistory(string message)
        {
            AddMessageToChatView(message);
        }

        private void AddMessageToChatView(string message)
        {
            string userName = message.PartBefore(':');
            string content = message.PartAfter(':');

            _chatView.AddText("red", userName, content);
        }
    }
}
