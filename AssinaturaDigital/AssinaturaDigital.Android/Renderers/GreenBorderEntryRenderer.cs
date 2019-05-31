using Android.Content;
using Android.Views;
using AssinaturaDigital.Droid.Renderers;
using AssinaturaDigital.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GreenBorderEntry), typeof(GreenBorderEntryRenderer))]
namespace AssinaturaDigital.Droid.Renderers
{
    public class GreenBorderEntryRenderer : EntryRenderer
    {
        public GreenBorderEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = Android.App.Application.Context.GetDrawable(Resource.Drawable.green_border_entry);
                Control.Gravity = GravityFlags.Center;
                Control.SetPadding(10, 0, 0, 0);
            }
        }
    }
}
