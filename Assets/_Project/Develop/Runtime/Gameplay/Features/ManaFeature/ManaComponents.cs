using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ManaFeature
{
    public class CurrentMana : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class MaxMana : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ManaRegenRate : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }
}
