using AssinaturaDigital.Services.Validations;
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
        private readonly IValidationsService _validationsService;
        private readonly IPreferences _preferences;

        public DelegateCommand NavigateToHomeCommand { get; }
        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand NavigateToDocumentsPageCommand { get; }

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
            IValidationsService validationsService,
            IPreferences preferences) : base(navigationService, pageDialogService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _validationsService = validationsService;
            _preferences = preferences;

            HasBackNavigation = false;
            HasFowardNavigation = false;

            NavigateToHomeCommand = new DelegateCommand(NavigateToHome, CanNavigate)
                .ObservesProperty(() => IsBusy);
            LogoutCommand = new DelegateCommand(Logout, CanNavigate)
                .ObservesProperty(() => IsBusy);
            NavigateToDocumentsPageCommand = new DelegateCommand(NavigateToDocumentsPage, CanNavigate)
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

            var video = parameters.GetValue<MediaFile>(AppConstants.Video);
            await Initialize(idUser, video);
        }

        bool ParametersAreValid(INavigationParameters parameters)
            => parameters != null && parameters[AppConstants.Video] != null;

        async Task Initialize(int idUser, MediaFile video)
        {
            try
            {
                IsBusy = true;
                Approved = await _validationsService.ValidateUser(idUser, video);

                if (Approved)
                {
                    Title = "Quase pronto para assinar!";
                    Message = "Estamos em processo de análise de dados e em breve, seus contratos estarão disponíveis. Você receberá uma notificação de aviso.";
                }
                else
                {
                    Title = "Cadastro não finalizado!";
                    Message = "Seu cadastro não pôde ser finalizado por divergências de verificação nas imagens e vídeo enviados. O problema pode ter ocorrido:\n\n- Nas imagens do documento (RG ou CNH)\n\n- No vídeo\n\nPor favor, repita o processo de envio de imagens e vídeo para que uma nova validação seja realizada.";
                }
            }
            catch
            {
                await _pageDialogService.DisplayAlertAsync(Title, "Falha ao validar vídeo.", "OK");
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

        async void NavigateToDocumentsPage()
        {
            try
            {
                IsBusy = true;
                await _navigationService.NavigateAsync(nameof(DocumentsSelectionPage));
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
