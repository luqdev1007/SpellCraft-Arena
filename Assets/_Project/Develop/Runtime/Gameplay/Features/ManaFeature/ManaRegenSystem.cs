using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ManaFeature
{
    public class ManaRegenSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _currentMana;
        private ReactiveVariable<float> _maxMana;
        private ReactiveVariable<float> _regenRate;

        public void OnInit(Entity entity)
        {
            _currentMana = entity.CurrentMana;
            _maxMana = entity.MaxMana;
            _regenRate = entity.ManaRegenRate;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_currentMana.Value >= _maxMana.Value)
                return;

            _currentMana.Value = System.Math.Min(
                _currentMana.Value + _regenRate.Value * deltaTime,
                _maxMana.Value);
        }
    }
}
