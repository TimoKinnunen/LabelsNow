using LabelsNow.Helpers;
using LabelsNow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace LabelsNow.Views
{
    public sealed partial class HomePage : Page
    {
        private MainPage mainPage;

        private PrintHelper printHelper;

        private bool pageIsLoaded;

        public static HomePage CurrentHomePage;

        private static List<LabelAddress> LabelAddressesInDatabase;

        public HomePage()
        {
            InitializeComponent();

            Loaded += Page_Loaded;

            mainPage = MainPage.CurrentMainPage;

            CurrentHomePage = this;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // code here
            #region things to do on OnNavigatedTo

            // need always the latest records from database
            LabelAddressesInDatabase = await App.LabelAddressesRepo.GetAllLabelAddressesAsync();

            App.NumberOfRecordsInDatabase = LabelAddressesInDatabase.Count;

            App.NumberOfLabelsPerPage = App.PaperNumberOfLabelRows * App.PaperNumberOfLabelColumns;

            App.NumberOfPages = App.NumberOfRecordsInDatabase == 0 ? 0 : App.NumberOfRecordsInDatabase / App.NumberOfLabelsPerPage + App.NumberOfRecordsInDatabase % App.NumberOfLabelsPerPage > 0 ? 1 : 0;

            NumberOfDatabaseAddressesTextBlock.Text = App.NumberOfPages < 2 ? $"Database contains {App.NumberOfRecordsInDatabase} addresses / {App.NumberOfLabelsPerPage} labels per page = {App.NumberOfPages} page." :
            $"Database contains {App.NumberOfRecordsInDatabase} addresses / {App.NumberOfLabelsPerPage} labels per page = {App.NumberOfPages} pages.";

            App.PaperForegroundSolidColorBrush = App.NamedColors.FirstOrDefault(n => n.ColorName == App.PaperFontColorName).SolidColorBrush;
            App.FontWeight = App.NamedFontWeights.FirstOrDefault(n => n.FontWeightName == App.PaperFontWeightName).FontWeight;
            App.FontFamily = App.NamedFontFamilies.FirstOrDefault(n => n.PaperFontFamilyName == App.PaperFontFamilyName).FontFamily;

            PaperNumberOfLabelRowsComboBox.Items.Clear();
            PaperNumberOfLabelRowsComboBox.ItemsSource = Enumerable.Range(1, 20);
            PaperNumberOfLabelRowsComboBox.SelectedValue = App.PaperNumberOfLabelRows;
            PaperNumberOfLabelRowsComboBox.Header = $"Number of label rows is {App.PaperNumberOfLabelRows}.";

            PaperNumberOfLabelColumnsComboBox.Items.Clear();
            PaperNumberOfLabelColumnsComboBox.ItemsSource = Enumerable.Range(1, 20);
            PaperNumberOfLabelColumnsComboBox.SelectedValue = App.PaperNumberOfLabelColumns;
            PaperNumberOfLabelColumnsComboBox.Header = $"Number of label columns is {App.PaperNumberOfLabelColumns}.";

            PaperFontFamilyNameComboBox.Items.Clear();
            PaperFontFamilyNameComboBox.ItemsSource = App.NamedFontFamilies;
            PaperFontFamilyNameComboBox.SelectedItem = App.NamedFontFamilies.FirstOrDefault(n => n.PaperFontFamilyName == App.PaperFontFamilyName);
            PaperFontFamilyNameComboBox.Header = $"Font name is '{App.PaperFontFamilyName}'.";

            PaperFontColorNameComboBox.Items.Clear();
            PaperFontColorNameComboBox.ItemsSource = App.NamedColors.Select(n => n).ToList();
            PaperFontColorNameComboBox.SelectedItem = App.NamedColors.FirstOrDefault(n => n.ColorName == App.PaperFontColorName);
            PaperFontColorNameComboBox.Header = $"Font color name is '{App.PaperFontColorName}'.";

            PaperFontWeightNameComboBox.Items.Clear();
            PaperFontWeightNameComboBox.ItemsSource = App.NamedFontWeights.Select(n => n.FontWeightName).ToList();
            PaperFontWeightNameComboBox.SelectedValue = App.PaperFontWeightName;
            PaperFontWeightNameComboBox.Header = $"Fontweight is '{App.PaperFontWeightName}'.";

            PaperFontSizeSlider.Value = App.PaperFontSize;
            PaperFontSizeSlider.Header = $"Font size is {App.PaperFontSize} px.";

            PaperLeftAndRightMarginSlider.Value = App.PaperLeftAndRightMargin;
            PaperLeftAndRightMarginSlider.Header = $"Paper left and right margin is {App.PaperLeftAndRightMargin * 25.4 / 96:N1} mm.";
            PaperTopAndBottomMarginSlider.Value = App.PaperTopAndBottomMargin;
            PaperTopAndBottomMarginSlider.Header = $"Paper top and bottom margin is {App.PaperTopAndBottomMargin * 25.4 / 96:N1} mm.";
            LabelTextTopMarginSlider.Value = App.LabelTextTopMargin;
            LabelTextTopMarginSlider.Header = $"Label text top margin is {App.LabelTextTopMargin * 25.4 / 96:N1} mm.";
            LabelTextBottomMarginSlider.Value = App.LabelTextBottomMargin;
            LabelTextBottomMarginSlider.Header = $"Label text bottom margin is {App.LabelTextBottomMargin * 25.4 / 96:N1} mm.";
            LabelTextLeftMarginSlider.Value = App.LabelTextLeftMargin;
            LabelTextLeftMarginSlider.Header = $"Label text left margin is {App.LabelTextLeftMargin * 25.4 / 96:N1} mm.";
            LabelTextRightMarginSlider.Value = App.LabelTextRightMargin;
            LabelTextRightMarginSlider.Header = $"Label text right margin is {App.LabelTextRightMargin * 25.4 / 96:N1} mm.";
            #endregion things to do on OnNavigatedTo

            // Initalize common helper class and register for printing
            printHelper = new PrintHelper();
            printHelper.RegisterForPrinting();
            // code here

            DrawPaperGrid();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pageIsLoaded = true;

            await GetData();

            DrawPaperGrid();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // code here
            SaveRoamingValues.SaveValues();

            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
            }
            // code here
        }

        private async Task GetData()
        {
            LabelAddressesInDatabase = LabelAddressesInDatabase == null ? await App.LabelAddressesRepo.GetAllLabelAddressesAsync() : LabelAddressesInDatabase;

            App.NumberOfRecordsInDatabase = LabelAddressesInDatabase.Count;

            App.NumberOfLabelsPerPage = App.PaperNumberOfLabelRows * App.PaperNumberOfLabelColumns;

            App.NumberOfPages = App.NumberOfRecordsInDatabase == 0 ? 0 : App.NumberOfRecordsInDatabase / App.NumberOfLabelsPerPage + (App.NumberOfRecordsInDatabase % App.NumberOfLabelsPerPage > 0 ? 1 : 0);
        }

        public async void DrawPaperGrid()
        {
            if (App.NumberOfRecordsInDatabase > 0)
            {
                // show only one page
                int i = 0;
                List<LabelAddress> LabelAddressesForOnePage = LabelAddressesInDatabase.Skip(i * App.NumberOfLabelsPerPage).Take(App.NumberOfLabelsPerPage).ToList();

                PrintableAreaStackPanel.Children.Clear();
                UIElement grid = await GridHelper.PreparePaperGrid(LabelAddressesForOnePage, null);
                PrintableAreaStackPanel.Children.Add(grid);
                PrintableAreaStackPanel.InvalidateMeasure();
                PrintableAreaStackPanel.UpdateLayout();
            }
            else
            {
                await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mainPage.NotifyUser("Your database for addresses is empty. Add some addresses and try again!", NotifyType.ErrorMessage);
                });
                ColorsWillHelpYouTextBlock.Visibility = Visibility.Collapsed;
            }

            //PaperSizeTextBlock.Text = $"Paper width is {0:N3} cm and height is {1:N3} cm.", App.PaperWidth * 2.54 / 96, App.PaperHeight * 2.54 / 96);
            PaperSizeTextBlock.Text = $"Paper width is {App.PaperWidth * 2.54 / 96:N3} cm and height is {App.PaperHeight * 2.54 / 96:N3} cm. Paper size is '{App.PrintMediaSizeName}' and orientation is '{App.PrintOrientationName}'.";
            LabelSizeTextBlock.Text = $"Label width is {App.LabelWidth * 25.4 / 96:N3} mm and height is {App.LabelHeight * 25.4 / 96:N3} mm.";

            NumberOfDatabaseAddressesTextBlock.Text = App.NumberOfPages < 2 ? $"Database contains {App.NumberOfRecordsInDatabase} addresses / {App.NumberOfLabelsPerPage} labels per page = {App.NumberOfPages} page." :
            $"Database contains {App.NumberOfRecordsInDatabase} addresses / {App.NumberOfLabelsPerPage} labels per page = {App.NumberOfPages} pages.";

            PaperFontSizeSlider.Header = $"Font size is {App.PaperFontSize} px.";
            PaperLeftAndRightMarginSlider.Header = $"Paper left and right margin is {App.PaperLeftAndRightMargin * 25.4 / 96:N1} mm.";
            PaperTopAndBottomMarginSlider.Header = $"Paper top and bottom margin is {App.PaperTopAndBottomMargin * 25.4 / 96:N1} mm.";
            LabelTextTopMarginSlider.Header = $"Label text top margin is {App.LabelTextTopMargin * 25.4 / 96:N1} mm.";
            LabelTextBottomMarginSlider.Header = $"Label text bottom margin is {App.LabelTextBottomMargin * 25.4 / 96:N1} mm.";
            LabelTextLeftMarginSlider.Header = $"Label text left margin is {App.LabelTextLeftMargin * 25.4 / 96:N1} mm.";
            LabelTextRightMarginSlider.Header = $"Label text right margin is {App.LabelTextRightMargin * 25.4 / 96:N1} mm.";
            PaperNumberOfLabelRowsComboBox.Header = $"Number of label rows is {App.PaperNumberOfLabelRows}.";
            PaperNumberOfLabelColumnsComboBox.Header = $"Number of label columns is {App.PaperNumberOfLabelColumns}.";
            PaperFontColorNameComboBox.Header = $"Font color name is '{App.PaperFontColorName}'.";
            PaperFontWeightNameComboBox.Header = $"Fontweight is '{App.PaperFontWeightName}'.";
            PaperFontFamilyNameComboBox.Header = $"Font name is '{App.PaperFontFamilyName}'.";

            // set sliders maximun defaults
            PaperFontSizeSlider.Minimum = 6;
            PaperFontSizeSlider.Maximum = 100;
            PaperLeftAndRightMarginSlider.Maximum = (int)(App.PaperWidth / 2);
            PaperTopAndBottomMarginSlider.Maximum = (int)(App.PaperHeight / 2);
            LabelTextTopMarginSlider.Maximum = (int)(App.LabelHeight / 2);
            LabelTextBottomMarginSlider.Maximum = (int)(App.LabelHeight / 2);
            LabelTextLeftMarginSlider.Maximum = (int)(App.LabelWidth / 2);
            LabelTextRightMarginSlider.Maximum = (int)(App.LabelWidth / 2);

        }

        private void PaperFontSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is Slider slider)
            {
                App.PaperFontSize = slider.Value;
                DrawPaperGrid();
            }
        }

        private void PaperLeftAndRightMarginSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is Slider slider)
            {
                App.PaperLeftAndRightMargin = slider.Value;
                DrawPaperGrid();
            }
        }

        private void PaperTopAndBottomMarginSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is Slider slider)
            {
                App.PaperTopAndBottomMargin = slider.Value;
                DrawPaperGrid();
            }
        }

        private void LabelTextTopMarginSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is Slider slider)
            {
                App.LabelTextTopMargin = slider.Value;
                DrawPaperGrid();
            }
        }


        private void LabelTextBottomMarginSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is Slider slider)
            {
                App.LabelTextBottomMargin = slider.Value;
                DrawPaperGrid();
            }
        }

        private void LabelTextLeftMarginSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is Slider slider)
            {
                App.LabelTextLeftMargin = slider.Value;
                DrawPaperGrid();
            }
        }

        private void LabelTextRightMarginSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is Slider slider)
            {
                App.LabelTextRightMargin = slider.Value;
                DrawPaperGrid();
            }
        }

        private void PaperNumberOfLabelRowsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is ComboBox comboBox)
            {
                App.PaperNumberOfLabelRows = (int)comboBox.SelectedItem;

                App.NumberOfLabelsPerPage = App.PaperNumberOfLabelRows * App.PaperNumberOfLabelColumns;

                App.NumberOfPages = App.NumberOfRecordsInDatabase == 0 ? 0 : App.NumberOfRecordsInDatabase / App.NumberOfLabelsPerPage + (App.NumberOfRecordsInDatabase % App.NumberOfLabelsPerPage > 0 ? 1 : 0);

                DrawPaperGrid();
            }
        }

        private void PaperNumberOfLabelColumnsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is ComboBox comboBox)
            {
                App.PaperNumberOfLabelColumns = (int)comboBox.SelectedItem;

                App.NumberOfLabelsPerPage = App.PaperNumberOfLabelRows * App.PaperNumberOfLabelColumns;

                App.NumberOfPages = App.NumberOfRecordsInDatabase == 0 ? 0 : App.NumberOfRecordsInDatabase / App.NumberOfLabelsPerPage + (App.NumberOfRecordsInDatabase % App.NumberOfLabelsPerPage > 0 ? 1 : 0);

                DrawPaperGrid();
            }
        }

        private void PaperFontColorNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is ComboBox comboBox)
            {
                NamedColor selectedItem = comboBox.SelectedItem as NamedColor;
                App.PaperFontColorName = selectedItem.ColorName;
                App.PaperForegroundSolidColorBrush = App.NamedColors.FirstOrDefault(n => n.ColorName == App.PaperFontColorName).SolidColorBrush;
                DrawPaperGrid();
            }
        }

        private void PaperFontWeightNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is ComboBox comboBox)
            {
                App.PaperFontWeightName = (string)comboBox.SelectedItem;
                App.FontWeight = App.NamedFontWeights.FirstOrDefault(n => n.FontWeightName == App.PaperFontWeightName).FontWeight;
                DrawPaperGrid();
            }
        }

        private void PaperFontFamilyNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!pageIsLoaded)
            {
                return;
            }

            if (sender is ComboBox comboBox)
            {
                NamedFontFamily selectedItem = comboBox.SelectedItem as NamedFontFamily;
                App.PaperFontFamilyName = selectedItem.PaperFontFamilyName;
                App.FontFamily = App.NamedFontFamilies.FirstOrDefault(n => n.PaperFontFamilyName == App.PaperFontFamilyName).FontFamily;
                DrawPaperGrid();
            }
        }


        #region MenuAppBarButton
        private void HomeAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            mainPage.GoToHomePage();
            mainPage.MenuNavigationListView.SelectedIndex = 0;
        }

        private void PaperSettingsAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            switch (PaperSettingsAppBarButton.Label)
            {
                case "Show paper settings":
                    PaperFontFamilyNameStackPanel.Visibility = Visibility.Visible;
                    PaperNumberRowsAndColumnsStackPanel.Visibility = Visibility.Visible;
                    PaperDatabaseAdressesStackPanel.Visibility = Visibility.Visible;
                    PaperMarginStackPanel.Visibility = Visibility.Visible;
                    PaperColorAndWeightStackPanel.Visibility = Visibility.Visible;
                    PaperSizeStackPanel.Visibility = Visibility.Visible;
                    PaperLabelSizeStackPanel.Visibility = Visibility.Visible;
                    PaperNumberRowsAndColumnsStackPanel.Visibility = Visibility.Visible;
                    PaperSettingsAppBarButton.Label = "Hide paper settings";
                    break;
                case "Hide paper settings":
                    PaperFontFamilyNameStackPanel.Visibility = Visibility.Collapsed;
                    PaperNumberRowsAndColumnsStackPanel.Visibility = Visibility.Collapsed;
                    PaperDatabaseAdressesStackPanel.Visibility = Visibility.Collapsed;
                    PaperMarginStackPanel.Visibility = Visibility.Collapsed;
                    PaperColorAndWeightStackPanel.Visibility = Visibility.Collapsed;
                    PaperSizeStackPanel.Visibility = Visibility.Collapsed;
                    PaperLabelSizeStackPanel.Visibility = Visibility.Collapsed;
                    PaperNumberRowsAndColumnsStackPanel.Visibility = Visibility.Collapsed;
                    PaperSettingsAppBarButton.Label = "Show paper settings";
                    break;
            }
        }

        private void LabelSettingsAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            switch (LabelSettingsAppBarButton.Label)
            {
                case "Show label settings":
                    LabelTopStackPanel.Visibility = Visibility.Visible;
                    LabelBottomStackPanel.Visibility = Visibility.Visible;
                    LabelSettingsAppBarButton.Label = "Hide label settings";
                    break;
                case "Hide label settings":
                    LabelTopStackPanel.Visibility = Visibility.Collapsed;
                    LabelBottomStackPanel.Visibility = Visibility.Collapsed;
                    LabelSettingsAppBarButton.Label = "Show label settings";
                    break;
            }
        }

        private async void PrintAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (PrintManager.IsSupported())
            {
                try
                {
                    // Tell the user how to print
                    await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        mainPage.NotifyUser($"Creating {App.NumberOfPages} print preview pages. This might take some time.", NotifyType.StatusMessage);
                    });

                    // Initalize common helper class and register for printing

                    if (App.NumberOfRecordsInDatabase == 0)
                    {
                        // Disable Print preview button
                        PrintAppBarButton.IsEnabled = false;

                        await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            mainPage.NotifyUser("Your database for addresses is empty. Add some addresses and try again!", NotifyType.ErrorMessage);
                            ColorsWillHelpYouTextBlock.Visibility = Visibility.Collapsed;
                        });
                    }
                    else
                    {
                        // Initialize print content for this grid in PrintHelper
                        printHelper.TransferContent(LabelAddressesInDatabase);

                        // Show print UI
                        await printHelper.ShowPrintUIAsync();
                    }
                }
                catch
                {
                    // Printing cannot proceed at this time
                    ContentDialog noPrintingDialog = new ContentDialog()
                    {
                        Title = "Printing error",
                        Content = "Sorry, printing can't proceed at this time.",
                        PrimaryButtonText = "OK"
                    };
                    await noPrintingDialog.ShowAsync();
                }
            }
            else
            {
                // Disable Print preview button
                PrintAppBarButton.IsEnabled = false;

                // Printing is not supported on this device
                ContentDialog noPrintingDialog = new ContentDialog()
                {
                    Title = "Printing not supported",
                    Content = "Sorry, printing is not supported on this device.",
                    PrimaryButtonText = "OK"
                };
                await noPrintingDialog.ShowAsync();
            }
        }

        private async void HelpAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Printing is not supported on this device
            ContentDialog helpPrintingDialog = new ContentDialog()
            {
                Title = "How to change paper size and orientation?",
                Content = "Use the Print preview button to preview and to print pages.\n" +
                "In the Print preview window change paper size and orientation and cancel Print preview!\n\n" +
                "In this manner paper size and orientation are changed on Home page! It is magic.\n\n" +
                "Paper size and orientation are also changed on Print preview page.",
                PrimaryButtonText = "OK"
            };
            await helpPrintingDialog.ShowAsync();

        }
        #endregion MenuAppBarButton
    }
}

