using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{    
    public class EnergyMaxValue : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyCurrentValue : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TimeToRestoreEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class AmountOfRestoreEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class CanRestoreEnergy : IEntityComponent
    {
        public ICompositeCondition Value;
    }
}
