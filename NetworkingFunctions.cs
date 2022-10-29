#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using AnimatorAsCode.V0;
using UnityEngine;
using VRC.SDK3.Dynamics.Contact.Components;

using DeltaNeverUsed.AvChat.NFuncs.Extra;
using DeltaNeverUsed.AvChat.Screen;

namespace DeltaNeverUsed.AvChat.NFuncs
{
    public static class NetworkingFunctions
    {
        private static AacFlBase _aac;
        private static AacFlLayer _fx;
        private static AacFlLayer _sender;
        private static AacFlLayer _receiver;
        private static AacFlLayer _bitsLayer;

        private static AacFlBoolParameter[] _Sbits;
        private static AacFlBoolParameter[] _Rbits;
        private static AacFlFloatParameter[] _data;
        private static AacFlBoolParameter _send;
        private static AacFlBoolParameter _sending;

        private static AacFlIntParameter _charPtr;
        private static AacFlIntParameter _charSize;

        private static GameObject _networkSender;
        private static GameObject _networkReceiver;

        private static int _messageLength;

        private static readonly Dictionary<AacFlLayer, AacFlState> ResetNodes =
            new Dictionary<AacFlLayer, AacFlState>();

        public static void Init(AacFlBase tAac, AacFlLayer tFX, AacFlLayer tSender, AacFlLayer tReceiver, AacFlLayer tBitsLayer,
            AacFlBoolParameter[] tSBits, AacFlBoolParameter[] tRBits,
            GameObject tNetworkSender, GameObject tNetworkReceiver,
            AacFlFloatParameter[] tData)
        {
            _aac = tAac;
            _fx = tFX;
            _sender = tSender;
            _receiver = tReceiver;
            _bitsLayer = tBitsLayer;
            _Sbits = tSBits;
            _Rbits = tRBits;
            _data = tData;
            _messageLength = 32;

            _networkSender = tNetworkSender;
            _networkReceiver = tNetworkReceiver;

            _charPtr = _fx.IntParameter("char_ptr");
            _charSize = _fx.IntParameter("char_size");

            _send = _fx.BoolParameter("sent");
            _sending = _fx.BoolParameter("sending");
        }

        public static AacFlState Reset(AacFlLayer layer)
        {
            if (ResetNodes.ContainsKey(layer))
                return ResetNodes[layer];

            var resetNode = layer.NewState("Reset", 10, 10);
            resetNode.Drives(_charPtr, 0);
            resetNode.Drives(_sender.BoolParameter("messageReady"), false);
            resetNode.Drives(_sending, false);
            resetNode.Drives(_sender.BoolParameter("sendo"), false);

            var auiou = _aac.NewClip().Toggling(_networkSender, false);
                
            
            for (int o = 0; o < 8; o++)
            {
                var s = _networkSender.transform.GetChild(o);
                auiou.Toggling(s.gameObject, false);
            }
            resetNode.WithAnimation(auiou);
                
                /*.Animating(clip =>
            {
                clip.Animates(_networkSender.transform, "m_IsActive").WithOneFrame(0);
                //clip.Animates(_networkSender.transform, "m_LocalPosition.x").WithOneFrame(_networkSender.transform.localPosition.x);
                //clip.Animates(_networkSender.transform, "m_LocalPosition.y").WithOneFrame(_networkSender.transform.localPosition.y);
                //clip.Animates(_networkSender.transform, "m_LocalPosition.z").WithOneFrame(0.5f);
                
                for (int o = 0; o < 8; o++)
                {
                    var s = _networkSender.transform.GetChild(o);
                    clip.Animates(s, "m_IsActive").WithOneFrame(0);
                    //clip.Animates(s, "m_LocalPosition.x").WithOneFrame(s.localPosition.x);
                    //clip.Animates(s, "m_LocalPosition.y").WithOneFrame(s.localPosition.y);
                    //clip.Animates(s, "m_LocalPosition.z").WithOneFrame(0.5f);
                }
            }));*/

            resetNode.Exits().Automatically();
            
            ResetNodes.Add(layer, resetNode);
            return resetNode;
        }

