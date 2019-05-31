using AssinaturaDigital.iOS.Renderers;
using AssinaturaDigital.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GreenBorderEntry), typeof(GreenBorderEntryRenderer))]
namespace AssinaturaDigital.iOS.Renderers
{
    public class GreenBorderEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.BorderColor = UIColor.FromRGB(139, 240, 129).CGColor;
                Control.Layer.BorderWidth = 1;
                Control.Layer.CornerRadius = 4;
                Control.TextAlignment = UITextAlignment.Center;
            }
        }
    }
}
