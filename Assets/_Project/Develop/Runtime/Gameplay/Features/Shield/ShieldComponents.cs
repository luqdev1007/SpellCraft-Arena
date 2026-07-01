using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Shield
{
    public class ShieldToggleRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class IsShieldActive : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class ShieldAbsorbPercent : IEntityComponent
    {
        public float Value;
    }

    public class ShieldManaPerUnit : IEntityComponent
    {
        public float Value;
    }
}
