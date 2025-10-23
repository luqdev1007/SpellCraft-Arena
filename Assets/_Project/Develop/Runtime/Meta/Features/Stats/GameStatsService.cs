using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Configs.Meta.Stats;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.DataManagment;
using System;

namespace Assets._Project.Develop.Runtime.Meta.Features.Stats
{
    public class GameStatsService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly WalletService _walletService;
        private readonly ConfigsProviderService _configsProviderService;

        private int _currentWins;
        private int _currentLoses;

        public GameStatsService(PlayerDataProvider playerDataProvider, WalletService walletService, ConfigsProviderService configsProviderService)
        {
            _playerDataProvider = playerDataProvider;
            _walletService = walletService;
            _configsProviderService = configsProviderService;
            _playerDataProvider.RegisterWriter(this);
            _playerDataProvider.RegisterReader(this);
        }

        public void RegisterVictory()
        {
            _currentWins++;
            _walletService.Add(CurrencyTypes.Gold, _configsProviderService.GetConfig<GameRewardsConfig>().RewardForWin);
        }

        public void RegisterDefeat()
        {
            _currentLoses++;

            if (_walletService.IsEnough(CurrencyTypes.Gold, _configsProviderService.GetConfig<GameRewardsConfig>().PenaltyForLose))
                _walletService.Spend(CurrencyTypes.Gold, _configsProviderService.GetConfig<GameRewardsConfig>().PenaltyForLose);
            else
                _walletService.Spend(CurrencyTypes.Gold, _walletService.GetCurrency(CurrencyTypes.Gold).Value);
        }

        public int GetWins() => _currentWins;
        public int GetLosses() => _currentLoses;

        public void ReadFrom(PlayerData data)
        {
            _currentWins = data.Wins;
            _currentLoses = data.Losses;
        }

        public void WriteTo(PlayerData data)
        {
            data.Wins = _currentWins;
            data.Losses = _currentLoses;
        }

        public void Reset()
        {
            _currentWins = 0;
            _currentLoses = 0;
        }
    }
}