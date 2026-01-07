using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.AssetsManagment;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigamesCreaturesFactory
    {
        private readonly DIContainer _container;

        public MinigamesCreaturesFactory(DIContainer container)
        {
            _container = container;
        }

        public Tower CreateTower(Transform parent)
        {
            Tower prefab = _container.Resolve<ResourcesAssetsLoader>()
                .Load<Tower>("Prefabs/Minigames/Tower");

            Tower instance = Object.Instantiate(prefab, parent);

            return instance;
        }

        public ExplosiveDevil CreateExplosiveDevil()
        {
            ExplosiveDevil prefab = _container.Resolve<ResourcesAssetsLoader>()
                .Load<ExplosiveDevil>("Prefabs/Enemies/ExplosiveDevil/ExplosiveDevil");

            ExplosiveDevil instance = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            return instance;
        }
    }
}
