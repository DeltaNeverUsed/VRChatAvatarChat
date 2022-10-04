#if UNITY_EDITOR
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using UnityEditor.Animations;
using AnimatorAsCodeFramework.Examples;
using AnimatorAsCode.V0;
using UnityEditor;

using DeltaNeverUsed.AvChat.NFuncs;

public class Chat : MonoBehaviour
{
    public VRCAvatarDescriptor avatar;
    public AnimatorController assetContainer;
    public string assetKey;

    [Space(20)]
    public GameObject networkContainer;
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
            var sender = aac.CreateSupportingFxLayer("NetworkSender");
            var receiver = aac.CreateSupportingFxLayer("NetworkReceiver");

            //Bit params
            var sBits = new AacFlBoolParameter[8];
            var rBits = new AacFlBoolParameter[8];
            for (var i = 0; i < 8; i++)
            {
                sBits[i] = fx.BoolParameter($"bit{i}");
                rBits[i] = fx.BoolParameter($"rbit{i}");
            }
            
            NetworkingFunctions.Init(
                aac,
                fx,
                sender, receiver,
                sBits, sBits,
                my.networkContainer.transform.Find("NetworkSender").gameObject,
                my.networkContainer.transform.Find("NetworkReceiver").gameObject
                );

            var senderEntry = sender.NewState("Entry");
            var receiverEntry = receiver.NewState("Entry");
            
            
            

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
                    repeat until done (Send Byte and activate byteSendSignal) <<<
                    ChatNetworkOccupied = false
                    return to start
             
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