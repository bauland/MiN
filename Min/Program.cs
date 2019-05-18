using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Bauland.Others;
using Bauland.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Spi;
using GHIElectronics.TinyCLR.Devices.Uart;
using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Pins;
// ReSharper disable FunctionNeverReturns
// ReSharper disable StringLiteralTypo

namespace Min
{
    class Color
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public Color(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public Color() : this(0, 0, 0) { }
    }

    internal static class Program
    {
        private static byte[] _readBuffer;
        private static LedStrip _ledStrip;
        private static UartController _serial;
        private static Color _color;
        private static void Main()
        {

            Setup();
            _ledStrip.SetAll(1, 63, 63, 0);
            _ledStrip.Show();

            DoEverNothing();
        }

        private static void Setup()
        {
            _color = new Color();
            _readBuffer = new byte[20];
            int size = 8;
            switch (DeviceInformation.DeviceName)
            {
                case "netduino":
                    _ledStrip = new LedStrip(size, Netduino3.SpiBus.Spi2, LedStrip.ColorOrder.Bgr);
                    break;
                case "FEZCLR":
                    _ledStrip = new LedStrip(size, FEZ.SpiBus.Spi1, LedStrip.ColorOrder.Bgr);
                    break;
                case "Electron":
                    _ledStrip = new LedStrip(size, Electron11.SpiBus.Spi1, LedStrip.ColorOrder.Bgr);
                    break;
                case "BrainPadBP2":
                    _ledStrip = new LedStrip(size, BrainPad.Expansion.SpiBus.Spi1, LedStrip.ColorOrder.Bgr);
                    _serial = UartController.FromName(BrainPad.Expansion.UartPort.Usart1);
                    break;
                default:
                    throw new ApplicationException($"Carte non supportée: {DeviceInformation.DeviceName}");
            }
            _serial.SetActiveSettings(9600, 8, UartParity.None, UartStopBitCount.One, UartHandshake.None);
            _serial.DataReceived += _serial_DataReceived;
            _serial.Enable();

            // Display memory information
            Debug.WriteLine($"Memory free: {Memory.FreeBytes}/{Memory.UsedBytes + Memory.FreeBytes}");
        }

        private static void _serial_DataReceived(UartController sender, DataReceivedEventArgs e)
        {
            Thread.Sleep(20);
            if (_serial.BytesToRead > 0)
            {
                var bytesReceived = _serial.Read(_readBuffer, 0, _serial.BytesToRead);
                string str = Encoding.UTF8.GetString(_readBuffer, 0, bytesReceived);
                if (!string.IsNullOrEmpty(str))
                {
                    Debug.WriteLine(str);
                    str = str.Trim(new[] { '\r', '\n' });
                    var idx = str.Split(':');
                    if (idx.Length == 2)
                    {
                        var variable = idx[0];
                        var val = idx[1];
                        if (byte.TryParse(val, out byte b))
                        {
                            Settings(variable, b);
                        }
                    }
                }
            }
        }

        private static void Settings(string variable, byte b)
        {
            switch (variable.ToLower())
            {
                case "red":
                    _color.Red = b;
                    break;
                case "green":
                    _color.Green = b;
                    break;
                case "blue":
                    _color.Blue = b;
                    break;
            }
        }

        private static void DoEverNothing()
        {
            while (true)
            {
                _ledStrip.SetAll(LedStrip.IntensityMax,_color.Red,_color.Green,_color.Blue);
                _ledStrip.Show();
                Thread.Sleep(20);
            }
        }
    }
}
