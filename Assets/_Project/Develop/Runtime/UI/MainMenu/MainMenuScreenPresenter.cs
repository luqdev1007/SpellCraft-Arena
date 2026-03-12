using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.MainMenu;
using Assets._Project.Develop.Runtime.UI.Wallet;

public class MainMenuScreenPresenter : IPresenter
{
    private readonly MainMenuScreenView _view;
    private readonly MainMenuPopupService _popupService;
    private readonly WalletPresenter _walletPresenter;

    public MainMenuScreenPresenter(
        MainMenuScreenView view,
        MainMenuPopupService popupService,
        WalletPresenter walletPresenter)
    {
        _view = view;
        _popupService = popupService;
        _walletPresenter = walletPresenter;
    }

    public void Initialize()
    {
        _walletPresenter.Initialize();
        _view.StartGameButtonClicked += OnStartGameButtonClicked;
    }

    public void Dispose()
    {
        _walletPresenter.Dispose();
        _view.StartGameButtonClicked -= OnStartGameButtonClicked;
    }

    private void OnStartGameButtonClicked()
    {
        _popupService.OpenLevelsMenuPopup();
    }
}