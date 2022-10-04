#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}

#endif