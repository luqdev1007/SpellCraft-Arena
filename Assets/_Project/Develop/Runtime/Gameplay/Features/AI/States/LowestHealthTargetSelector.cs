using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using System.Collections.Generic;
using System.Linq;

public class LowestHealthTargetSelector : ITargetSelector
{
    private Entity _source;

    public LowestHealthTargetSelector(Entity entity)
    {
        _source = entity;
    }

    public Entity SelectTargetFrom(IEnumerable<Entity> targets)
    {
        IEnumerable<Entity> selectedTargets = targets.Where(target =>
        {
            bool result = target.HasComponent<TakeDamageRequest>();

            if (target.TryGetCanApplyDamage(out ICompositeCondition canApplyDamage))
            {
                result = result && canApplyDamage.Evaluate();
            }

            result = result && (target != _source);

            return result;
        });

        if (selectedTargets.Any() == false)
            return null;

        Entity lowestHealthTarget = selectedTargets.First();

        foreach (Entity target in selectedTargets)
        {
            if (target.CurrentHealth.Value < lowestHealthTarget.CurrentHealth.Value)
            {
                lowestHealthTarget = target;
            }
        }

        return lowestHealthTarget;
    }
}
