using System;
using System.Collections;
using System.Text;
using System.Threading;
using Bauland.Others;
using Bauland.Pins;
using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Pins;

namespace Min
{
    internal static class Program
    {
        private static LedStrip _ledStrip;
        private static void Main()
        {

            Setup();
            _ledStrip.SetAll(1, 63, 63, 0);
            _ledStrip.Show();

            DoEverNothing();

        }

        private static void Setup()
        {
            switch (DeviceInformation.DeviceName)
            {
                case "netduino":
                    _ledStrip = new LedStrip(8, Netduino3.SpiBus.Spi2, Netduino3.GpioPin.D10, LedStrip.ColorOrder.Bgr);
                    break;
                case "FEZCLR":
                    _ledStrip = new LedStrip(8, FEZ.SpiBus.Spi1, FEZ.GpioPin.D10, LedStrip.ColorOrder.Bgr);
                    break;
                default:
                    DoEverNothing();
                    break;
            }
        }

        private static void DoEverNothing()
        {
            while (true)
            {
                Thread.Sleep(20);
            }
        }
    }
}
