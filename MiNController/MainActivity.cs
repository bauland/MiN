using System.Linq;
using Android.App;
using Android.Bluetooth;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util.Logging;
using Min;

namespace MiNController
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private Color _color;
        private ListView _listView;
        private Device[] _devices;
        private SeekBar _redSeekBar;
        private SeekBar _blueSeekBar;
        private SeekBar _greenSeekBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            _color=new Color();
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            _listView = FindViewById<ListView>(Resource.Id.listView1);
            _redSeekBar = FindViewById<SeekBar>(Resource.Id.seekBarRed);
            _greenSeekBar = FindViewById<SeekBar>(Resource.Id.seekBarGreen);
            _blueSeekBar = FindViewById<SeekBar>(Resource.Id.seekBarBlue);
            _redSeekBar.ProgressChanged += _redSeekBar_ProgressChanged;
            _greenSeekBar.ProgressChanged += _greenSeekBar_ProgressChanged;
            _blueSeekBar.ProgressChanged += _blueSeekBar_ProgressChanged;
            var button = FindViewById<Button>(Resource.Id.btn_send);
            button.Click += _button_Click;
            FindViewById<Button>(Resource.Id.btn_scan).Click += btnScan_Click;
            CreatePseudoDevices();
            FillListView();
            UpdateColor();
            navigation.SetOnNavigationItemSelectedListener(this);
        }

        private void btnScan_Click(object sender, System.EventArgs e)
        {
            CreatePseudoDevices();
            FillListView();
        }

        private void _blueSeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            Log.WriteLine(LogPriority.Info,"Data",$"Blue: {e.Progress}");
            _color.Blue = (byte) e.Progress;
            UpdateColor();
        }

        private void _greenSeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            Log.WriteLine(LogPriority.Info, "Data", $"Green: {e.Progress}");
            _color.Green = (byte)e.Progress;
            UpdateColor();
        }

        private void _redSeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            Log.WriteLine(LogPriority.Info, "Data", $"Red: {e.Progress}");
            _color.Red = (byte)e.Progress;
            UpdateColor();
        }

        private void UpdateColor()
        {
            ImageView iv = FindViewById<ImageView>(Resource.Id.ivColor);
            iv.SetImageDrawable(new ColorDrawable(new Android.Graphics.Color(_color.Red,_color.Blue,_color.Green)));
        }

        private void FillListView()
        {
            DeviceAdapter adapter=new DeviceAdapter(this,_devices);

            _listView.Adapter = adapter;
            _listView.ItemClick += _listView_ItemClick;
        }

        private void _button_Click(object sender, System.EventArgs e)
        {
            Log.WriteLine(LogPriority.Info, "Data",$"Color: R:{_color.Red},G:{_color.Green},B:{_color.Blue}");
        }

        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var adapter=(DeviceAdapter)_listView.Adapter;
            var item=adapter.GetItem(e.Position);
            if (!item.IsSelectable) return;
            Log.WriteLine(LogPriority.Info, "Data", $"Connecting to {item.Name}");

        }

        private void CreatePseudoDevices()
        {
            //_devices = new[]
            //{
            //    new Device()
            //    {
            //        Name = "Bluetooth disable",
            //        IsSelectable = false
            //    },
            //    new Device()
            //    {
            //        IsSelectable = true,
            //        Name = "Test",
            //        Address = "14:41:14"
            //    }
            //};

            var btAdapter = BluetoothAdapter.DefaultAdapter;
            if (!btAdapter.IsEnabled)
            {
                _devices = new[]
                {
                    new Device()
                    {
                        Name = "Bluetooth disable",
                        IsSelectable = false
                    }
                };
                return;
            }

            int n = btAdapter.BondedDevices.Count;
            Log.WriteLine(LogPriority.Info, "BT", n.ToString());
            _devices=new Device[n];
            int idx = 0;
            foreach(var bondedDevice in btAdapter.BondedDevices) { 
                _devices[idx]=new Device(){Name = bondedDevice.Name,IsSelectable = true,Address = bondedDevice.Address};
                idx++;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    return true;
                case Resource.Id.navigation_dashboard:
                    return true;
                case Resource.Id.navigation_settings:
                    return true;
            }
            return false;
        }
    }
}

