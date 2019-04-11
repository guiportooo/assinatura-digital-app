using Prism.Services;
using System;
using System.Threading.Tasks;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class PageDialogServiceMock : IPageDialogService
    {
        public string Title { get; private set; }
        public string Message { get; private set; }
        public string CancelButton { get; private set; }

        public Task DisplayAlertAsync(string title, string message, string cancelButton)
        {
            Title = title;
            Message = message;
            CancelButton = cancelButton;
            return Task.FromResult(true);
        }

        public Task<bool> DisplayAlertAsync(string title, string message, string acceptButton, string cancelButton)
        {
            throw new NotImplementedException();
        }

        public Task<string> DisplayActionSheetAsync(string title, string cancelButton, string destroyButton, params string[] otherButtons)
        {
            throw new NotImplementedException();
        }

        public Task DisplayActionSheetAsync(string title, params IActionSheetButton[] buttons)
        {
            throw new NotImplementedException();
        }
    }
}
