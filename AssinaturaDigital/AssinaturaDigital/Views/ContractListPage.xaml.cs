using AssinaturaDigital.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AssinaturaDigital.Views
{
    public partial class ContractListPage : ContentPage
    {
        public ContractListPage()
        {
            InitializeComponent();
        }

        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            var isToogled = (bool)e.Value;
            ((ContractListViewModel)BindingContext).FiltreContracts(!isToogled);
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            ((ContractListViewModel)BindingContext).ReloadContractList(e.NewTextValue);
        }
    }
}
