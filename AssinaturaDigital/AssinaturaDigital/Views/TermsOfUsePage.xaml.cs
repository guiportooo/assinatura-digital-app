using AssinaturaDigital.Events;
using Prism.Events;
using Xamarin.Forms;

namespace AssinaturaDigital.Views
{
    public partial class TermsOfUsePage : ContentPage
    {
        private readonly IEventAggregator _eventAggregator;

        public TermsOfUsePage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
        }

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            var scrollView = sender as ScrollView;
            var scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            if (scrollingSpace <= e.ScrollY)
                _eventAggregator.GetEvent<ScrolledToBottomEvent>().Publish();
        }
    }
}
