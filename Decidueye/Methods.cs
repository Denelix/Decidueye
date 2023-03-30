using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Decidueye
{
    public class Methods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetAsyncKeyState(int keyCode);
        public static bool IsKeyDown(int keyCode)
        {
            return (GetAsyncKeyState(keyCode) & 0x8000) != 0;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public int type;
            public InputUnion u;
            public static int Size => Marshal.SizeOf(typeof(INPUT));
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mouseInput;
            [FieldOffset(0)]
            public KEYBDINPUT keyboardInput;
            [FieldOffset(0)]
            public HARDWAREINPUT hardwareInput;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        public static void SimulateKeyDown(int keyCode)
        {
            INPUT[] inputs = new INPUT[]
            {
        new INPUT
        {
            type = 1, // INPUT_KEYBOARD
            u = new InputUnion
            {
                keyboardInput = new KEYBDINPUT
                {
                    wVk = (ushort)keyCode,
                    dwFlags = 0 // 0 for key down
                }
            }
        }
            };
            SendInput((uint)inputs.Length, inputs, INPUT.Size);
        }

        public static void SimulateKeyUp(int keyCode)
        {
            INPUT[] inputs = new INPUT[]
            {
        new INPUT
        {
            type = 1, // INPUT_KEYBOARD
            u = new InputUnion
            {
                keyboardInput = new KEYBDINPUT
                {
                    wVk = (ushort)keyCode,
                    dwFlags = 2 // KEYEVENTF_KEYUP
                }
            }
        }
            };
            SendInput((uint)inputs.Length, inputs, INPUT.Size);
        }

    }
}
