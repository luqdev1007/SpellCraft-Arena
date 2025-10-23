using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.DataManagment;

namespace Assets._Project.Develop.Runtime.Meta.Features.Stats
{
    public class GameStatsService : IDataReader<PlayerData>, IDataWriter<PlayerData>
    {
        private readonly PlayerDataProvider _playerDataProvider;

        public int Wins { get; private set; }
        public int Losses { get; private set; }

        public GameStatsService(PlayerDataProvider playerDataProvider)
        {
            _playerDataProvider = playerDataProvider;

            _playerDataProvider.RegisterWriter(this);
            _playerDataProvider.RegisterReader(this);
        }

        public void RegisterVictory()
        {
            Wins++;
        }

        public void RegisterDefeat()
        {
            Losses++;
        }

        public void ReadFrom(PlayerData data)
        {
            Wins = data.Wins;
            Losses = data.Losses;
        }

        public void WriteTo(PlayerData data)
        {
            data.Wins = Wins;
            data.Losses = Losses;
        }

        public void Reset()
        {
            Wins = 0;
            Losses = 0;
        }
    }
}