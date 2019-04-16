namespace AssinaturaDigital.Models
{
    public class TokenDigit : NotifyCommandModel
    {
        private string _digit;
        public string Digit
        {
            get => _digit;
            set
            {
                SetProperty(ref _digit, value);
                _commandToNotify?.RaiseCanExecuteChanged();
            }
        }

        public TokenDigit(string digit) => _digit = digit;
    }
}
