using Prism.Mvvm;
namespace AssinaturaDigital.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
    }
}
