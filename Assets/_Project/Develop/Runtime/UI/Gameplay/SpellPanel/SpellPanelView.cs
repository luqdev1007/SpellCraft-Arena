using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.SpellPanel
{
    public class SpellPanelView : MonoBehaviour, IView
    {
        [field: SerializeField] public AspectButtonView[] AspectButtons { get; private set; } = new AspectButtonView[7];
        [field: SerializeField] public Button CastButton { get; private set; }
        [field: SerializeField] public Image ActiveSpellIcon { get; private set; }
        [field: SerializeField] public Button TeleportButton { get; private set; }
        [field: SerializeField] public Image BlinkCooldownFillImage { get; private set; }
        [field: SerializeField] public Button ShieldButton { get; private set; }
        [field: SerializeField] public Image ShieldFrameImage { get; private set; }
        [field: SerializeField] public Image ShieldIconImage { get; private set; }
    }
}
