#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using AnimatorAsCode.V0;
using UnityEngine;

namespace DeltaNeverUsed.AvChat.NFuncs.Extra
{
    public static class funcs
    {
        private static int _memAmount = 0;
        private static int _substateAmount = 0;
        public static int get_mem_id() { _memAmount++; return _memAmount - 1; }
        public static int get_substate_id() { _substateAmount++; return _substateAmount - 1; }

        private static int _stateAmount = 0;
        public static int get_state_id() { _stateAmount++; return _stateAmount - 1; }

        public static bool[] int_to_bool(int input, int size = 12)
        {
            bool[] bools = new bool[size];

            for (int i = 0; i < size; i++)
            {
                int temp = input >> i;
                bools[i] = Convert.ToBoolean(temp & 1);
            }

            return bools;
        }
        public static uint bool_to_int(bool[] input)
        {
            uint integer = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i]) 
                    integer |= (uint)(1 << (input.Length-1)-i);
            }

            return integer;
        }

        public static AacFlStateMachine BoolToFloatParam(AacFlLayer layer, AacFlBoolParameter[] paramBools,
            AacFlFloatParameter paramFloat)
        {
            var sm = layer.NewSubStateMachine("Bool To Int");

            var entry = sm.NewState("Entry");
            entry.Drives(paramFloat, 0);
            
            int value = 1;
            AacFlState lastA = entry;
            AacFlState lastP = entry;
            for (int i = 0; i < paramBools.Length; i++)
            {
                var add = sm.NewState($"bit{i}");
                var pass = sm.NewState($"pass{i}");

                add.DrivingIncreases(paramFloat, value);
                
                lastA.TransitionsTo(add).When(paramBools[i].IsTrue());
                lastP.TransitionsTo(add).When(paramBools[i].IsTrue());
                lastA.TransitionsTo(pass).When(paramBools[i].IsFalse());
                lastP.TransitionsTo(pass).When(paramBools[i].IsFalse());

                
                lastA = add;
                lastP = pass;
                value <<= 1;
            }

            lastA.Exits().Automatically();
            lastP.Exits().Automatically();
            
            return sm;
        }

        public static AacFlStateMachine FloatToBoolParam(AacFlLayer layer, AacFlFloatParameter paramFloat,
            AacFlBoolParameter[] paramBools)
        {
            var sm = layer.NewSubStateMachine("Bool To Int");

            var tempFloat = layer.FloatParameter("TempConvertFloat");
            
            var entry = sm.NewState("Entry");
            entry.DrivingCopies(paramFloat, tempFloat);
            
            int value = 1 << paramBools.Length-1;
            AacFlState last = entry;
            AacFlState lastC = entry;
            for (int i = 0; i < paramBools.Length; i++)
            {
                var check = sm.NewState($"bit{i} check");
                var pass = sm.NewState($"smashOrPass{i}");

                check.Drives(paramBools[i], true);
                pass.Drives(paramBools[i], false);
                
                check.DrivingDecreases(tempFloat, value);
                pass.DrivingIncreases(tempFloat, value);

                check.TransitionsTo(pass).When(tempFloat.IsLessThan(0));
                lastC.TransitionsTo(check).When(tempFloat.IsGreaterThan(-1));
                last.TransitionsTo(check).When(layer.BoolParameter("UNUSED").IsFalse());

                lastC = check;
                last = pass;
                value >>= 1;
            }

            lastC.Exits().Automatically();
            last.Exits().Automatically();
            
            return sm;
        }
    }
}

#endif