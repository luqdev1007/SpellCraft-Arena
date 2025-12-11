using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class RestoreEnergySystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _timeToRestoreEnergy;
        private ReactiveVariable<float> _amountOfRestoreEnergy;
        private ICompositeCondition _canRestoreEnergy;

        private float _currentTimer = 0;

        public void OnInit(Entity entity)
        {
            _currentEnergy = entity.EnergyCurrentValue;
            _maxEnergy = entity.EnergyMaxValue;
            _timeToRestoreEnergy = entity.TimeToRestoreEnergy;
            _amountOfRestoreEnergy = entity.AmountOfRestoreEnergy;
            _canRestoreEnergy = entity.CanRestoreEnergy;
        }

        public void OnUpdate(float deltaTime)
        {
            Debug.Log("Restore energy system");

            if (_canRestoreEnergy.Evaluate() == false)
                return;

            if (_currentEnergy.Value == _maxEnergy.Value)
                return;

            if (_currentTimer >= _timeToRestoreEnergy.Value)
            {
                _currentTimer = 0;
                RestoreEnergy();
            }

            _currentTimer += deltaTime;
        }

        private void RestoreEnergy()
        {
            _currentEnergy.Value = Mathf.Min(_currentEnergy.Value + _amountOfRestoreEnergy.Value, _maxEnergy.Value);
            Debug.Log("Energy restored: " + _currentEnergy.Value);
        }
    }
}
