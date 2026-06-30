using Assets._Project.Develop.Runtime.Configs.Gameplay.Loot;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LootFeature
{
    public class DropLootService
    {
        private LootListConfig _lootListConfig;
        private LootFactory _lootFactory;

        public DropLootService(LootListConfig lootListConfig, LootFactory lootFactory)
        {
            _lootListConfig = lootListConfig;
            _lootFactory = lootFactory;
        }

        public void DropLootFor(Entity entity)
        {
            Transform entityTransform = entity.Transform;

            DropCoins(entityTransform.position);
            DropHealth(entityTransform.position);
        }

        public void DropCoins(Vector3 position)
        {
            List<CoinsLootConfig> coinsConfigs = _lootListConfig.LootConfigs
                .Where(loot => loot.GetType() == typeof(CoinsLootConfig))
                .Cast<CoinsLootConfig>()
                .ToList();

            if (coinsConfigs.Count > 0 && Random.Range(0, 100) > 50)
            {
                CoinsLootConfig coinsLootConfig = coinsConfigs[Random.Range(0, coinsConfigs.Count)];

                _lootFactory.CreateCoinsLoot(coinsLootConfig.PrefabPath, position, coinsLootConfig.Coins);
            }
        }

        public void DropHealth(Vector3 position)
        {
            List<HealthLootConfig> healthConfigs = _lootListConfig.LootConfigs
                .Where(loot => loot.GetType() == typeof(HealthLootConfig))
                .Cast<HealthLootConfig>()
                .ToList();

            if (healthConfigs.Count > 0 && Random.Range(0, 100) > 50)
            {
                HealthLootConfig healthLootConfig = healthConfigs[Random.Range(0, healthConfigs.Count)];

                _lootFactory.CreateHealthLoot(healthLootConfig.PrefabPath, position, healthLootConfig.Health);
            }
        }
    }
}
