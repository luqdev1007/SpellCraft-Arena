using Assets._Project.Develop.Runtime.Configs.Gameplay.Spells;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class ActiveSpellConfig : IEntityComponent
    {
        public SpellConfig Value;
    }

    public class IsCasting : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class CastWindupCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class CastRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }
}
