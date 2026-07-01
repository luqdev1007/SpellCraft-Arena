using Assets._Project.Develop.Runtime.Configs.Gameplay.Spells;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class ChainLightningActive : IEntityComponent
    {
        public ReactiveVariable<bool> Value;
    }

    public class ChainLightningJumpTimer : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class ChainLightningState : IEntityComponent
    {
        public List<Entity> HitTargets;
        public Entity CurrentTarget;
        public int JumpsRemaining;
        public float CurrentDamage;
        public SpellConfig Config;
    }
}
