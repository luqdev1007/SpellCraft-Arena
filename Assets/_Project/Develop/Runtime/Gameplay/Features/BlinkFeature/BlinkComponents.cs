using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.BlinkFeature
{
    public class BlinkRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class BlinkExecutedEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class BlinkCooldownInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class BlinkCooldownCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class InBlinkCooldown : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }
}
