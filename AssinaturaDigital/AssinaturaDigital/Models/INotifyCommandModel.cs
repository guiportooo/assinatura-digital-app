using Prism.Commands;

namespace AssinaturaDigital.Models
{
    public interface INotifyCommandModel
    {
        void SetCommandToNotify(DelegateCommand commandToNotify);
    }
}
