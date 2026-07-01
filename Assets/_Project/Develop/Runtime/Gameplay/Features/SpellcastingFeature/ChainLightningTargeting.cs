using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public static class ChainLightningTargeting
    {
        // Virtual hitscan: no Physics query in this project resolves Collider -> Entity,
        // so candidates are found the same way Ice Spikes/AI targeting do — by iterating
        // EntitiesLifeContext.Entities and testing geometry against the aim ray.
        public static bool TryVirtualSphereCast(Entity caster, EntitiesLifeContext entitiesLifeContext, Vector3 origin, Vector3 direction, float radius, float maxDistance, out Entity hitTarget)
        {
            hitTarget = null;
            float closestDistanceAlongRay = float.MaxValue;

            IReadOnlyList<Entity> entities = entitiesLifeContext.Entities;

            for (int i = 0; i < entities.Count; i++)
            {
                Entity candidate = entities[i];

                if (IsValidTarget(caster, candidate) == false)
                    continue;

                Vector3 toCandidate = candidate.Transform.position - origin;
                float distanceAlongRay = Vector3.Dot(toCandidate, direction);

                if (distanceAlongRay < 0f || distanceAlongRay > maxDistance)
                    continue;

                float perpendicularDistance = (toCandidate - direction * distanceAlongRay).magnitude;

                if (perpendicularDistance > radius)
                    continue;

                if (distanceAlongRay < closestDistanceAlongRay)
                {
                    closestDistanceAlongRay = distanceAlongRay;
                    hitTarget = candidate;
                }
            }

            return hitTarget != null;
        }

        public static bool TryFindNextTarget(Entity caster, EntitiesLifeContext entitiesLifeContext, Vector3 origin, float range, ICollection<Entity> excluded, out Entity nextTarget)
        {
            nextTarget = null;
            float closestSqrDistance = float.MaxValue;
            float sqrRange = range * range;

            IReadOnlyList<Entity> entities = entitiesLifeContext.Entities;

            for (int i = 0; i < entities.Count; i++)
            {
                Entity candidate = entities[i];

                if (IsValidTarget(caster, candidate) == false)
                    continue;

                if (excluded.Contains(candidate))
                    continue;

                float sqrDistance = (candidate.Transform.position - origin).sqrMagnitude;

                if (sqrDistance > sqrRange)
                    continue;

                if (sqrDistance < closestSqrDistance)
                {
                    closestSqrDistance = sqrDistance;
                    nextTarget = candidate;
                }
            }

            return nextTarget != null;
        }

        private static bool IsValidTarget(Entity caster, Entity candidate)
        {
            if (candidate == caster)
                return false;

            if (candidate.HasComponent<TakeDamageRequest>() == false)
                return false;

            if (candidate.TryGetIsDead(out ReactiveVariable<bool> isDead) && isDead.Value)
                return false;

            if (candidate.TryGetCanApplyDamage(out ICompositeCondition canApplyDamage) && canApplyDamage.Evaluate() == false)
                return false;

            if (EntitiesHelper.IsSameTeam(caster, candidate))
                return false;

            return true;
        }
    }
}
