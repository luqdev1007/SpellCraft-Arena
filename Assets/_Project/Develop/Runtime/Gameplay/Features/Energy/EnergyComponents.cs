using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
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
}
