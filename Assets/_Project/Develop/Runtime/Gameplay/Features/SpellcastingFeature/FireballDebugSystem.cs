using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class FireballDebugSystem : IInitializableSystem, IDisposableSystem
    {
        private Entity _entity;
        private readonly List<IDisposable> _subs = new();

        public void OnInit(Entity entity)
        {
            _entity = entity;

            Transform t = entity.Transform;
            Debug.Log($"[FIREBALL DEBUG] SPAWNED pos={t.position} layer={LayerMask.LayerToName(t.gameObject.layer)}");

            _subs.Add(entity.IsTouchDeathMask.Subscribe((prev, cur) =>
            {
                if (cur && !prev)
                    Debug.Log("[FIREBALL DEBUG] IsTouchDeathMask → TRUE");
            }));

            _subs.Add(entity.IsTouchAnotherTeam.Subscribe((prev, cur) =>
            {
                if (cur && !prev)
                    Debug.Log("[FIREBALL DEBUG] IsTouchAnotherTeam → TRUE (hit enemy)");
            }));

            _subs.Add(entity.IsDead.Subscribe((prev, cur) =>
            {
                if (cur)
                    Debug.Log($"[FIREBALL DEBUG] DIED — DeathMask={entity.IsTouchDeathMask.Value} AnotherTeam={entity.IsTouchAnotherTeam.Value}");
            }));
        }

        public void OnDispose()
        {
            foreach (IDisposable sub in _subs)
                sub.Dispose();
            _subs.Clear();
        }
    }
}
