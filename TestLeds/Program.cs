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
            _ledStrip=new LedStrip(8,BrainPad.Expansion.SpiBus.Spi1,LedStrip.ColorOrder.Bgr);
            _ledStrip.Clear();
            _ledStrip.Show();

            while (true)
            {
                _ledStrip.SetAll(LedStrip.IntensityMax, 255, 255, 255);
                _ledStrip.Show();
                Thread.Sleep(2000);
                _ledStrip.SetAll(LedStrip.IntensityMax, 0, 0, 255);
                _ledStrip.Show();
                Thread.Sleep(2000);
                _ledStrip.Clear();
                _ledStrip.Show();
                Thread.Sleep(2000);
            }
        }
    }
}
