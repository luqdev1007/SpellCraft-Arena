using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Timer;
using System;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigamesFactory
    {
        private readonly DIContainer _container;

        public MinigamesFactory(DIContainer container)
        {
            _container = container;
        }

        public Minigame CreateFor(MinigameModes mode)
        {
            switch (mode)
            {
                case MinigameModes.TowerDefence:
                    return new TowerDefenceMode(
                        _container.Resolve<MinigamesCreaturesFactory>(),
                        _container.Resolve<TimerServiceFactory>());

                default:
                    throw new ArgumentException($"{mode} mode is not determined");
            }
        }
    }
}
