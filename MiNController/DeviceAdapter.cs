using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace MiNController
{
    public class DeviceAdapter:ArrayAdapter<Device>
    {

        public DeviceAdapter(Context context, Device[] devices):base(context,0,devices)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Device device = GetItem(position);
            if (convertView == null)
            {
                convertView = LayoutInflater.From(Context).Inflate(Resource.Layout.item_device, parent, false);
            }

            TextView tvName = convertView.FindViewById<TextView>(Resource.Id.tvName);
            TextView tvAddress = convertView.FindViewById<TextView>(Resource.Id.tvAddress);
            ImageView ivColor = convertView.FindViewById<ImageView>(Resource.Id.color);

            tvName.Text = device.Name;
            tvAddress.Text = device.Address;
            if(device.IsSelectable) ivColor.SetImageDrawable(new ColorDrawable(Color.Green));

            return convertView;
        }
    }
}