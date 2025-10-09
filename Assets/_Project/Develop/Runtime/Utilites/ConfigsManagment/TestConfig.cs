using UnityEngine;

namespace Assets._Project.Develop.Runtime.Utilites.ConfigsManagment
{
    [CreateAssetMenu(fileName = "Test Config", menuName = "StaticData/Configs/Test Config", order = 54)]
    public class TestConfig : ScriptableObject
    {
        [field: SerializeField] public string Message { get; private set; } = "Hello, World!";
    }
}