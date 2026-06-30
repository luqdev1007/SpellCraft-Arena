using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature
{
    public class MobileInput : IInputService
    {
        public bool IsEnabled { get; set; } = true;

        public Vector3 MoveDirection
        {
            get
            {
                if (IsEnabled == false)
                    return Vector3.zero;

                float h = SimpleInput.GetAxis("Horizontal");
                float v = SimpleInput.GetAxis("Vertical");

                return new Vector3(h, 0f, v);
            }
        }

        public Vector3 RotateDirection => MoveDirection;

        public bool IsAttackKeyPressed => false;

        public bool IsCastRequested => false;

        public bool IsBlinkRequested => false;
    }
}
