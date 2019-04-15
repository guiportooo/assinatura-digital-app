using Prism.Commands;
using Prism.Mvvm;

namespace AssinaturaDigital.Models
{
    public abstract class NotifyCommandModel : BindableBase, INotifyCommandModel
    {
        protected DelegateCommand _commandToNotify;

        public void SetCommandToNotify(DelegateCommand commandToNotify) => _commandToNotify = commandToNotify;
    }
}
