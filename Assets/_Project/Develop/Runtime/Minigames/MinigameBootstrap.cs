using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class MinigameBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private MinigameInputArgs _inputArgs;

        private EntitiesLifeContext _entitiesLifeContext;
        private AIBrainsContext _brainsContext;

        private MinigameStatesContext _minigameStatesContext;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not MinigameInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(MinigameInputArgs)} type");

            _inputArgs = gameplayInputArgs;

            MinigameContextRegistrations.Process(_container, _inputArgs);
        }

        public override IEnumerator Initialize()
        {
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();

            _brainsContext = _container.Resolve<AIBrainsContext>();

            _minigameStatesContext = _container.Resolve<MinigameStatesContext>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log($"Start minigame");

            _minigameStatesContext.Run();
        }

        private void Update()
        {
            _brainsContext?.Update(Time.deltaTime);

            _entitiesLifeContext?.Update(Time.deltaTime);

            _minigameStatesContext?.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            
        }
    }
}
