#if UNITY_EDITOR
using System.Collections.Generic;
using AnimatorAsCode.V0;
using UnityEngine;
using VRC.SDK3.Dynamics.Contact.Components;

namespace DeltaNeverUsed.AvChat.NFuncs
{
    public static class NetworkingFunctions
    {
        private static AacFlBase _aac;
        private static AacFlLayer _fx;
        private static AacFlLayer _sender;
        private static AacFlLayer _receiver;

        private static AacFlBoolParameter[] _bits;

        public static void Init(AacFlBase tAac, AacFlLayer tFX, AacFlLayer tSender, AacFlLayer tReceiver, AacFlBoolParameter[] tBits)
        {
            _aac = tAac;
            _fx = tFX;
            _sender = tSender;
            _receiver = tReceiver;
            _bits = tBits;
        }

        public static void CanSend(AacFlState smDst, AacFlState smSrc)
        {
            // Continue to dst state if the message is ready to be sent and the network isn't Occupied
            smSrc.TransitionsTo(smDst).When(
                _sender.BoolParameter("messageReady").IsTrue()).And(
                _sender.BoolParameter("NetworkOccupied").IsFalse()); 
        }

        public static AacFlState ActivateSendingSignal(GameObject networkSender)
        {
            var activateSendingSignal = _sender.NewState("NetworkOccupied")
                .WithAnimation(_aac.NewClip().Animating(clip =>
                {
                    clip.Animates(networkSender.GetComponent<VRCContactSender>(), "radius")
                        .WithOneFrame(6f);
                }));

            return activateSendingSignal;
        }

        public static void Sleep(AacFlState smDst, AacFlState smSrc, float seconds)
        {
            
        }
    }
}
#endif