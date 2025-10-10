using Assets._Project.Develop.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuContextRegistrations
    {
        public static void Process(DIContainer container)
        {
            Debug.Log("Process registrations on main menu scene");
        }
    }
}
