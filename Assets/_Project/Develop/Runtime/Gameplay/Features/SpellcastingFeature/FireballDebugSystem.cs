using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class FireballDebugSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private Entity _entity;
        private LayerMask _deathMask;
        private readonly List<IDisposable> _subs = new();

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _deathMask = entity.DeathMask;

            Transform t = entity.Transform;
            int layer = t.gameObject.layer;
            Debug.Log($"[FIREBALL DEBUG] SPAWNED pos={t.position} layer={LayerMask.LayerToName(layer)}({layer}) DeathMask={_deathMask.value}");

            if (entity.TryGetTeam(out var teamVar))
                Debug.Log($"[FIREBALL DEBUG] Team={teamVar.Value}");

            if (entity.TryGetOwner(out var ownerVar) && ownerVar.Value?.Transform != null)
                Debug.Log($"[FIREBALL DEBUG] Owner='{ownerVar.Value.Transform.name}' ownerPos={ownerVar.Value.Transform.position}");

            _subs.Add(entity.IsTouchDeathMask.Subscribe((prev, cur) =>
            {
                if (cur && !prev)
                    Debug.Log("[FIREBALL DEBUG] IsTouchDeathMask → TRUE (death mask hit will kill fireball)");
            }));

            _subs.Add(entity.IsTouchAnotherTeam.Subscribe((prev, cur) =>
            {
                if (cur && !prev)
                    Debug.Log("[FIREBALL DEBUG] IsTouchAnotherTeam → TRUE (enemy contact will kill fireball)");
                if (!cur && prev)
                    Debug.Log("[FIREBALL DEBUG] IsTouchAnotherTeam → FALSE (left enemy contact)");
            }));

            _subs.Add(entity.IsDead.Subscribe((prev, cur) =>
            {
                if (cur)
                    Debug.Log($"[FIREBALL DEBUG] FIREBALL DIED — TouchDeathMask={entity.IsTouchDeathMask.Value}, TouchAnotherTeam={entity.IsTouchAnotherTeam.Value}");
            }));
        }

        public void OnUpdate(float deltaTime)
        {
            LogColliderContacts();
            LogEntityContacts();
        }

        public void OnDispose()
        {
            foreach (IDisposable sub in _subs)
                sub.Dispose();

            _subs.Clear();
        }

        private void LogColliderContacts()
        {
            Buffer<Collider> contacts = _entity.ContactCollidersBuffer;
            if (contacts.Count == 0)
                return;

            for (int i = 0; i < contacts.Count; i++)
            {
                Collider c = contacts.Items[i];
                if (c == null) continue;

                int layer = c.gameObject.layer;
                bool isDeathMask = ((1 << layer) & _deathMask.value) != 0;
                Debug.Log($"[FIREBALL DEBUG] Collider[{i}] '{c.gameObject.name}' layer={LayerMask.LayerToName(layer)}({layer}) tag={c.tag} matchesDeathMask={isDeathMask}");
            }
        }

        private void LogEntityContacts()
        {
            Buffer<Entity> entities = _entity.ContactEntitiesBuffer;
            if (entities.Count == 0)
                return;

            for (int i = 0; i < entities.Count; i++)
            {
                Entity contact = entities.Items[i];
                if (contact == null) continue;

                bool hasDamageReq = contact.TryGetTakeDamageRequest(out _);
                bool sameTeam = EntitiesHelper.IsSameTeam(_entity, contact);
                string name = contact.Transform != null ? contact.Transform.name : "unknown";
                Debug.Log($"[FIREBALL DEBUG] Entity[{i}] '{name}' hasDamageRequest={hasDamageReq} sameTeam={sameTeam}");
            }
        }
    }
}
