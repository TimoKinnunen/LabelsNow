using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace LabelsNow.Views
{
    public sealed partial class AboutPage : Page
    {
        private MainPage mainPage;

        public AboutPage()
        {
            InitializeComponent();

            SizeChanged += Page_SizeChanged;
            Loaded += Page_Loaded;

            mainPage = MainPage.CurrentMainPage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetPageContentStackPanelWidth();
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetPageContentStackPanelWidth();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // code here
            var myPackage = Windows.ApplicationModel.Package.Current;

            LabelsNowImage.Source = new BitmapImage(myPackage.Logo);

            AppDisplayNameTextBlock.Text = myPackage.DisplayName;

            PublisherTextBlock.Text = myPackage.PublisherDisplayName;

            var version = myPackage.Id.Version;
            VersionTextBlock.Text = $"Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            // code here
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // code here
            // code here
        }

        private void SetPageContentStackPanelWidth()
        {
            //PageContentStackPanel.Width =
            //    Math.Min(MyConstants.DefaultWidth, ActualWidth -
            //    PageContentScrollViewer.Margin.Left -
            //    PageContentScrollViewer.Padding.Right);
            PageContentStackPanel.Width = ActualWidth -
                PageContentScrollViewer.Margin.Left -
                PageContentScrollViewer.Padding.Right;
        }

        #region MenuAppBarButton
        private void HomeAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            mainPage.GoToHomePage();
            mainPage.MenuNavigationListView.SelectedIndex = 0;
        }
        #endregion MenuAppBarButton
    }
}