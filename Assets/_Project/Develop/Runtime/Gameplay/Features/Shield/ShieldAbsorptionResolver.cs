using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Shield
{
    public static class ShieldAbsorptionResolver
    {
        // Model A: the shield only ever absorbs up to AbsorbPercent of a hit — the rest always
        // passes through, even with unlimited mana. See CLAUDE.md task description for the spec.
        public static float Resolve(Entity target, float incoming)
        {
            if (target.TryGetIsShieldActive(out ReactiveVariable<bool> isShieldActive) == false)
                return incoming;

            if (isShieldActive.Value == false)
                return incoming;

            if (target.TryGetShieldAbsorbPercent(out float absorbPercent) == false)
                return incoming;

            if (target.TryGetShieldManaPerUnit(out float manaPerUnit) == false)
                return incoming;

            if (target.TryGetCurrentMana(out ReactiveVariable<float> currentMana) == false)
                return incoming;

            float desiredAbsorb = incoming * absorbPercent / 100f;
            float manaNeeded = desiredAbsorb * manaPerUnit;

            float absorbed;
            float manaSpent;

            if (currentMana.Value >= manaNeeded)
            {
                absorbed = desiredAbsorb;
                manaSpent = manaNeeded;
            }
            else
            {
                absorbed = currentMana.Value / manaPerUnit;
                manaSpent = currentMana.Value;
            }

            absorbed = Mathf.Min(absorbed, incoming);

            currentMana.Value = Mathf.Max(0f, currentMana.Value - manaSpent);

            if (manaSpent > 0f && currentMana.Value <= 0f)
                isShieldActive.Value = false;

            return incoming - absorbed;
        }
    }
}
