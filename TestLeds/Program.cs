using System.Threading;
using Bauland.Others;
using GHIElectronics.TinyCLR.Pins;

// ReSharper disable FunctionNeverReturns

namespace TestLeds
{
    static class Program
    {
        private static LedStrip _ledStrip;
        static void Main()
        {
            //Patch();
            // Display memory information
            // Debug.WriteLine($"Memory free: {Memory.FreeBytes}/{Memory.UsedBytes + Memory.FreeBytes}");
            _ledStrip = new LedStrip(350, STM32F4.SpiBus.Spi1);
            // Display memory information
            // Debug.WriteLine($"Memory free: {Memory.FreeBytes}/{Memory.UsedBytes + Memory.FreeBytes}");

            _ledStrip.Clear();
            _ledStrip.Show();

            byte brightness = LedStrip.IntensityMax / 8;
            while (true)
            {
                _ledStrip.SetAll(brightness, 255, 255, 255);
                _ledStrip.Show();
                Thread.Sleep(2000);
                _ledStrip.SetAll(brightness, 0, 0, 255);
                _ledStrip.Show();
                Thread.Sleep(2000);
                _ledStrip.SetAll(brightness, 255, 0, 0);
                _ledStrip.Show();
                Thread.Sleep(2000);
                _ledStrip.SetAll(brightness, 0, 255, 0);
                _ledStrip.Show();
                Thread.Sleep(2000);
                _ledStrip.Clear();
                _ledStrip.Show();
                Thread.Sleep(2000);
            }
        }

        //private static void Patch()
        //{
        //    // BrainPad, FEZCLR
        //    const int MODE_ADDRESS = 0x20001438;
        //    const int EVENT_ADDRESS = 0x20001ef8;
        //    const int OFFSET = 0;

        //    // G30
        //    //const int MODE_ADDRESS = 0x20001a68;
        //    //const int EVENT_ADDRESS = 0x20002348;
        //    //const int OFFSET = 0;

        //    // G120
        //    //const int MODE_ADDRESS = 0x200446F9; 
        //    //const int EVENT_ADDRESS = 0x20044FD9; 
        //    //const int OFFSET = int.MaxValue;

        //    // G400
        //    //const int MODE_ADDRESS = 0x2060bd10;
        //    //const int EVENT_ADDRESS = 0x2060c5f0;
        //    //const int OFFSET = 0;

        //    var isUsbDebugMode = Marshal.ReadInt32(IntPtr.Add(new IntPtr(OFFSET), MODE_ADDRESS)) != 0 ? true : false;
        //    var value = Marshal.ReadInt32(IntPtr.Add(new IntPtr(OFFSET), (EVENT_ADDRESS))) | (isUsbDebugMode ? 512 : 256);
        //    Marshal.WriteInt32(IntPtr.Add(new IntPtr(OFFSET), EVENT_ADDRESS), value);
        //}
    }
}
