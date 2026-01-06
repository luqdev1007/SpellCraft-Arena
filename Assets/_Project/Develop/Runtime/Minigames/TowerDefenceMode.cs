using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using System.Linq;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class TowerDefenceMode : Minigame
    {
        private readonly EntitiesFactory _entitiesFactory;

        private Transform[] _spawnPoints; // config

        public TowerDefenceMode(EntitiesFactory entitiesFactory)
        {
            Name = "Tower defence";

            _entitiesFactory = entitiesFactory;
        }

        public override void Start()
        {
            Debug.Log(Name + " mode is begin!");

            Transform[] positions = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
            _spawnPoints = positions.Where(i => i.gameObject.name.ToLower().Contains("tower")).ToArray(); // kek

            foreach (Transform spawn in _spawnPoints)
            {
                _entitiesFactory.CreateTower(spawn);
            }
        }
    }
}
