using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;
using GHIElectronics.TinyCLR.Devices.Uart;
using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Pins;

namespace testUART
{
    class Program
    {
        private static UartController _serial;
        private static byte[] _readBuffer;
        static void Main()
        {
            try
            {
                _readBuffer = new byte[40];
                _serial = UartController.FromName(BrainPad.Expansion.UartPort.Usart1);
                _serial.SetActiveSettings(9600, 8, UartParity.None, UartStopBitCount.One, UartHandshake.None);
                _serial.DataReceived += Serial_DataReceived;
                _serial.Enable();

            DisplayMemory();
                while (true)
                    Thread.Sleep(20);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void DisplayMemory()
        {
            Debug.WriteLine($"Memory free: {Memory.FreeBytes}/{Memory.FreeBytes + Memory.UsedBytes}");
        }

        private static void Serial_DataReceived(UartController sender, DataReceivedEventArgs e)
        {
            var bytesReceived = _serial.Read(_readBuffer, 0, e.Count);
            var str = Encoding.UTF8.GetString(_readBuffer, 0, bytesReceived);

            SendAck();
        }

        private static void SendAck()
        {
            var msg = "OK";
            var bytes = Encoding.UTF8.GetBytes(msg);
            _serial.Write(bytes);
        }
    }
}
