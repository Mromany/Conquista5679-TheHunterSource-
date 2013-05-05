using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace PhoenixProject.Game
{
    public class Native
    {
        public Native()
        {
            //Class0.N47LJ78z09Kgf();
        }

        [DllImport("Library.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BF_cfb64_encrypt(byte[] in_, byte[] out_, int length, IntPtr schedule, byte[] ivec, ref int num, int enc);
        [DllImport("Library.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void BF_set_key(IntPtr _key, int len, byte[] data);
        [DllImport("Library.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CAST_cfb64_encrypt(byte[] in_, byte[] out_, int length, IntPtr schedule, byte[] ivec, ref int num, int enc);
        [DllImport("Library.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CAST_set_key(IntPtr _key, int len, byte[] data);
        [DllImport("KERNEL32.DLL")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);
        [DllImport("KERNEL32.DLL", SetLastError = true)]
        public static extern void GetSystemTime(ref SystemTime sysTime);
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void* memcpy(void* dest, void* src, uint size);
        [DllImport("KERNEL32.DLL")]
        public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, int wAttributes);
        [DllImport("winmm.dll")]
        public static extern uint timeGetTime();
        [DllImport("winmm.dll", EntryPoint = "timeGetTime")]
        public static extern uint timeGetTime_1();

        [StructLayout(LayoutKind.Sequential)]
        public struct SystemTime
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Millisecond;
        }
    }
}
