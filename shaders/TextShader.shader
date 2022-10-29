Shader "Delta/TextShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SizeX ("Size X", int)          = 16
        _SizeY ("Size Y", int)          = 8
        _FontX ("Font X", int)          = 16
        _FontY ("Font Y", int)          = 16
        [MaterialToggle] _Swap ("Swap every other line", int) = 0
        
        _Char0 ("_Char0", float)        = 0
        _Char1 ("_Char1", float)        = 0
        _Char2 ("_Char2", float)        = 0
        _Char3 ("_Char3", float)        = 0
        _Char4 ("_Char4", float)        = 0
        _Char5 ("_Char5", float)        = 0
        _Char6 ("_Char6", float)        = 0
        _Char7 ("_Char7", float)        = 0
        _Char8 ("_Char8", float)        = 0
        _Char9 ("_Char9", float)        = 0
        _Char10 ("_Char10", float)      = 0
        _Char11 ("_Char11", float)      = 0
        _Char12 ("_Char12", float)      = 0
        _Char13 ("_Char13", float)      = 0
        _Char14 ("_Char14", float)      = 0
        _Char15 ("_Char15", float)      = 0
        _Char16 ("_Char16", float)      = 0
        _Char17 ("_Char17", float)      = 0
        _Char18 ("_Char18", float)      = 0
        _Char19 ("_Char19", float)      = 0
        _Char20 ("_Char20", float)      = 0
        _Char21 ("_Char21", float)      = 0
        _Char22 ("_Char22", float)      = 0
        _Char23 ("_Char23", float)      = 0
        _Char24 ("_Char24", float)      = 0
        _Char25 ("_Char25", float)      = 0
        _Char26 ("_Char26", float)      = 0
        _Char27 ("_Char27", float)      = 0
        _Char28 ("_Char28", float)      = 0
        _Char29 ("_Char29", float)      = 0
        _Char30 ("_Char30", float)      = 0
        _Char31 ("_Char31", float)      = 0
        _Char32 ("_Char32", float)      = 0
        _Char33 ("_Char33", float)      = 0
        _Char34 ("_Char34", float)      = 0
        _Char35 ("_Char35", float)      = 0
        _Char36 ("_Char36", float)      = 0
        _Char37 ("_Char37", float)      = 0
        _Char38 ("_Char38", float)      = 0
        _Char39 ("_Char39", float)      = 0
        _Char40 ("_Char40", float)      = 0
        _Char41 ("_Char41", float)      = 0
        _Char42 ("_Char42", float)      = 0
        _Char43 ("_Char43", float)      = 0
        _Char44 ("_Char44", float)      = 0
        _Char45 ("_Char45", float)      = 0
        _Char46 ("_Char46", float)      = 0
        _Char47 ("_Char47", float)      = 0
        _Char48 ("_Char48", float)      = 0
        _Char49 ("_Char49", float)      = 0
        _Char50 ("_Char50", float)      = 0
        _Char51 ("_Char51", float)      = 0
        _Char52 ("_Char52", float)      = 0
        _Char53 ("_Char53", float)      = 0
        _Char54 ("_Char54", float)      = 0
        _Char55 ("_Char55", float)      = 0
        _Char56 ("_Char56", float)      = 0
        _Char57 ("_Char57", float)      = 0
        _Char58 ("_Char58", float)      = 0
        _Char59 ("_Char59", float)      = 0
        _Char60 ("_Char60", float)      = 0
        _Char61 ("_Char61", float)      = 0
        _Char62 ("_Char62", float)      = 0
        _Char63 ("_Char63", float)      = 0
        _Char64 ("_Char64", float)      = 0
        _Char65 ("_Char65", float)      = 0
        _Char66 ("_Char66", float)      = 0
        _Char67 ("_Char67", float)      = 0
        _Char68 ("_Char68", float)      = 0
        _Char69 ("_Char69", float)      = 0
        _Char70 ("_Char70", float)      = 0
        _Char71 ("_Char71", float)      = 0
        _Char72 ("_Char72", float)      = 0
        _Char73 ("_Char73", float)      = 0
        _Char74 ("_Char74", float)      = 0
        _Char75 ("_Char75", float)      = 0
        _Char76 ("_Char76", float)      = 0
        _Char77 ("_Char77", float)      = 0
        _Char78 ("_Char78", float)      = 0
        _Char79 ("_Char79", float)      = 0
        _Char80 ("_Char80", float)      = 0
        _Char81 ("_Char81", float)      = 0
        _Char82 ("_Char82", float)      = 0
        _Char83 ("_Char83", float)      = 0
        _Char84 ("_Char84", float)      = 0
        _Char85 ("_Char85", float)      = 0
        _Char86 ("_Char86", float)      = 0
        _Char87 ("_Char87", float)      = 0
        _Char88 ("_Char88", float)      = 0
        _Char89 ("_Char89", float)      = 0
        _Char90 ("_Char90", float)      = 0
        _Char91 ("_Char91", float)      = 0
        _Char92 ("_Char92", float)      = 0
        _Char93 ("_Char93", float)      = 0
        _Char94 ("_Char94", float)      = 0
        _Char95 ("_Char95", float)      = 0
        _Char96 ("_Char96", float)      = 0
        _Char97 ("_Char97", float)      = 0
        _Char98 ("_Char98", float)      = 0
        _Char99 ("_Char99", float)      = 0
        _Char100 ("_Char100", float)    = 0
        _Char101 ("_Char101", float)    = 0
        _Char102 ("_Char102", float)    = 0
        _Char103 ("_Char103", float)    = 0
        _Char104 ("_Char104", float)    = 0
        _Char105 ("_Char105", float)    = 0
        _Char106 ("_Char106", float)    = 0
        _Char107 ("_Char107", float)    = 0
        _Char108 ("_Char108", float)    = 0
        _Char109 ("_Char109", float)    = 0
        _Char110 ("_Char110", float)    = 0
        _Char111 ("_Char111", float)    = 0
        _Char112 ("_Char112", float)    = 0
        _Char113 ("_Char113", float)    = 0
        _Char114 ("_Char114", float)    = 0
        _Char115 ("_Char115", float)    = 0
        _Char116 ("_Char116", float)    = 0
        _Char117 ("_Char117", float)    = 0
        _Char118 ("_Char118", float)    = 0
        _Char119 ("_Char119", float)    = 0
        _Char120 ("_Char120", float)    = 0
        _Char121 ("_Char121", float)    = 0
        _Char122 ("_Char122", float)    = 0
        _Char123 ("_Char123", float)    = 0
        _Char124 ("_Char124", float)    = 0
        _Char125 ("_Char125", float)    = 0
        _Char126 ("_Char126", float)    = 0
        _Char127 ("_Char127", float)    = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            float _Char0;
            float _Char1;
            float _Char2;
            float _Char3;
            float _Char4;
            float _Char5;
            float _Char6;
            float _Char7;
            float _Char8;
            float _Char9;
            float _Char10;
            float _Char11;
            float _Char12;
            float _Char13;
            float _Char14;
            float _Char15;
            float _Char16;
            float _Char17;
            float _Char18;
            float _Char19;
            float _Char20;
            float _Char21;
            float _Char22;
            float _Char23;
            float _Char24;
            float _Char25;
            float _Char26;
            float _Char27;
            float _Char28;
            float _Char29;
            float _Char30;
            float _Char31;
            float _Char32;
            float _Char33;
            float _Char34;
            float _Char35;
            float _Char36;
            float _Char37;
            float _Char38;
            float _Char39;
            float _Char40;
            float _Char41;
            float _Char42;
            float _Char43;
            float _Char44;
            float _Char45;
            float _Char46;
            float _Char47;
            float _Char48;
            float _Char49;
            float _Char50;
            float _Char51;
            float _Char52;
            float _Char53;
            float _Char54;
            float _Char55;
            float _Char56;
            float _Char57;
            float _Char58;
            float _Char59;
            float _Char60;
            float _Char61;
            float _Char62;
            float _Char63;
            float _Char64;
            float _Char65;
            float _Char66;
            float _Char67;
            float _Char68;
            float _Char69;
            float _Char70;
            float _Char71;
            float _Char72;
            float _Char73;
            float _Char74;
            float _Char75;
            float _Char76;
            float _Char77;
            float _Char78;
            float _Char79;
            float _Char80;
            float _Char81;
            float _Char82;
            float _Char83;
            float _Char84;
            float _Char85;
            float _Char86;
            float _Char87;
            float _Char88;
            float _Char89;
            float _Char90;
            float _Char91;
            float _Char92;
            float _Char93;
            float _Char94;
            float _Char95;
            float _Char96;
            float _Char97;
            float _Char98;
            float _Char99;
            float _Char100;
            float _Char101;
            float _Char102;
            float _Char103;
            float _Char104;
            float _Char105;
            float _Char106;
            float _Char107;
            float _Char108;
            float _Char109;
            float _Char110;
            float _Char111;
            float _Char112;
            float _Char113;
            float _Char114;
            float _Char115;
            float _Char116;
            float _Char117;
            float _Char118;
            float _Char119;
            float _Char120;
            float _Char121;
            float _Char122;
            float _Char123;
            float _Char124;
            float _Char125;
            float _Char126;
            float _Char127;

            int _SizeX;
            int _SizeY;
            int _FontX;
            int _FontY;
            int _Swap;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half2 text_uv = i.uv * half2(_SizeX, _SizeY);
                float chars[128];

                //return float4(frac(text_uv), 0, 1);

                if(true) // just want to hide this :(
                {
                    chars[0] = floor(_Char0);
                    chars[1] = floor(_Char1);
                    chars[2] = floor(_Char2);
                    chars[3] = floor(_Char3);
                    chars[4] = floor(_Char4);
                    chars[5] = floor(_Char5);
                    chars[6] = floor(_Char6);
                    chars[7] = floor(_Char7);
                    chars[8] = floor(_Char8);
                    chars[9] = floor(_Char9);
                    chars[10] = floor(_Char10);
                    chars[11] = floor(_Char11);
                    chars[12] = floor(_Char12);
                    chars[13] = floor(_Char13);
                    chars[14] = floor(_Char14);
                    chars[15] = floor(_Char15);
                    chars[16] = floor(_Char16);
                    chars[17] = floor(_Char17);
                    chars[18] = floor(_Char18);
                    chars[19] = floor(_Char19);
                    chars[20] = floor(_Char20);
                    chars[21] = floor(_Char21);
                    chars[22] = floor(_Char22);
                    chars[23] = floor(_Char23);
                    chars[24] = floor(_Char24);
                    chars[25] = floor(_Char25);
                    chars[26] = floor(_Char26);
                    chars[27] = floor(_Char27);
                    chars[28] = floor(_Char28);
                    chars[29] = floor(_Char29);
                    chars[30] = floor(_Char30);
                    chars[31] = floor(_Char31);
                    chars[32] = floor(_Char32);
                    chars[33] = floor(_Char33);
                    chars[34] = floor(_Char34);
                    chars[35] = floor(_Char35);
                    chars[36] = floor(_Char36);
                    chars[37] = floor(_Char37);
                    chars[38] = floor(_Char38);
                    chars[39] = floor(_Char39);
                    chars[40] = floor(_Char40);
                    chars[41] = floor(_Char41);
                    chars[42] = floor(_Char42);
                    chars[43] = floor(_Char43);
                    chars[44] = floor(_Char44);
                    chars[45] = floor(_Char45);
                    chars[46] = floor(_Char46);
                    chars[47] = floor(_Char47);
                    chars[48] = floor(_Char48);
                    chars[49] = floor(_Char49);
                    chars[50] = floor(_Char50);
                    chars[51] = floor(_Char51);
                    chars[52] = floor(_Char52);
                    chars[53] = floor(_Char53);
                    chars[54] = floor(_Char54);
                    chars[55] = floor(_Char55);
                    chars[56] = floor(_Char56);
                    chars[57] = floor(_Char57);
                    chars[58] = floor(_Char58);
                    chars[59] = floor(_Char59);
                    chars[60] = floor(_Char60);
                    chars[61] = floor(_Char61);
                    chars[62] = floor(_Char62);
                    chars[63] = floor(_Char63);
                    chars[64] = floor(_Char64);
                    chars[65] = floor(_Char65);
                    chars[66] = floor(_Char66);
                    chars[67] = floor(_Char67);
                    chars[68] = floor(_Char68);
                    chars[69] = floor(_Char69);
                    chars[70] = floor(_Char70);
                    chars[71] = floor(_Char71);
                    chars[72] = floor(_Char72);
                    chars[73] = floor(_Char73);
                    chars[74] = floor(_Char74);
                    chars[75] = floor(_Char75);
                    chars[76] = floor(_Char76);
                    chars[77] = floor(_Char77);
                    chars[78] = floor(_Char78);
                    chars[79] = floor(_Char79);
                    chars[80] = floor(_Char80);
                    chars[81] = floor(_Char81);
                    chars[82] = floor(_Char82);
                    chars[83] = floor(_Char83);
                    chars[84] = floor(_Char84);
                    chars[85] = floor(_Char85);
                    chars[86] = floor(_Char86);
                    chars[87] = floor(_Char87);
                    chars[88] = floor(_Char88);
                    chars[89] = floor(_Char89);
                    chars[90] = floor(_Char90);
                    chars[91] = floor(_Char91);
                    chars[92] = floor(_Char92);
                    chars[93] = floor(_Char93);
                    chars[94] = floor(_Char94);
                    chars[95] = floor(_Char95);
                    chars[96] = floor(_Char96);
                    chars[97] = floor(_Char97);
                    chars[98] = floor(_Char98);
                    chars[99] = floor(_Char99);
                    chars[100] = floor(_Char100);
                    chars[101] = floor(_Char101);
                    chars[102] = floor(_Char102);
                    chars[103] = floor(_Char103);
                    chars[104] = floor(_Char104);
                    chars[105] = floor(_Char105);
                    chars[106] = floor(_Char106);
                    chars[107] = floor(_Char107);
                    chars[108] = floor(_Char108);
                    chars[109] = floor(_Char109);
                    chars[110] = floor(_Char110);
                    chars[111] = floor(_Char111);
                    chars[112] = floor(_Char112);
                    chars[113] = floor(_Char113);
                    chars[114] = floor(_Char114);
                    chars[115] = floor(_Char115);
                    chars[116] = floor(_Char116);
                    chars[117] = floor(_Char117);
                    chars[118] = floor(_Char118);
                    chars[119] = floor(_Char119);
                    chars[120] = floor(_Char120);
                    chars[121] = floor(_Char121);
                    chars[122] = floor(_Char122);
                    chars[123] = floor(_Char123);
                    chars[124] = floor(_Char124);
                    chars[125] = floor(_Char125);
                    chars[126] = floor(_Char126);
                    chars[127] = floor(_Char127);
                }

                
                uint current_char = floor(text_uv.x) + floor(text_uv.y) * _SizeX;

                if ((bool)_Swap)
                {
                    current_char-=_SizeX;
                    if ((int)text_uv.y % 2 == 0)
                    {
                        current_char+=_SizeX*2;
                    }
                }
                
                //return float4(chars[current_char] / 16, 0, 0, 1);

                const float cur_char_val = current_char < 128 ? chars[current_char]+_FontX : _FontX; //sshh it works
                const half2 char_pos = half2(cur_char_val % _FontX, -floor(cur_char_val / _FontX));
                
                //return float4(frac(char_pos / 16), 0, 1);

                const half2 char_uv = frac(text_uv) / half2(_FontX, _FontY) + char_pos / half2(_FontX, _FontY);
                // sample the texture
                fixed4 col = tex2D(_MainTex, char_uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
