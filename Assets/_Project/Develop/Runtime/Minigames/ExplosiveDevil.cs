using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Minigames
{
    public class ExplosiveDevil : MonoBehaviour
    {
        public void SetTarget(Tower tower)
        {
            transform.LookAt(tower.transform.position);
        }
    }
}
