#if UNITY_EDITOR
using System;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using UnityEditor.Animations;
using AnimatorAsCodeFramework.Examples;
using AnimatorAsCode.V0;
using UnityEditor;

using DeltaNeverUsed.AvChat.NFuncs;
using DeltaNeverUsed.AvChat.NFuncs.Extra;
using DeltaNeverUsed.AvChat.Screen;
using DeltaNeverUsed.AvChat.Keypad;

public class Chat : MonoBehaviour
{
    public VRCAvatarDescriptor avatar;
    public AnimatorController assetContainer;
    public string assetKey;

    [Space(20)]
    public GameObject networkContainer;
    public GameObject screenObject;
    public GameObject keypadObject;
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
        }

        private void Create()
        {
            var my = (Chat)target;
            
            var aac = AacExample.AnimatorAsCode("NetworkChat", my.avatar, my.assetContainer, GUID.Generate().ToString());
            
            var fx = aac.CreateMainFxLayer();
            var screen = aac.CreateSupportingFxLayer("Screen");
            var keypadLayer = aac.CreateSupportingFxLayer("Keypad");

            var sender = aac.CreateSupportingFxLayer("NetworkSender");
            var bitsLayer = aac.CreateSupportingFxLayer("Bittos");
            var receiver = aac.CreateSupportingFxLayer("NetworkReceiver");

            //Bit params
            var bytes = new AacFlFloatParameter[96];
            var keypadBytes = new AacFlFloatParameter[32];
            
            var sBits = new AacFlBoolParameter[8];
            var rBits = new AacFlBoolParameter[8];
            for (var i = 0; i < sBits.Length; i++)
            {
                sBits[i] = fx.BoolParameter($"bit{i}");
                rBits[i] = fx.BoolParameter($"rbit{i}");
            }
            
            funcs.Init(fx);

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = fx.FloatParameter($"storageByte{i}");
            }
            Array.Copy(bytes, keypadBytes, keypadBytes.Length);
            
            NetworkingFunctions.Init(
                aac,
                fx,
                sender, receiver, bitsLayer,
                sBits, rBits,
                my.networkContainer.transform.Find("NetworkSender").gameObject,
                my.networkContainer.transform.Find("NetworkReceiver").gameObject,
                bytes
                );
            ScreenFunctions.Init(aac, screen, new Vector2(32, 3), bytes, my.screenObject.GetComponent<MeshRenderer>());
            ScreenFunctions.CreateScreen();

            KeypadCreator.Init(aac, keypadLayer, my.keypadObject, keypadBytes, new Vector2(3,3));
            KeypadCreator.CreateFields();
            
            
            var sendButton = aac.CreateSupportingFxLayer("SendButton");
            var sendButtonEntry = sendButton.NewState("Entry");
            var sendMessage = sendButton.NewState("SendMessage");

            sendMessage.Drives(fx.BoolParameter("messageReady"), true);
            sendButtonEntry.TransitionsTo(sendMessage).When(fx.BoolParameter("ShootMessage").IsTrue());
            sendMessage.Exits().Automatically();

            var senderEntry = sender.NewState("Entry");
            //var receiverEntry = receiver.NewState("Entry");
            

            NetworkingFunctions.ReceiveBits();
            

            //Sending parameters

            //Sending Logic
            var aaa = sender.NewState("temp thing");
            NetworkingFunctions.CanSend(aaa, senderEntry);

            var pushMsgSendparam = fx.BoolParameter("pushMsgSendparam");

            var pushMsgSend = ScreenFunctions.push_message(sender, true);
            senderEntry.TransitionsTo(pushMsgSend).When(pushMsgSendparam.IsFalse())
                .And(fx.BoolParameter("messageReady").IsFalse())
                .And(fx.BoolParameter("IsLocal").IsTrue());
            pushMsgSend.Exits().Automatically();

            pushMsgSend.Drives(pushMsgSendparam, true);
            aaa.Drives(pushMsgSendparam, false);

            //var tempDst = sender.NewState("Temp");
            
            var senderSM = NetworkingFunctions.SendBits();
            aaa.TransitionsTo(senderSM).Automatically();

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