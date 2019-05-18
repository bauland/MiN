using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;
using GHIElectronics.TinyCLR.Devices.Uart;
using GHIElectronics.TinyCLR.Pins;

namespace SetBluetoothModule
{
    class Program
    {
        private static UartController _serial;
        private static byte[] _readBuffer;

        static void Main()
        {
            try
            {
                Setup();
                Check();
                Reset();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            while (true)
            {
                Check();
                Thread.Sleep(2000);
            }
        }

        private static void Reset()
        {
            var buffer = Encoding.UTF8.GetBytes("AT+NAMEHC-06");
            _serial.Write(buffer);
            Thread.Sleep(1000);
            if (_serial.BytesToRead > 0)
            {
                var bytesReceived = _serial.Read(_readBuffer, 0, _serial.BytesToRead);
                string str = Encoding.UTF8.GetString(_readBuffer, 0, bytesReceived);
                if (!string.IsNullOrEmpty(str))
                    Debug.WriteLine(str);
            }

        }

        private static void Check()
        {
            var buffer = Encoding.UTF8.GetBytes("AT");
            _serial.Write(buffer);
            Thread.Sleep(1000);
            if (_serial.BytesToRead > 0)
            {
                var bytesReceived = _serial.Read(_readBuffer, 0, _serial.BytesToRead);
                string str = Encoding.UTF8.GetString(_readBuffer, 0, bytesReceived);
                if (!string.IsNullOrEmpty(str))
                    Debug.WriteLine(str);
            }
        }

        private static void Setup()
        {
            _readBuffer = new byte[40];
            _serial = UartController.FromName(BrainPad.Expansion.UartPort.Usart1);
            _serial.SetActiveSettings(9600, 8, UartParity.None, UartStopBitCount.One, UartHandshake.None);
            // _serial.DataReceived += _serial_DataReceived;
            _serial.ClearReadBuffer();
            _serial.ClearWriteBuffer();
            _serial.Enable();
        }

        private static void _serial_DataReceived(UartController sender, DataReceivedEventArgs e)
        {
            if (_serial.BytesToRead > 0)
            {
                var bytesReceived = _serial.Read(_readBuffer, 0, _serial.BytesToRead);
                string str = Encoding.UTF8.GetString(_readBuffer, 0, bytesReceived);
                if (!string.IsNullOrEmpty(str))
                    Debug.WriteLine(str);
            }
        }
    }
}
