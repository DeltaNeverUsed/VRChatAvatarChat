using System.Collections;
using System.Collections.Generic;
using AnimatorAsCode.V0;
using UnityEngine;

using DeltaNeverUsed.AvChat.NFuncs.Extra;

namespace  DeltaNeverUsed.AvChat.Screen
{
    public static class ScreenFunctions
    {
        private static AacFlLayer _screenLayer;
        private static Vector2 _screenSize;

        public static void Init(AacFlLayer tScreenLayer, Vector2 tScreenSize)
        {
            _screenLayer = tScreenLayer;
            _screenSize = tScreenSize;
        }

        public static void CreateScreen()
        {
            
        }
    }

}