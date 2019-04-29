using AssinaturaDigital.Validations.Renderers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace AssinaturaDigital.Validations.Behaviors
{
    public class ValidationBehavior : Behavior<View>
    {
        IErrorStyle _style = new BasicErrorStyle();
        View _view;
        public string PropertyName { get; set; }
        public ObservableCollection<IValidator> Validators { get; set; } = new ObservableCollection<IValidator>();

        public bool Validate()
        {
            bool isValid = true;
            string errorMessage = "";

            foreach (var validator in Validators)
            {
                bool result = validator.Check(_view.GetType()
                                       .GetProperty(PropertyName)
                                       .GetValue(_view) as string);
                isValid = isValid && result;

                if (!result)
                {
                    errorMessage = validator.Message;
                    break;
                }
            }

            if (!isValid)
            {
                _style.ShowError(_view, errorMessage);
                return false;
            }
            else
            {
                _style.RemoveError(_view);
                return true;
            }

        }

        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);

            _view = bindable as View;
            _view.PropertyChanged += OnPropertyChanged;
            _view.Unfocused += OnUnFocused;
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);

            _view.PropertyChanged -= OnPropertyChanged;
            _view.Unfocused -= OnUnFocused;
        }
        void OnUnFocused(object sender, FocusEventArgs e) =>
            Validate();

        void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropertyName)
                Validate();
        }
    }
}
