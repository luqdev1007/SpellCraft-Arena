using Assets._Project.Develop.Runtime.Utilites.Timer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class TowerDefenceMode : Minigame
    {
        private readonly MinigamesCreaturesFactory _creaturesFactory;

        private Transform[] _spawnPoints; // config

        private List<Tower> _towers = new();
        private List<ExplosiveDevil> _enemies = new();

        private int _wavesCount = 2;
        private int _stagesCount = 2;
        private float _spawnCooldown = 3;

        private TimerService _timer;

        public TowerDefenceMode(MinigamesCreaturesFactory factory, TimerServiceFactory timerServiceFactory)
        {
            _creaturesFactory = factory;
            _timer = timerServiceFactory.Create(_spawnCooldown);
        }

        public override bool IsMinigameCompleted { get; protected set; } = false;
        public override bool PreperationOver { get; protected set; } = false;
        public override bool ReturnToPreperation { get; protected set; } = false;

        public override void Init()
        {
            Debug.Log("td mode begin!");

            IsMinigameCompleted = false;

            Transform[] positions = Object.FindObjectsByType<Transform>(FindObjectsSortMode.None);
            _spawnPoints = positions.Where(i => i.gameObject.name.ToLower().Contains("tower")).ToArray(); // kek

            foreach (Transform spawnPoint in _spawnPoints)
            {
                _towers.Add(_creaturesFactory.CreateTower(spawnPoint));
            }

            ReturnToPreperation = true;
        }

        public override void StartPreperation()
        {
            ReturnToPreperation = false;
            PreperationOver = false;

            _stagesCount = 2;

            // show ui
            Button button = Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .First(b => b.gameObject.name.Equals("StartWaveButton"));

            button.gameObject.SetActive(true);

            button.onClick.AddListener(() =>
            {
                PreperationOver = true;
                button.gameObject.SetActive(false);
            });
        }

        public override void Update(float deltaTime)
        {
            if (_stagesCount > 0 && _timer.IsOver)
            {
                for (int i = 0; i < 4; i++)
                {
                    ExplosiveDevil devil = _creaturesFactory.CreateExplosiveDevil();
                    _enemies.Add(devil);
                    devil.SetTarget(_towers[Random.Range(0, _towers.Count)]);
                }

                _stagesCount--;
                _timer.Restart();
            }
            else if (_stagesCount == 0 && AllEnemiesDead())
            {
                _wavesCount--;

                if (_wavesCount == 0)
                    IsMinigameCompleted = true;
                else
                    ReturnToPreperation = true;
            }
        }

        private bool AllEnemiesDead()
        {
            foreach (ExplosiveDevil enemy in _enemies)
                if (enemy.gameObject.activeSelf)
                    return false;

            return true;
        }
    }
}
