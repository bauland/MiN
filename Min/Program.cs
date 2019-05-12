﻿using System;
using System.Threading;
using Bauland.Others;
using Bauland.Pins;
using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Pins;
// ReSharper disable FunctionNeverReturns
// ReSharper disable StringLiteralTypo

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
                    _ledStrip = new LedStrip(8, Netduino3.SpiBus.Spi2, LedStrip.ColorOrder.Bgr);
                    break;
                case "FEZCLR":
                    _ledStrip = new LedStrip(8, FEZ.SpiBus.Spi1, LedStrip.ColorOrder.Bgr);
                    break;
                case "Electron":
                    _ledStrip = new LedStrip(8, Electron11.SpiBus.Spi1, LedStrip.ColorOrder.Bgr);
                    break;
                case "BrainpadBP2":
                    _ledStrip = new LedStrip(8, BrainPadBP2.SpiBus.Spi2, LedStrip.ColorOrder.Bgr);
                    break;
                default:
                    throw new ApplicationException($"Carte non supportée: {DeviceInformation.DeviceName}");
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