        public static void CanSend(AacFlState smDst, AacFlState smSrc)
        {
            // Continue to dst state if the message is ready to be sent and the network isn't Occupied
            smSrc.TransitionsTo(smDst).When(
                _sender.BoolParameter("messageReady").IsTrue()).And(
                _sender.BoolParameter("NetworkOccupied").IsFalse()).And(
                _sender.BoolParameter("IsLocal").IsTrue()); 
        }
        public static void CanSend(AacFlStateMachine smDst, AacFlState smSrc)
        {
            // Continue to dst state if the message is ready to be sent and the network isn't Occupied
            smSrc.TransitionsTo(smDst).When(
                _sender.BoolParameter("messageReady").IsTrue()).And(
                _sender.BoolParameter("NetworkOccupied").IsFalse()).And(
                _sender.BoolParameter("IsLocal").IsTrue()); 
        }

        public static AacFlStateMachine ActivateSendingSignal()
        {
            var sm = _bitsLayer.NewSubStateMachine("NetworkOccupiedSM");
            var activateSendingSignal = sm.NewState("NetworkOccupied")
                .WithAnimation(_aac.NewClip().Toggling(_networkSender, true));
                    
                    /*.Animating(clip =>
                {
                    //clip.Animates(_networkSender.transform, "m_LocalPosition.x").WithOneFrame(_networkSender.transform.localPosition.x);
                    //clip.Animates(_networkSender.transform, "m_LocalPosition.y").WithOneFrame(_networkSender.transform.localPosition.y);
                    //clip.Animates(_networkSender.transform, "m_LocalPosition.z").WithOneFrame(0f);
                    clip.Animates(_networkSender.transform, "m_IsActive").WithOneFrame(1);
                }));*/
            var deactivateSendingSignal = sm.NewState("NetworkOccupied")
                .WithAnimation(_aac.NewClip().Toggling(_networkSender, false));
                    
                    /*.Animating(clip =>
                {
                    //clip.Animates(_networkSender.transform, "m_LocalPosition.x").WithOneFrame(_networkSender.transform.localPosition.x);
                    //clip.Animates(_networkSender.transform, "m_LocalPosition.y").WithOneFrame(_networkSender.transform.localPosition.y);
                    //clip.Animates(_networkSender.transform, "m_LocalPosition.z").WithOneFrame(0.5f);
                    clip.Animates(_networkSender, "m_IsActive").WithOneFrame(0);
                }));*/
            
            sm.EntryTransitionsTo(activateSendingSignal).When(_sending.IsTrue());
            sm.EntryTransitionsTo(deactivateSendingSignal).When(_sending.IsFalse());

            activateSendingSignal.Exits().Automatically();
            deactivateSendingSignal.Exits().Automatically();

            return sm;
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
        public static void CheckNetworkAvailable(AacFlState smDst, AacFlState smSrc)
        {
            CanSend(smDst, smSrc);
            smSrc.TransitionsTo(Reset(_sender)).Automatically();
        }
        
        //Thanks past me
        private static AacFlStateMachine CreateBitAnimations(AacFlStateMachine smLocal, AacFlBoolParameter[] inputP, bool[] inputT)
        {
            var entry = smLocal.NewState("Entry"); // honestly not sure why this is needed, but it is
            var stateAmount = inputT.Length / inputP.Length;
            for (int state = 0; state < stateAmount; state++)
            {
                var tstate = smLocal.NewState($"{state}_BitAnimations");
                var conditions = entry.TransitionsTo(tstate).WhenConditions();
                
                var aeiou = _aac.NewClip();
                for (int o = 0; o < inputP.Length; o++)
                {
                    var s = _networkSender.transform.GetChild(o);
                    //clip.Animates(s, "m_LocalPosition.x").WithOneFrame(s.localPosition.x);
                    //clip.Animates(s, "m_LocalPosition.y").WithOneFrame(s.localPosition.y);
                    //clip.Animates(s,
                    //        "m_LocalPosition.z")
                    //    .WithOneFrame(inputT[o + state * inputP.Length] ? 0f : 0.5f);
                    aeiou.Toggling(s.gameObject, inputT[o + state * inputP.Length]);
                }
                tstate.WithAnimation(aeiou);
                
                /*.Animating(clip =>
                {
                    for (int o = 0; o < inputP.Length; o++)
                    {
                        var s = _networkSender.transform.GetChild(o);
                        //clip.Animates(s, "m_LocalPosition.x").WithOneFrame(s.localPosition.x);
                        //clip.Animates(s, "m_LocalPosition.y").WithOneFrame(s.localPosition.y);
                        //clip.Animates(s,
                        //        "m_LocalPosition.z")
                        //    .WithOneFrame(inputT[o + state * inputP.Length] ? 0f : 0.5f);
                        clip.Animates(s, "m_IsActive").WithOneFrame(inputT[o + state * inputP.Length] ? 1 : 0);
                    }
                }));*/
                    
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
            rstChar.Drives(_sending, true);

            var cpTable = new List<bool>();

            for (int i = 0; i < 256; i++)
            { 
                var temp = funcs.int_to_bool(i, 8);
                foreach (bool t_b in temp)
                {
                    cpTable.Add(t_b);
                }
            }
            
            // Bit animations
            var bitAnimations = _bitsLayer.NewSubStateMachine("BitAnimations");
            var t = CreateBitAnimations(
                bitAnimations,
                _Sbits,
                cpTable.ToArray()
            );
            
            var activateSendingSignal = ActivateSendingSignal();

            t.TransitionsTo(activateSendingSignal);
            
            var buddo = _bitsLayer.NewSubStateMachine("i don't know");

            var sendON = buddo.NewState("sendON")
                .WithAnimation(_aac.NewClip()
                    .Toggling(_networkSender.transform.GetChild(_networkSender.transform.childCount - 1).gameObject, true));
                    
                    /*.Animating(clip => {
                    var s = _networkSender.transform.GetChild(_networkSender.transform.childCount-1);
                    //clip.Animates(s, "m_LocalPosition.x").WithOneFrame(s.localPosition.x);
                    //clip.Animates(s, "m_LocalPosition.y").WithOneFrame(s.localPosition.y);
                    //clip.Animates(s, "m_LocalPosition.z").WithOneFrame(0f);
                    clip.Animates(s, "m_IsActive").WithOneFrame(1);
                }));*/
            var sendOFF = buddo.NewState("sendOFF")
                .WithAnimation(_aac.NewClip()
                    .Toggling(_networkSender.transform.GetChild(_networkSender.transform.childCount - 1).gameObject, false));
                    
                    /*.Animating(clip => {
                    var s = _networkSender.transform.GetChild(_networkSender.transform.childCount-1);
                    //clip.Animates(s, "m_LocalPosition.x").WithOneFrame(s.localPosition.x);
                    //clip.Animates(s, "m_LocalPosition.y").WithOneFrame(s.localPosition.y);
                    //clip.Animates(s, "m_LocalPosition.z").WithOneFrame(0.5f);
                    clip.Animates(s, "m_IsActive").WithOneFrame(0);
                })); */
            buddo.EntryTransitionsTo(sendON).When(_fx.BoolParameter("sendo").IsTrue());
            buddo.EntryTransitionsTo(sendOFF).When(_fx.BoolParameter("sendo").IsFalse());
            sendON.Exits().Automatically();
            sendOFF.Exits().Automatically();

            buddo.Exits();

            t.TransitionsTo(buddo);
            activateSendingSignal.TransitionsTo(buddo);
            // Probably shouldn't be in here

            var storageToByte = dataSender.NewSubStateMachine("Storage to Byte");
            var tempFloatStorage = _sender.FloatParameter("tempFloatStorage");
            
            var ftb = funcs.FloatToBoolParam(storageToByte, tempFloatStorage, _Sbits);
            
            for (int i = 0; i < _data.Length; i++)
            {
                var tempCopy = storageToByte.NewState($"Copy to temp {i}");
                tempCopy.DrivingCopies(_data[i], tempFloatStorage);
                
                storageToByte.EntryTransitionsTo(tempCopy).When(_charPtr.IsEqualTo(i));
                tempCopy.TransitionsTo(ftb).Automatically();
            }

            ftb.Exits();


            rstChar.TransitionsTo(storageToByte).Automatically();

            var tempCheck = _fx.IntParameter("LoopCheck");

            var loop = dataSender.NewState("loop");
            loop.Drives(_sender.BoolParameter("sendo"), true);
            
            var tempSleep = dataSender.NewState("sleep");

            var temptemp = dataSender.NewState("temp kayyy");
            NetworkingFunctions.Sleep(tempSleep, loop, 1f);
            NetworkingFunctions.CheckNetworkAvailable(temptemp, tempSleep);
            
            temptemp.Drives(_sender.BoolParameter("sendo"), false);
            temptemp.DrivingIncreases(_charPtr, 1);

            for (int i = 0; i < 128; i++)
            { temptemp.TransitionsTo(storageToByte).When(_charPtr.IsLessThan(i+1)).And(_charSize.IsGreaterThan(i)); }
            
            temptemp.TransitionsTo(Reset(_sender)).When(_fx.BoolParameter("UNUSED").IsFalse());

            storageToByte.TransitionsTo(loop);

            t.TransitionsTo(loop);
            //loop.TransitionsTo(t).When()

            return dataSender;
        }
        
        
        
        
        // Receive stuff
        
        public static AacFlStateMachine copy_byte(AacFlFloatParameter src)
        {
            var sm = _receiver.NewSubStateMachine("Copy To Storage");
            
            for (int i = 0; i < _messageLength; i++)
            {
                var copy = sm.NewState($"copy_byte");
                copy.DrivingCopies(src, _data[i+_messageLength]);
                copy.DrivingIncreases(_charPtr, 1);

                sm.EntryTransitionsTo(copy).When(_charPtr.IsEqualTo(i));
                copy.Exits().Automatically();
            }
            
            return sm;
        }

        public static void ReceiveBits()
        {
            var tempFloatStorage = _receiver.FloatParameter("tempFloatStorage");

            var entry = _receiver.NewState("Entry");

            var tempCopy = funcs.BoolToFloatParam(_receiver, _Rbits, tempFloatStorage);

            var copy = copy_byte(tempFloatStorage);

            tempCopy.TransitionsTo(copy);
            
            
            var sent = _fx.BoolParameter("pushtempsent");

            var pushMessage = ScreenFunctions.push_message(_receiver, false);
            pushMessage.Drives(_charPtr, 0);
            pushMessage.Drives(sent, true);
            pushMessage.Exits().Automatically();
            
            
            
            entry.TransitionsTo(pushMessage).When(_receiver.BoolParameter("NetworkOccupied").IsTrue())
                .And(_charPtr.IsGreaterThan(0))
                .And(_sending.IsFalse())
                .And(sent.IsFalse());
            entry.TransitionsTo(tempCopy).When(_send.IsTrue()).And(_receiver.BoolParameter("NetworkOccupied").IsTrue());


            //tempCopy.TransitionsTo(tempCopy).wh

            var resetSent = _receiver.NewState("Reset Sent");
            resetSent.Drives(sent, false);
            resetSent.Exits().Automatically();
            
            entry.TransitionsTo(resetSent).When(_send.IsFalse()).And(_receiver.BoolParameter("NetworkOccupied").IsFalse());
            
            copy.TransitionsTo(entry);




        }
    }
}
#endif