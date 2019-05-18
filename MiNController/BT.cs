using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using Java.Util;

namespace MiNController
{
    public class Bt
    {
        private BluetoothSocket _socket;
        private bool _initialized;
        private string _name;

        public Bt(string name)
        {
            _initialized = false;
            _name = name;
        }

        async Task Connect()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
            if (adapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!adapter.IsEnabled)
                throw new Exception("Bluetooth adapter is not enabled.");

            BluetoothDevice device = (from bd in adapter.BondedDevices
                                      where bd.Name == _name
                                      select bd).FirstOrDefault();

            if (device == null)
                throw new Exception("Named device not found.");
            // Get SPP
            _socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            await _socket.ConnectAsync();
            _initialized = true;
        }

        //async Task<byte[]> Read()
        //{
        //    // Read data from the device
        //    await _socket.InputStream.ReadAsync(buffer, 0, buffer.Length);
        //}

        async Task Write(string message)
        {
            if (!_initialized) return;
            var buffer = UTF8Encoding.UTF8.GetBytes(message);
            // Write data to the device
            await _socket.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}