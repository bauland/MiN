﻿using System;
using System.Runtime.InteropServices;

namespace Min
{
    public static class Utility
    {
        public static void Patch()
        {
            // BrainPad, FEZCLR
            const int MODE_ADDRESS = 0x20001438;
            const int EVENT_ADDRESS = 0x20001ef8;
            const int OFFSET = 0;

            // G30
            //const int MODE_ADDRESS = 0x20001a68;
            //const int EVENT_ADDRESS = 0x20002348;
            //const int OFFSET = 0;

            // G120
            //const int MODE_ADDRESS = 0x200446F9; 
            //const int EVENT_ADDRESS = 0x20044FD9; 
            //const int OFFSET = int.MaxValue;

            // G400
            //const int MODE_ADDRESS = 0x2060bd10;
            //const int EVENT_ADDRESS = 0x2060c5f0;
            //const int OFFSET = 0;

            var isUsbDebugMode = Marshal.ReadInt32(IntPtr.Add(new IntPtr(OFFSET), MODE_ADDRESS)) != 0 ? true : false;
            var value = Marshal.ReadInt32(IntPtr.Add(new IntPtr(OFFSET), (EVENT_ADDRESS))) | (isUsbDebugMode ? 512 : 256);
            Marshal.WriteInt32(IntPtr.Add(new IntPtr(OFFSET), EVENT_ADDRESS), value);
        }
    }
}
