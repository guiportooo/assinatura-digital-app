using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AssinaturaDigital.Validations.Renderers
{
    public class BasicErrorStyle : IErrorStyle
    {
        public void ShowError(View view, string message)
        {
            var layout = view.Parent as StackLayout;
            int viewIndex = layout.Children.IndexOf(view);

            if (viewIndex + 1 < layout.Children.Count)
            {
                var sibling = layout.Children[viewIndex + 1];
                string siblingStyleId = view.Id.ToString();
                if (sibling.StyleId == siblingStyleId)
                {
                    var errorLabel = sibling as Label;
                    errorLabel.Text = message;
                    errorLabel.IsVisible = true;

                    return;
                }
            }

            view.Margin = 0;

            layout.Children.Insert(viewIndex + 1, new Label
            {
                Text = message,
                FontSize = 10,
                StyleId = view.Id.ToString(),
                TextColor = Color.FromHex("#DAA520"),
                Margin = new Thickness(0,0,0,10)
            });
        }

        public void RemoveError(View view)
        {
            var layout = view.Parent as StackLayout;
            int viewIndex = layout.Children.IndexOf(view);

            if (viewIndex + 1 < layout.Children.Count)
            {
                var sibling = layout.Children[viewIndex + 1];
                string siblingStyleId = view.Id.ToString();

                if (sibling.StyleId == siblingStyleId)
                {
                    sibling.IsVisible = false;
                    view.Margin = new Thickness(0, 0, 0, 10);
                }
            }
        }
    }
}

