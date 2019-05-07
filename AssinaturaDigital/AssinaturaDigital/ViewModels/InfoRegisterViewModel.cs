using AssinaturaDigital.Services.Selfies;
using AssinaturaDigital.Utilities;
using AssinaturaDigital.Views;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials.Interfaces;

namespace AssinaturaDigital.ViewModels
{
    public class InfoRegisterViewModel : ViewModelBase, INavigatingAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly ISelfiesService _selfiesService;
        private readonly IPreferences _preferences;

        public DelegateCommand NavigateToHomeCommand { get; }
        public DelegateCommand LogoutCommand { get; }

        private bool _approved;
        public bool Approved
        {
            get => _approved;
            set => SetProperty(ref _approved, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public InfoRegisterViewModel(INavigationService navigationService,
            IPageDialogService pageDialogService,
            ISelfiesService selfiesService,
            IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _selfiesService = selfiesService;
            _preferences = preferences;

            HasFowardNavigation = false;

            NavigateToHomeCommand = new DelegateCommand(NavigateToHome, CanNavigate)
                .ObservesProperty(() => IsBusy);
            LogoutCommand = new DelegateCommand(Logout, CanNavigate)
                .ObservesProperty(() => IsBusy);
        }

        public async void OnNavigatingTo(INavigationParameters parameters)
        {
            if (!ParametersAreValid(parameters))
            {
                GoBack();
                return;
            }

            var idUser = _preferences.Get(AppConstants.IdUser, 0);

            if (idUser == 0)
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Usuário inválido!", "OK");
                GoBack();
                return;
            }

            var photo = parameters.GetValue<MediaFile>(AppConstants.Selfie);
            await Initialize(idUser, photo);
        }

        bool ParametersAreValid(INavigationParameters parameters)
            => parameters != null && parameters[AppConstants.Selfie] != null;

        async Task Initialize(int idUser, MediaFile photo)
        {
            try
            {
                IsBusy = true;
                Approved = await _selfiesService.SaveSelfie(idUser, photo);

                if (Approved)
                {
                    Title = "Quase pronto para assinar!";
                    Message = "Estamos em processo de análise de dados e em breve, seus contratos estarão disponíveis. Você receberá uma notificação de aviso.";
                }
                else
                {
                    Title = "Imagem inválida!";
                    Message = "Você será redirecionado para uma nova selfie.";
                }
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao validar selfie.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        bool CanNavigate() => !IsBusy;

        async void NavigateToHome()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(HomePage));
            }
            catch (Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync(Title, ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async void Logout()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(MainPage));
            }
            catch (Exception ex)
            {
                await _pageDialogService.DisplayAlertAsync(Title, ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
