using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
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

        public Minigame CreateBy(MinigameModes mode)
        {
            switch (mode)
            {
                case MinigameModes.TowerDefence:
                    return new TowerDefenceMode(_container.Resolve<EntitiesFactory>());

                default:
                    throw new ArgumentException($"{mode} mode is not determined");
            }
        }
    }
}
