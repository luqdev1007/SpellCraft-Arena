using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature
{
    public class CanTeleport : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public class AmountOfEnergyForTeleport : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportInitialCooldown : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportCurrentCooldown : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class IsTeleportCooldownReady : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class TeleportRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }
}
