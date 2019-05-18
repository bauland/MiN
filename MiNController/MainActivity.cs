using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace MiNController
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        private ListView _listView;
        private Device[] _devices;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            CreatePseudoDevices();
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            _listView = FindViewById<ListView>(Resource.Id.listView1);
            FillListView();
            navigation.SetOnNavigationItemSelectedListener(this);
        }

        private void FillListView()
        {
            DeviceAdapter adapter=new DeviceAdapter(this,_devices);

            _listView.Adapter = adapter;
            _listView.ItemClick += _listView_ItemClick;
        }

        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var adapter=(DeviceAdapter)_listView.Adapter;
            var item=adapter.GetItem(e.Position);
        }

        private void CreatePseudoDevices()
        {
            _devices = new[]
            {
                new Device()
                {
                    Name = "Bluetooth disable",
                    IsSelectable = false
                },
                new Device()
                {
                    IsSelectable = true,
                    Name = "Test",
                    Address = "14:41:14"
                }
            };
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
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_settings:
                    textMessage.SetText(Resource.String.title_settings);
                    return true;
            }
            return false;
        }
    }
}

