using Assets._Project.Develop.Runtime.UI.Core;

public class ChatPresenter : IPresenter
{
    private ChatView _chatView;

    public ChatPresenter(ChatView chatView)
    {
        _chatView = chatView;
    }

    public void Initialize()
    {

    }

    public void Dispose()
    {

    }
}