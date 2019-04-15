using AssinaturaDigital.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AssinaturaDigital.Models
{
    public class NotifyCommandCollectionModel<NotifyCommandModel> : BindableBase
    {
        private ObservableCollection<NotifyCommandModel> _items;
        public ObservableCollection<NotifyCommandModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public NotifyCommandCollectionModel(DelegateCommand commandToNotify, IEnumerable<NotifyCommandModel> items)
            => _items = items
            .ToList()
            .Select(x =>
            {
                ((INotifyCommandModel)x).SetCommandToNotify(commandToNotify);
                return x;
            })
            .ToObservableCollection();
    }
}
