using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Sensors;
using Assets._Project.Develop.Runtime.Utilites;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public abstract class Minigame
    {
        public string Name;

        public abstract void Start();
    }
}
