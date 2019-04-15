using Prism.Commands;

namespace AssinaturaDigital.UnitTests.Mocks
{
    public class DelegateCommandMock : DelegateCommand
    {
        public bool CanExecuteChangeRaised { get; private set; }

        public DelegateCommandMock() : base(() => { })
            => CanExecuteChanged += (sender, e) => CanExecuteChangeRaised = true;
    }
}
