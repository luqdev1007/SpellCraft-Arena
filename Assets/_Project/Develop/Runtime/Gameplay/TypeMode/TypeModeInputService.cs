using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeInputService
    {
        public bool TryGetPressedSymbol(out string symbol)
        {
            symbol = null;

            if (!Input.anyKeyDown)
                return false;

            string pressed = Input.inputString.ToUpper();

            if (string.IsNullOrEmpty(pressed))
                return false;

            symbol = pressed;
            return true;
        }

        public bool IsContinuePressed()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}