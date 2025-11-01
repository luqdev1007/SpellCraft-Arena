using Assets._Project.Develop.Infrastructure.DI;

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
}
