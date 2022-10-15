#if UNITY_EDITOR
using AnimatorAsCode.V0;
using UnityEngine;

namespace DeltaNeverUsed.AvChat.Keypad
{
    public class Keypad
    {
        private AacFlLayer _keypadLayer;
        private AacFlBoolParameter _keyPressed;

        public void Init(AacFlLayer tKeypadLayer, Vector2 tSize)
        {
            _keypadLayer = tKeypadLayer;

            _keyPressed = _keypadLayer.BoolParameter("KeyPressed");
        }

        public void CreateFields()
        {
            
        }

        private void CreateSubFields(char[] chars)
        {
            var subField = _keypadLayer.NewSubStateMachine(chars.ToString());
            
            
            for (int i = 0; i < chars.Length; i++)
            {
                
            }
        }
    }

}
#endif