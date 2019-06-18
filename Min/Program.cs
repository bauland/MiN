using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Bauland.Others;
using Bauland.Pins;
using GHIElectronics.TinyCLR.Devices.Uart;
using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Pins;
// ReSharper disable FunctionNeverReturns
// ReSharper disable StringLiteralTypo
// ReSharper disable MemberCanBePrivate.Global

namespace Min
{
    internal static class Program
    {
        private static byte[] _readBuffer;
        private static LedStrip _ledStrip;
        private static UartController _serial;
        private static Color _color;
        private static void Main()
        {

            Setup();
            _ledStrip.Clear();
            _ledStrip.Show();

            DoEverNothing();
        }

        private static void Setup()
        {
            _readBuffer = new byte[30];
            _color = new Color(0, 127, 255);
            int size = 300;
            _ledStrip = new LedStrip(size, Electron11.SpiBus.Spi1);
            _serial = UartController.FromName(Electron11.UartPort.Uart2);
            _serial.SetActiveSettings(9600, 8, UartParity.None, UartStopBitCount.One, UartHandshake.None);
            _serial.DataReceived += _serial_DataReceived;
            _serial.Enable();

            // Display memory information
            //Debug.WriteLine($"Memory free: {Memory.FreeBytes}/{Memory.UsedBytes + Memory.FreeBytes}");
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
                    ProcessData(str);
                }
            }
        }

        private static void ProcessData(string str)
        {
            var param = str.Split(new[] { '$' });
            foreach (var p in param)
            {
                if (!string.IsNullOrEmpty(p))
                {
                    var setting = p.Trim(new[] { '\r', '\n' });
                    var idx = setting.Split(':');
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
                _ledStrip.SetAll(LedStrip.IntensityMax / 8, _color.Red, _color.Green, _color.Blue);
                _ledStrip.Show();
                Thread.Sleep(20);
            }
        }
    }
}
