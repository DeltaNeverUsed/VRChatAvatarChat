﻿#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using AnimatorAsCode.V0;
using UnityEngine;
using VRC.SDK3.Dynamics.Contact.Components;

using DeltaNeverUsed.AvChat.NFuncs.Extra;

namespace DeltaNeverUsed.AvChat.NFuncs
{
    public static class NetworkingFunctions
    {
        private static AacFlBase _aac;
        private static AacFlLayer _fx;
        private static AacFlLayer _sender;
        private static AacFlLayer _receiver;

        private static AacFlBoolParameter[] _Sbits;
        private static AacFlBoolParameter[] _Rbits;

        private static AacFlIntParameter _charPtr;
        private static AacFlIntParameter _charSize;

        private static GameObject _networkSender;
        private static GameObject _networkReceiver;

        private static readonly Dictionary<AacFlLayer, AacFlState> ResetNodes =
            new Dictionary<AacFlLayer, AacFlState>();

        public static void Init(AacFlBase tAac, AacFlLayer tFX, AacFlLayer tSender, AacFlLayer tReceiver,
            AacFlBoolParameter[] tSBits, AacFlBoolParameter[] tRBits,
            GameObject tNetworkSender, GameObject tNetworkReceiver)
        {
            _aac = tAac;
            _fx = tFX;
            _sender = tSender;
            _receiver = tReceiver;
            _Sbits = tSBits;
            _Rbits = tRBits;

            _networkSender = tNetworkSender;
            _networkReceiver = tNetworkReceiver;

            _charPtr = _fx.IntParameter("char_ptr");
            _charSize = _fx.IntParameter("char_size");
        }

        public static AacFlState Reset(AacFlLayer layer)
        {
            if (ResetNodes.ContainsKey(layer))
                return ResetNodes[layer];

            var resetNode = layer.NewState("Reset", 10, 10);
            
            resetNode.WithAnimation(_aac.NewClip().Animating(clip =>
            {
                clip.Animates(_networkSender.transform, "m_LocalPosition.x").WithOneFrame(_networkSender.transform.localPosition.x);
                clip.Animates(_networkSender.transform, "m_LocalPosition.y").WithOneFrame(_networkSender.transform.localPosition.y);
                clip.Animates(_networkSender.transform, "m_LocalPosition.z").WithOneFrame(0.5f);
                
                for (int o = 0; o < 8; o++)
                {
                    var s = _networkSender.transform.GetChild(o);
                    clip.Animates(s, "m_LocalPosition.x").WithOneFrame(s.localPosition.x);
                    clip.Animates(s, "m_LocalPosition.y").WithOneFrame(s.localPosition.y);
                    clip.Animates(s, "m_LocalPosition.z").WithOneFrame(0.5f);
                }
            }));

            resetNode.Exits().Automatically();
            
            ResetNodes.Add(layer, resetNode);
            return resetNode;
        }

        public static void CanSend(AacFlState smDst, AacFlState smSrc)
        {
            // Continue to dst state if the message is ready to be sent and the network isn't Occupied
            smSrc.TransitionsTo(smDst).When(
                _sender.BoolParameter("messageReady").IsTrue()).And(
                _sender.BoolParameter("NetworkOccupied").IsFalse()); 
        }
        public static void CanSend(AacFlStateMachine smDst, AacFlState smSrc)
        {
            // Continue to dst state if the message is ready to be sent and the network isn't Occupied
            smSrc.TransitionsTo(smDst).When(
                _sender.BoolParameter("messageReady").IsTrue()).And(
                _sender.BoolParameter("NetworkOccupied").IsFalse()); 
        }

        public static AacFlState ActivateSendingSignal()
        {
            var activateSendingSignal = _sender.NewState("NetworkOccupied")
                .WithAnimation(_aac.NewClip().Animating(clip =>
                {
                    clip.Animates(_networkSender.transform, "m_LocalPosition.x").WithOneFrame(_networkSender.transform.localPosition.x);
                    clip.Animates(_networkSender.transform, "m_LocalPosition.y").WithOneFrame(_networkSender.transform.localPosition.y);
                    clip.Animates(_networkSender.transform, "m_LocalPosition.z").WithOneFrame(0f);
                }));

            return activateSendingSignal;
        }

        public static void Sleep(AacFlState smDst, AacFlState smSrc, float seconds)
        {
            smSrc.TransitionsTo(smDst).WithTransitionDurationSeconds(seconds).Automatically();
        }
        
        public static void CheckNetworkAvailable(AacFlStateMachine smDst, AacFlState smSrc)
        {
            CanSend(smDst, smSrc);
            smSrc.TransitionsTo(Reset(_sender)).Automatically();
        }
        
        //Thanks past me
        private static AacFlStateMachine CreateBitAnimations(AacFlStateMachine sm, AacFlBoolParameter[] inputP, bool[] inputT)
        {
            var smLocal = sm.NewSubStateMachine("BitAnimations");

            var stateAmount = inputT.Length / inputP.Length;
            for (int state = 0; state < stateAmount; state++)
            {
                var tstate = smLocal.NewState($"{state}_BitAnimations");
                var conditions = smLocal.EntryTransitionsTo(tstate).WhenConditions();

                
                tstate.WithAnimation(_aac.NewClip().Animating(clip =>
                {
                    for (int o = 0; o < inputP.Length; o++)
                    {
                        var s = _networkSender.transform.GetChild(o);
                        clip.Animates(s, "m_LocalPosition.x").WithOneFrame(s.localPosition.x);
                        clip.Animates(s, "m_LocalPosition.y").WithOneFrame(s.localPosition.y);
                        clip.Animates(s,
                                "m_LocalPosition.z")
                            .WithOneFrame(inputT[o + state * inputP.Length] ? 0f : 0.5f);
                    }
                }));
                    
                for (int i = 0; i < inputP.Length; i++)
                {
                    conditions.And(inputP[i].IsEqualTo(inputT[i + state * inputP.Length]));
                }
                tstate.Exits().When(_fx.BoolParameter("UNUSED").IsFalse());
            }

            return smLocal;
        }

        public static AacFlStateMachine SendBits()
        {
            var dataSender = _sender.NewSubStateMachine("Data sender");

            var rstChar = dataSender.NewState("Reset Char pointer");
            rstChar.Drives(_charPtr, 0);
            
            //Start
            
            var cpTable = new List<bool>();

            for (int i = 0; i < 256; i++)
            { 
                var temp = funcs.int_to_bool(i, 8);
                foreach (bool t_b in temp)
                {
                    cpTable.Add(t_b);
                }
            }
            
            var t = CreateBitAnimations(
                dataSender,
                _Sbits,
                cpTable.ToArray()
            );

            rstChar.TransitionsTo(t).Automatically();
            t.Exits();
            
            //End
            
            return dataSender;
        }
    }
}
#endif