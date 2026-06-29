using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
    public class SelfReleaseSystem : IInitializableSystem, IUpdatableSystem
    {
        private readonly EntitiesLifeContext _entitiesLifeContext;

        private Entity _entity;

        private ICompositeCondition _mustSelfRelease;

        public SelfReleaseSystem(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _mustSelfRelease = entity.MustSelfRelease;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_mustSelfRelease.Evaluate())
            {
                Debug.Log(_entity.Transform.gameObject.name + "self released");
                _entitiesLifeContext.Release(_entity);
            }
        }
    }
}
