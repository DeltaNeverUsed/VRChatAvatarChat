#if UNITY_EDITOR
using System;
using System.Linq;
using AnimatorAsCode.V0;
using UnityEngine;

namespace DeltaNeverUsed.AvChat.Keypad
{
    public static class KeypadCreator
    {
        private static GameObject _keypad;
        private static AacFlBase _aac;
        private static AacFlLayer _keypadLayer;
        private static AacFlFloatParameter[] _data;
        private static AacFlIntParameter _charSize;

        private static AacFlBoolParameter[] _x;
        private static AacFlBoolParameter[] _y;

        public static void Init(AacFlBase tAac, AacFlLayer tKeypadLayer, GameObject tKeypad, AacFlFloatParameter[] tData, Vector2 tSize)
        {
            _keypadLayer = tKeypadLayer;
            _keypad = tKeypad;
            _data = tData;
            _aac = tAac;
            
            _charSize = _keypadLayer.IntParameter("char_size");

            _x = new AacFlBoolParameter[3];
            _y = new AacFlBoolParameter[3];
            for (int i = 0; i < 3; i++)
            {
                _x[i] = _keypadLayer.BoolParameter($"X{i}");
                _y[i] = _keypadLayer.BoolParameter($"Y{i}");
            }
            
        }

        private static void WaitForUnClick(AacFlState entry, AacFlState proceed)
        {
            // Check if button is no longer pressed
            var entryTrans = entry.TransitionsTo(proceed).WhenConditions();
            var tempXY = _x.Concat(_y).ToArray();
            
            foreach (var param in tempXY)
            { entryTrans.And(param.IsFalse()); }
        }

        private static void DriveData(AacFlStateMachine sm, int value)
        {
            int pos = 0;
            var tempThing = _keypadLayer.FloatParameter("TempFloatDataDriver");
            
            var setTempData = sm.NewState("DriveData");
            sm.EntryTransitionsTo(setTempData).When(_charSize.IsEqualTo(pos));
            setTempData.Exits().Automatically();
            
            setTempData.Drives(tempThing, value);
        }

        public static void CreateFields()
        {
            var entry = _keypadLayer.NewState("Entry");
            var proceed = _keypadLayer.NewState("Proceed");
            WaitForUnClick(entry, proceed);
            
            entry.WithAnimation(_aac.NewClip().Animating(clip =>
            {
                clip.Animates(_keypad.GetComponent<MeshRenderer>(), "material._XOffset").WithOneFrame(0);
                clip.Animates(_keypad.GetComponent<MeshRenderer>(), "material._YOffset").WithOneFrame(0.75f);
            }));

            string[] fields =
            {
                "ABCDEFGHI", "JKLMNOPQ", "RSTUVWXYZ",
                "abcdefghi", "jklmnopq", "rstuvwxyz",
                "012345678", "9!?\"#()+-", " \x08"
            };
            
            for (int i = 0; i < fields.Length; i++)
            {
                int tempI = i + 1;
                int Fakex = tempI % 3;
                int Fakey = tempI / 3;
                int x = i % 3;
                int y = i / 3;
                var subField = CreateSubFields(fields[i].ToCharArray(), new Vector2(1.0f / 3.0f * Fakex, 1 - 1.0f / 4.0f * (Fakey+1)));
                var t = proceed.TransitionsTo(subField).WhenConditions();
                t.And(_x[x].IsTrue());
                t.And(_y[y].IsTrue());

                subField.Exits();
            }
        }

        private static AacFlStateMachine CreateSubFields(char[] chars, Vector2 keypadPos)
        {
            var subField = _keypadLayer.NewSubStateMachine(new string(chars));

            var entry = subField.NewState("Entry");
            var proceed = subField.NewState("Proceed");

            WaitForUnClick(entry, proceed);

            entry.WithAnimation(_aac.NewClip().Animating(clip =>
            {
                clip.Animates(_keypad.GetComponent<MeshRenderer>(), "material._XOffset").WithOneFrame(keypadPos.x);
                clip.Animates(_keypad.GetComponent<MeshRenderer>(), "material._YOffset").WithOneFrame(keypadPos.y);
            }));

            var tempThing = _keypadLayer.FloatParameter("TempFloatDataDriver");

            var dataDriver = subField.NewSubStateMachine("DriveData");
            var dataDriverEntry = dataDriver.NewState("Entry");
            
            var setDataDec = dataDriver.NewState("setDataDec").DrivingDecreases(_charSize, 1);
            var setDataInc = dataDriver.NewState("setDataInc").DrivingIncreases(_charSize, 1);

            dataDriverEntry.TransitionsTo(setDataDec)
                .When(tempThing.IsLessThan('\x08' + 0.5f - 32))
                .And(tempThing.IsGreaterThan('\x08' - 0.5f - 32))
                .And(_charSize.IsGreaterThan(0));
            setDataInc.Exits().Automatically();

            int pos = 0;
            foreach (var data in _data)
            {
                var setData = dataDriver.NewState("DriveData");
                setDataDec.TransitionsTo(setData).When(_charSize.IsEqualTo(pos));
                dataDriverEntry.TransitionsTo(setData).When(_charSize.IsEqualTo(pos));
                
                setData.DrivingCopies(tempThing, data);

                setData.TransitionsTo(setDataInc)
                    .When(tempThing.IsLessThan('\x08' - 0.5f - 32))
                    .Or().When(tempThing.IsGreaterThan('\x08' + 0.5f - 32));
                setData.Exits().Automatically();

                pos++;
            }


            for (int i = 0; i < chars.Length; i++)
            {
                // ReSharper disable once PossibleLossOfFraction
                Vector2 xy = new Vector2(i % 3, i / 3);
                
                var field = subField.NewSubStateMachine($"Field{i}");
                DriveData(field, chars[i]-32);

                proceed.TransitionsTo(field).When(_x[(int)xy.x].IsTrue()).And(_y[(int)xy.y].IsTrue());

                field.TransitionsTo(dataDriver);
            }

            dataDriver.Exits();

            return subField;
        }
    }

}
#endif