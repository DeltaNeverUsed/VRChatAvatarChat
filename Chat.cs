#if UNITY_EDITOR
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using UnityEditor.Animations;
using AnimatorAsCodeFramework.Examples;
using AnimatorAsCode.V0;
using UnityEditor;

using DeltaNeverUsed.AvChat.NFuncs;
using DeltaNeverUsed.AvChat.NFuncs.Extra;
using DeltaNeverUsed.AvChat.Screen;

public class Chat : MonoBehaviour
{
    public VRCAvatarDescriptor avatar;
    public AnimatorController assetContainer;
    public string assetKey;

    [Space(20)]
    public GameObject networkContainer;
    public GameObject screenObject;
}

namespace DeltaNeverUsed.AvChat
{
    [CustomEditor(typeof(Chat), true)]
    public class ChatEditor : Editor
    {
        private Chat my;
        private AacFlBase aac;

        public override void OnInspectorGUI()
        {
            var prop = serializedObject.FindProperty("assetKey");
            if (prop.stringValue.Trim() == "")
            {
                prop.stringValue = GUID.Generate().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            
            DrawDefaultInspector();
            
            if (GUILayout.Button("Create")) { Create(); }
            if (GUILayout.Button("Create Shader Crap")) { CreateShaderCrap(); }
        }

        private void CreateShaderCrap()
        {
            string crap = "";
            for (int i = 0; i < 128; i++)
            {
                crap += $"chars[{i}] = floor(_Char{i});\n";
                
            }
            Debug.Log(crap);
        }
        
        private void Create()
        {
            var my = (Chat)target;
            
            var aac = AacExample.AnimatorAsCode("NetworkChat", my.avatar, my.assetContainer, GUID.Generate().ToString());
            
            var fx = aac.CreateMainFxLayer();
            var screen = aac.CreateSupportingFxLayer("Screen");

            var sender = aac.CreateSupportingFxLayer("NetworkSender");
            var receiver = aac.CreateSupportingFxLayer("NetworkReceiver");

            //Bit params
            var bytes = new AacFlFloatParameter[64];
            
            var sBits = new AacFlBoolParameter[8];
            var rBits = new AacFlBoolParameter[8];
            for (var i = 0; i < sBits.Length; i++)
            {
                sBits[i] = fx.BoolParameter($"bit{i}");
                rBits[i] = fx.BoolParameter($"rbit{i}");
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = fx.FloatParameter($"storageByte{i}");
            }
            
            NetworkingFunctions.Init(
                aac,
                fx,
                sender, receiver,
                sBits, rBits,
                my.networkContainer.transform.Find("NetworkSender").gameObject,
                my.networkContainer.transform.Find("NetworkReceiver").gameObject,
                bytes
                );
            ScreenFunctions.Init(aac, screen, new Vector2(32, 2), bytes, my.screenObject.GetComponent<MeshRenderer>());
            ScreenFunctions.CreateScreen();

            var senderEntry = sender.NewState("Entry");
            //var receiverEntry = receiver.NewState("Entry");

            NetworkingFunctions.ReceiveBits();


            // var t = funcs.FloatToBoolParam(fx, fx.FloatParameter("Test"), rBits);
            // var t2 = fx.NewState("t");
            // t.TransitionsTo(t2);
            // t2.Exits().WithTransitionDurationSeconds(1).Automatically();

            //Sending parameters

            //Sending Logic
            var activateSendingSignal = NetworkingFunctions.ActivateSendingSignal();
            
            NetworkingFunctions.CanSend(activateSendingSignal, senderEntry);

            //var tempDst = sender.NewState("Temp");
            
            var serderSM = NetworkingFunctions.SendBits();

            var tempSleep = sender.NewState("sleep");
            NetworkingFunctions.Sleep(tempSleep, activateSendingSignal, 1f);


            NetworkingFunctions.CheckNetworkAvailable(serderSM, tempSleep);

            /* TODO Networking
             Sending:
                if !messageReady
                    return to start
                if chatNetworkOccupied
                    return to start
                
                chatNetworkOccupied = true
                wait 0.5ish seconds
                
                if chatNetworkOccupied
                    return to start
                else 
                    repeat until done (Send Byte and activate byteSendSignal)
                    ChatNetworkOccupied = false
                    return to start <<
             
             Receiving:
                char_ptr = 0
                repeat forever
                    if byteSendSignal
                        Display[char_ptr] write byte
                        char_ptr++
                        
                        return to start
            */
        }
    }
}
#endif