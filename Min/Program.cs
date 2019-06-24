using System;
using System.Text;
using System.Threading;
using Bauland.Others;
using Bauland.Pins;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Uart;

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
        private static GpioPin _ledGreen, _ledBlue;
        private static Random _random;

        private static void Main()
        {

            Setup();
            try
            {


                _ledStrip.Clear();
                _ledStrip.Show();

                DoEver();
            }
            catch (Exception)
            {
                _ledBlue.Write(GpioPinValue.High);
            }
        }

        private static void Setup()
        {
            _readBuffer = new byte[30];
            _color = new Color(0, 127, 255);
            int size = 350;
            _ledStrip = new LedStrip(size, Electron11.SpiBus.Spi1);
            _random = new Random(DateTime.Now.Millisecond);
            _serial = UartController.FromName(Electron11.UartPort.Uart2);
            _serial.SetActiveSettings(9600, 8, UartParity.None, UartStopBitCount.One, UartHandshake.None);
            _serial.DataReceived += _serial_DataReceived;
            _serial.Enable();

            var ctl = GpioController.GetDefault();
            _ledGreen = ctl.OpenPin(Electron11.GpioPin.Led1);
            _ledGreen.SetDriveMode(GpioPinDriveMode.Output);
            _ledGreen.Write(GpioPinValue.High);

            _ledBlue = ctl.OpenPin(Electron11.GpioPin.Led2);
            _ledBlue.SetDriveMode(GpioPinDriveMode.Output);
            _ledBlue.Write(GpioPinValue.Low);

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
            var param = str.Split('$');
            foreach (var p in param)
            {
                if (!string.IsNullOrEmpty(p))
                {
                    var setting = p.Trim('\r', '\n');
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

        private static void DoEver()
        {
            byte brightness = LedStrip.IntensityMax / 4;
            while (true)
            {
                var color = _color;
                Animate(brightness, color);
            }
        }

        private static void Animate(byte brightness, Color color)
        {
            IAnimation animation = ChooseAnimation(brightness, color);
            animation.Do();
        }

        private static IAnimation ChooseAnimation(byte brightness, Color color)
        {
            var nb = _random.Next(4);
            switch (nb)
            {
                case 0:
                    return new Animation1(brightness, color, _ledStrip);
                case 2:
                    return new Animation2(brightness, color, _ledStrip);
                default:
                    return new AnimationNothing(_random);
            }
        }
    }
}
