#if UNITY_EDITOR
using AnimatorAsCode.V0;
using UnityEngine;

using DeltaNeverUsed.AvChat.NFuncs.Extra;
using UnityEditor.Animations;

namespace  DeltaNeverUsed.AvChat.Screen
{
    public static class ScreenFunctions
    {
        private static AacFlBase _aac;
        private static AacFlLayer _screenLayer;
        private static Vector2 _screenSize;
        private static AacFlFloatParameter[] _data;
        private static MeshRenderer _screen;

        public static void Init(AacFlBase tAac, AacFlLayer tScreenLayer, Vector2 tScreenSize, AacFlFloatParameter[] tData, MeshRenderer tScreen)
        {
            _screenLayer = tScreenLayer;
            _screenSize = tScreenSize;
            _data = tData;
            _aac = tAac;
            _screen = tScreen;
        }

        public static void CreateScreen()
        {
            var entry = _screenLayer.NewState("Entry");

            var last = entry;
            for (int i = 0; i < _data.Length; i++)
            {
                var proxyTree = _aac.NewBlendTreeAsRaw();
                proxyTree.blendParameter = _data[i].Name;
                proxyTree.blendType = BlendTreeType.Simple1D;
                proxyTree.minThreshold = 0;
                proxyTree.maxThreshold = 255;
                proxyTree.useAutomaticThresholds = true;
                proxyTree.children = new[]
                {
                    new ChildMotion {motion = _aac.NewClip().Animating(clip =>
                    {
                        clip.Animates(_screen, $"material._Char{i}").WithOneFrame(0);
                    }).Clip, timeScale = 1, threshold = 0},
                    new ChildMotion {motion = _aac.NewClip().Animating(clip =>
                    {
                        clip.Animates(_screen, $"material._Char{i}").WithOneFrame(255);
                    }).Clip, timeScale = 1, threshold = 1}
                };

                var s = _screenLayer.NewState($"display{i}");
                s.WithAnimation(proxyTree);

                last.TransitionsTo(s).Automatically();
                last = s;
            }

            last.Exits().Automatically();
        }

        public static AacFlState push_message(AacFlLayer tScreenLayer)
        {
            var t = tScreenLayer.NewState("Push");
            
            for (int x = 0; x < _screenSize.x; x++)
            {
                t.Drives(_data[x], 0);
            }

            for (int y = 0; y < (int)_screenSize.y-1; y++)
            {
                for (int x = 0; x < _screenSize.x; x++)
                {
                    int temp = (int)_screenSize.y - 1 - y;
                    //Debug.Log(x + (int)_screenSize.x * temp);
                    t.DrivingCopies(_data[x + (int)_screenSize.x * (temp-1)], _data[x + (int)_screenSize.x * temp]);
                }
            }

            return t;
        }
    }

}
#endif