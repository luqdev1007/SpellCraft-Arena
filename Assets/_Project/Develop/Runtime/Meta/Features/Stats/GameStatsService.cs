using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Configs.Meta.Stats;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;

namespace Assets._Project.Develop.Runtime.Meta.Features.Stats
{
    public class GameStatsService
    {
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly WalletService _walletService;
        private readonly GameRewardsConfig _rewardsConfig;

        public GameStatsService(PlayerDataProvider playerDataProvider, WalletService walletService, ConfigsProviderService configsProviderService)
        {
            _playerDataProvider = playerDataProvider;
            _walletService = walletService;
            _rewardsConfig = configsProviderService.GetConfig<GameRewardsConfig>();
        }

        public void RegisterVictory()
        {
            _playerDataProvider.CurrentData.Wins++;
            _walletService.Add(CurrencyTypes.Gold, _rewardsConfig.RewardForWin);
        }

        public void RegisterDefeat()
        {
            _playerDataProvider.CurrentData.Losses++;

            if (_walletService.IsEnough(CurrencyTypes.Gold, _rewardsConfig.PenaltyForLose))
                _walletService.Spend(CurrencyTypes.Gold, _rewardsConfig.PenaltyForLose);
            else
                _walletService.Spend(CurrencyTypes.Gold, _walletService.GetCurrency(CurrencyTypes.Gold).Value);
        }

        public int GetWins() => _playerDataProvider.CurrentData.Wins;
        public int GetLosses() => _playerDataProvider.CurrentData.Losses;
    }
}