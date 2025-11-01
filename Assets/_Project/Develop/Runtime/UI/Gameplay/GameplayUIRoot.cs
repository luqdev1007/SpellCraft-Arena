using Assets._Project.Develop.Runtime.UI.MainMenu;
using Assets._Project.Develop.Runtime.UI;
using UnityEngine;

public class GameplayUIRoot : MonoBehaviour
{
    [field: SerializeField] public Transform HUDLayer { get; private set; }
    [field: SerializeField] public Transform PopupsLayer { get; private set; }
    [field: SerializeField] public Transform VFXUnderPopupsLayer { get; private set; }
    [field: SerializeField] public Transform VFXOverPopupsLayer { get; private set; }
}
