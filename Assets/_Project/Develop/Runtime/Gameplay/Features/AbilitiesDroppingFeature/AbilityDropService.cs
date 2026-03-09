using Assets._Project.Develop.Runtime.Configs.Gameplay.Abilities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilitiesDroppingFeature
{
    public class AbilityDropService
    {
        private readonly AbilitiesConfigsContainer _abilitiesConfigsContainer;
        private readonly AbilityDropingRulesService _abilityDropingRules;

        public AbilityDropService(
            AbilitiesConfigsContainer abilitiesConfigsContainer,
            AbilityDropingRulesService abilityDropingRules)
        {
            _abilitiesConfigsContainer = abilitiesConfigsContainer;
            _abilityDropingRules = abilityDropingRules;
        }

        public List<AbilityConfig> Drop(int count, Entity entity)
        {
            List<AbilityConfig> availablesAbilities
                = new List<AbilityConfig>(_abilitiesConfigsContainer
                    .AbilityConfigs
                    .Where(abilityOption => _abilityDropingRules.IsAvailable(abilityOption, entity)));

            List<AbilityConfig> selectedAbilities = new();

            for (int i = 0; i < count; i++)
            {
                AbilityConfig selectedAbility = availablesAbilities[UnityEngine.Random.Range(0, availablesAbilities.Count)];
                selectedAbilities.Add(selectedAbility);
                availablesAbilities.Remove(selectedAbility);
            }

            return selectedAbilities;
        }
    }
}