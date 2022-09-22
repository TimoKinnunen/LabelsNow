using LabelsNow.Models;
using LabelsNow.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace LabelsNow.Views
{
    public sealed partial class AddressesPage : Page
    {
        private MainPage mainPage;

        private LabelAddressViewModel selectedLabelAddressViewModel;

        private ObservableCollection<LabelAddressViewModel> LabelAddressViewModels;

        public AddressesPage()
        {
            InitializeComponent();

            Loaded += AddressesPage_Loaded;

            mainPage = MainPage.CurrentMainPage;
        }

        private async void AddressesPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the addresses from a SQLite database
            // Remember to enable the NavigationCacheMode of this Page to avoid
            // load the data every time user navigates back and forward.    

            LabelAddressViewModels = await App.LabelAddressesRepo.GetAllLabelAddressViewModelsAsObservableCollectionAsync();

            if (LabelAddressViewModels.Count > 0)
            {
                MasterListView.ItemsSource = LabelAddressViewModels;

                RecordStatusTextBlock.Text = string.Empty;
            }

            DatabaseStatusTextBlock.Text = LabelAddressViewModels.Count < 2 ?
                $"Database contains {LabelAddressViewModels.Count} address." :
                $"Database contains {LabelAddressViewModels.Count} addresses.";

            RecordStatusTextBlock.Text = string.Empty;

            ShowButtonsWhenAddressCountIsChanged();
        }

        private void ShowButtonsWhenAddressCountIsChanged()
        {
            Line1TextBox.Text = string.Empty;
            Line2TextBox.Text = string.Empty;
            Line3TextBox.Text = string.Empty;
            Line4TextBox.Text = string.Empty;
            Line5TextBox.Text = string.Empty;
            Line6TextBox.Text = string.Empty;
            Line7TextBox.Text = string.Empty;
            Line8TextBox.Text = string.Empty;

            DetailContentPresenter.Visibility = Visibility.Visible;
            AddNewAddressStackPanel.Visibility = Visibility.Collapsed;

            MasterListView.IsEnabled = true;

            MasterListView.SelectionMode = ListViewSelectionMode.Single;

            if (LabelAddressViewModels.Count > 0)
            {
                ShowSearchAppBarButton.Visibility = Visibility.Visible;
                HideSearchAppBarButton.Visibility = Visibility.Collapsed;
                SearchTextBox.Visibility = Visibility.Collapsed;

                SelectMultipleAddressesAppBarButton.Visibility = Visibility.Visible;
                DeleteMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;
                CancelSelectMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;

                AddNewAddressAppBarButton.Visibility = Visibility.Visible;
                SaveNewAddressAppBarButton.Visibility = Visibility.Collapsed;
                CancelNewAddressAppBarButton.Visibility = Visibility.Collapsed;

                if (MasterListView.SelectedItem != null)
                {
                    CopyAddressAppBarButton.Visibility = Visibility.Visible;
                    EditAddressAppBarButton.Visibility = Visibility.Visible;
                    DeleteAddressAppBarButton.Visibility = Visibility.Visible;
                }
                else
                {
                    CopyAddressAppBarButton.Visibility = Visibility.Collapsed;
                    EditAddressAppBarButton.Visibility = Visibility.Collapsed;
                    DeleteAddressAppBarButton.Visibility = Visibility.Collapsed;
                }
                SaveAddressAfterEditAppBarButton.Visibility = Visibility.Collapsed;
                CancelEditAddressAppBarButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                ShowSearchAppBarButton.Visibility = Visibility.Collapsed;
                HideSearchAppBarButton.Visibility = Visibility.Collapsed;

                SelectMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;
                DeleteMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;
                CancelSelectMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;

                AddNewAddressAppBarButton.Visibility = Visibility.Visible;
                SaveNewAddressAppBarButton.Visibility = Visibility.Collapsed;
                CancelNewAddressAppBarButton.Visibility = Visibility.Collapsed;

                CopyAddressAppBarButton.Visibility = Visibility.Collapsed;
                EditAddressAppBarButton.Visibility = Visibility.Collapsed;
                SaveAddressAfterEditAppBarButton.Visibility = Visibility.Collapsed;
                CancelEditAddressAppBarButton.Visibility = Visibility.Collapsed;

                DeleteAddressAppBarButton.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // code here
            // code here
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // code here
            // code here
        }

        #region MenuAppBarButton
        private void HomeAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            mainPage.GoToHomePage();
            mainPage.MenuNavigationListView.SelectedIndex = 0;
        }

        #region search buttons
        private void ShowSearchAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowSearchAppBarButton.Visibility = Visibility.Collapsed;
            HideSearchAppBarButton.Visibility = Visibility.Visible;

            selectedLabelAddressViewModel = null;
            MasterListView.SelectedItem = null;

            SearchTextBox.Visibility = Visibility.Visible;
            SearchTextBox.Focus(FocusState.Programmatic); // set focus
        }

        private void HideSearchAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowSearchAppBarButton.Visibility = Visibility.Visible;
            HideSearchAppBarButton.Visibility = Visibility.Collapsed;

            DatabaseStatusTextBlock.Text = LabelAddressViewModels.Count < 2 ?
                $"Database contains {LabelAddressViewModels.Count} address." :
                $"Database contains {LabelAddressViewModels.Count} addresses.";

            RecordStatusTextBlock.Text = string.Empty;

            SearchTextBox.Text = string.Empty;
            SearchTextBox.Visibility = Visibility.Collapsed;
        }
        #endregion search buttons

        #endregion MenuAppBarButton

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            selectedLabelAddressViewModel = e.ClickedItem as LabelAddressViewModel;

            if (MasterListView.SelectionMode == ListViewSelectionMode.Single)
            {
                CopyAddressAppBarButton.Visibility = Visibility.Visible;

                EditAddressAppBarButton.Visibility = Visibility.Visible;

                DeleteAddressAppBarButton.Visibility = Visibility.Visible;
            }
        }

        private void EditAddressAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (selectedLabelAddressViewModel != null)
            {
                DetailContentPresenter.Visibility = Visibility.Collapsed;
                AddNewAddressStackPanel.Visibility = Visibility.Visible;

                MasterListView.IsEnabled = false;

                // hide these
                CopyAddressAppBarButton.Visibility = Visibility.Collapsed;
                EditAddressAppBarButton.Visibility = Visibility.Collapsed;
                // show these
                SaveAddressAfterEditAppBarButton.Visibility = Visibility.Visible;
                CancelEditAddressAppBarButton.Visibility = Visibility.Visible;
                // disable these
                ShowSearchAppBarButton.Visibility = Visibility.Collapsed;
                HideSearchAppBarButton.Visibility = Visibility.Collapsed;
                SearchTextBox.Visibility = Visibility.Collapsed;
                SelectMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;
                AddNewAddressAppBarButton.Visibility = Visibility.Collapsed;
                DeleteAddressAppBarButton.Visibility = Visibility.Collapsed;

                // if address is not visible bring it to view
                if (MasterListView.SelectedItem != selectedLabelAddressViewModel)
                {
                    LabelAddressViewModel labelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == selectedLabelAddressViewModel.Guid);
                    if (labelAddressViewModel != null)
                    {
                        MasterListView.ScrollIntoView(labelAddressViewModel, ScrollIntoViewAlignment.Leading);
                    }
                }
                else
                {
                    LabelAddressViewModel labelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == selectedLabelAddressViewModel.Guid);
                    if (labelAddressViewModel != null)
                    {
                        MasterListView.ScrollIntoView(labelAddressViewModel, ScrollIntoViewAlignment.Leading);
                    }
                }

                // show what can be edited
                Line1TextBox.Text = selectedLabelAddressViewModel.Line1;
                Line2TextBox.Text = selectedLabelAddressViewModel.Line2;
                Line3TextBox.Text = selectedLabelAddressViewModel.Line3;
                Line4TextBox.Text = selectedLabelAddressViewModel.Line4;
                Line5TextBox.Text = selectedLabelAddressViewModel.Line5;
                Line6TextBox.Text = selectedLabelAddressViewModel.Line6;
                Line7TextBox.Text = selectedLabelAddressViewModel.Line7;
                Line8TextBox.Text = selectedLabelAddressViewModel.Line8;

                Line1TextBox.Focus(FocusState.Programmatic);
            }
        }

        private async void SaveAddressAfterEditAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (selectedLabelAddressViewModel != null)
            {
                int numberUpdatedRecords = 0;
                if (!string.IsNullOrWhiteSpace(Line1TextBox.Text))
                {
                    // find record from database
                    LabelAddress labelAddress = await App.LabelAddressesRepo.GetLabelAddressByGuidAsync(selectedLabelAddressViewModel.Guid);
                    if (labelAddress != null)
                    {
                        labelAddress.Line1 = Line1TextBox.Text;
                        labelAddress.Line2 = Line2TextBox.Text;
                        labelAddress.Line3 = Line3TextBox.Text;
                        labelAddress.Line4 = Line4TextBox.Text;
                        labelAddress.Line5 = Line5TextBox.Text;
                        labelAddress.Line6 = Line6TextBox.Text;
                        labelAddress.Line7 = Line7TextBox.Text;
                        labelAddress.Line8 = Line8TextBox.Text;

                        // update in database
                        numberUpdatedRecords = await App.LabelAddressesRepo.UpdateLabelAddressAsync(labelAddress);
                        //if (numberUpdatedRecords == 1)
                        //{
                        //    // update ListView
                        //    //selectedLabelAddressViewModel.Line1 = Line1TextBox.Text;
                        //    //selectedLabelAddressViewModel.Line2 = Line2TextBox.Text;
                        //    //selectedLabelAddressViewModel.Line3 = Line3TextBox.Text;
                        //    //selectedLabelAddressViewModel.Line4 = Line4TextBox.Text;
                        //    //selectedLabelAddressViewModel.Line5 = Line5TextBox.Text;
                        //    //selectedLabelAddressViewModel.Line6 = Line6TextBox.Text;
                        //    //selectedLabelAddressViewModel.Line7 = Line7TextBox.Text;
                        //    //selectedLabelAddressViewModel.Line8 = Line8TextBox.Text;
                        //}
                    }

                    LabelAddressViewModels = await App.LabelAddressesRepo.GetAllLabelAddressViewModelsAsObservableCollectionAsync();
                    MasterListView.ItemsSource = LabelAddressViewModels;
                    LabelAddressViewModel existingLabelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == selectedLabelAddressViewModel.Guid);
                    if (existingLabelAddressViewModel != null)
                    {
                        MasterListView.ScrollIntoView(existingLabelAddressViewModel, ScrollIntoViewAlignment.Leading);
                        MasterListView.SelectedItem = existingLabelAddressViewModel as LabelAddressViewModel;
                        selectedLabelAddressViewModel = existingLabelAddressViewModel as LabelAddressViewModel;
                    }
                }
                else
                {
                    await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        mainPage.NotifyUser("Text on line1 is mandatory.\nSorry, can't save when text is missing on line1.\nEnter some text on line1. Try again!", NotifyType.ErrorMessage);
                    });
                }
                DatabaseStatusTextBlock.Text = LabelAddressViewModels.Count < 2 ?
                    $"Database contains {LabelAddressViewModels.Count} address." :
                    $"Database contains {LabelAddressViewModels.Count} addresses.";

                RecordStatusTextBlock.Text = numberUpdatedRecords < 2 ?
                    $"Updated {numberUpdatedRecords} address." :
                    $"Updated {numberUpdatedRecords} addresses.";

                ShowButtonsWhenAddressCountIsChanged();
            }
        }

        private void CancelEditAddressAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowButtonsWhenAddressCountIsChanged();
        }

        private void SelectMultipleAddressesAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MasterListView.SelectionMode = ListViewSelectionMode.Multiple;

            SelectMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;

            DeleteMultipleAddressesAppBarButton.Visibility = Visibility.Visible;
            CancelSelectMultipleAddressesAppBarButton.Visibility = Visibility.Visible;

            AddNewAddressAppBarButton.Visibility = Visibility.Collapsed;
            SaveNewAddressAppBarButton.Visibility = Visibility.Collapsed;
            CancelNewAddressAppBarButton.Visibility = Visibility.Collapsed;

            CopyAddressAppBarButton.Visibility = Visibility.Collapsed;
            EditAddressAppBarButton.Visibility = Visibility.Collapsed;
            SaveAddressAfterEditAppBarButton.Visibility = Visibility.Collapsed;
            CancelEditAddressAppBarButton.Visibility = Visibility.Collapsed;

            DeleteAddressAppBarButton.Visibility = Visibility.Collapsed;
        }

        private async void DeleteMultipleAddressesAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RecordStatusTextBlock.Text = "Deleting addresses. Please wait.";

            AddressesPageProgressRing.IsIndeterminate = true;
            AddressesPageProgressRing.Visibility = Visibility.Visible;

            DeleteMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;

            // disable these
            ShowSearchAppBarButton.Visibility = Visibility.Collapsed;
            HideSearchAppBarButton.Visibility = Visibility.Collapsed;
            SearchTextBox.Visibility = Visibility.Collapsed;

            int numberDeletedRecords = 0;

            var firstDeletedId = 0;

            List<LabelAddressViewModel> selectedItems = new List<LabelAddressViewModel>();

            foreach (LabelAddressViewModel labelAddressViewModel in MasterListView.SelectedItems)
            {
                selectedItems.Add(labelAddressViewModel);

                firstDeletedId = MasterListView.SelectedIndex; // index for first MasterListView record we select to delete
            }

            if (selectedItems.Count > 0)
            {
                foreach (LabelAddressViewModel labelAddressViewModel in selectedItems)
                {
                    string guid = labelAddressViewModel.Guid;
                    LabelAddressViewModels.Remove(labelAddressViewModel);
                    int numberDeletedRecord = await App.LabelAddressesRepo.DeleteLabelAddressByGuidAsync(guid);
                    numberDeletedRecords += numberDeletedRecord;

                    var xx = MasterListView.SelectedIndex;
                }

                selectedLabelAddressViewModel = null;

                LabelAddressViewModels = await App.LabelAddressesRepo.GetAllLabelAddressViewModelsAsObservableCollectionAsync();
                MasterListView.ItemsSource = LabelAddressViewModels;

                #region find suitable record for ScrollIntoView
                // hope that LabelAddressViewModels records are in same order as MasterListView records?
                var i = 0;
                string suitableGuid = string.Empty;
                foreach (var item in LabelAddressViewModels)
                {
                    if (i == firstDeletedId)
                    {
                        suitableGuid = item.Guid;
                        break;
                    }
                    i++;
                }
                LabelAddressViewModel existingLabelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == suitableGuid);
                if (existingLabelAddressViewModel != null)
                {
                    MasterListView.ScrollIntoView(existingLabelAddressViewModel, ScrollIntoViewAlignment.Leading);
                    //MasterListView.SelectedItem = existingLabelAddressViewModel as LabelAddressViewModel;
                    //selectedLabelAddressViewModel = existingLabelAddressViewModel as LabelAddressViewModel;
                }
                if (existingLabelAddressViewModel == null)
                {
                    if (firstDeletedId > 0)
                    {
                        i = 0;
                        firstDeletedId--; // deleted record might be last record, decrease with one
                        suitableGuid = string.Empty;
                        foreach (LabelAddressViewModel labelAddressViewModel in LabelAddressViewModels)
                        {
                            if (i == firstDeletedId)
                            {
                                suitableGuid = labelAddressViewModel.Guid;
                                break;
                            }
                            i++;
                        }
                        existingLabelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == suitableGuid);
                        if (existingLabelAddressViewModel != null)
                        {
                            MasterListView.ScrollIntoView(existingLabelAddressViewModel, ScrollIntoViewAlignment.Leading);
                            //MasterListView.SelectedItem = existingLabelAddressViewModel as LabelAddressViewModel;
                            //selectedLabelAddressViewModel = existingLabelAddressViewModel as LabelAddressViewModel;
                        }
                    }
                }
                #endregion find suitable record for ScrollIntoView

            }

            DatabaseStatusTextBlock.Text = LabelAddressViewModels.Count < 2 ?
                $"Database contains {LabelAddressViewModels.Count} address." :
                $"Database contains {LabelAddressViewModels.Count} addresses.";

            RecordStatusTextBlock.Text = numberDeletedRecords < 2 ?
                $"Deleted {numberDeletedRecords} address." :
                $"Deleted {numberDeletedRecords} addresses.";

            AddressesPageProgressRing.IsIndeterminate = false;
            AddressesPageProgressRing.Visibility = Visibility.Collapsed;

            ShowButtonsWhenAddressCountIsChanged();
        }

        private void CancelSelectMultipleAddressesAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowButtonsWhenAddressCountIsChanged();
        }

        private void AddNewAddressAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DetailContentPresenter.Visibility = Visibility.Collapsed;
            AddNewAddressStackPanel.Visibility = Visibility.Visible;

            MasterListView.IsEnabled = false;

            // hide this
            AddNewAddressAppBarButton.Visibility = Visibility.Collapsed;
            // show these
            SaveNewAddressAppBarButton.Visibility = Visibility.Visible;
            CancelNewAddressAppBarButton.Visibility = Visibility.Visible;
            // disable these
            ShowSearchAppBarButton.Visibility = Visibility.Collapsed;
            HideSearchAppBarButton.Visibility = Visibility.Collapsed;
            SearchTextBox.Visibility = Visibility.Collapsed;
            SelectMultipleAddressesAppBarButton.Visibility = Visibility.Collapsed;
            AddNewAddressAppBarButton.Visibility = Visibility.Collapsed;
            DeleteAddressAppBarButton.Visibility = Visibility.Collapsed;
            CopyAddressAppBarButton.Visibility = Visibility.Collapsed;
            EditAddressAppBarButton.Visibility = Visibility.Collapsed;

            Line1TextBox.Focus(FocusState.Programmatic);
        }

        private async void SaveNewAddressAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int numberAddedRecords = 0;
            if (!string.IsNullOrWhiteSpace(Line1TextBox.Text))
            {
                string newGuid = Guid.NewGuid().ToString();

                LabelAddress newLabelAddress = new LabelAddress
                {
                    // set id here and not in database
                    Guid = newGuid,
                    Line1 = Line1TextBox.Text,
                    Line2 = Line2TextBox.Text,
                    Line3 = Line3TextBox.Text,
                    Line4 = Line4TextBox.Text,
                    Line5 = Line5TextBox.Text,
                    Line6 = Line6TextBox.Text,
                    Line7 = Line7TextBox.Text,
                    Line8 = Line8TextBox.Text
                };
                // add to database
                numberAddedRecords = await App.LabelAddressesRepo.AddLabelAddressAsync(newLabelAddress);

                LabelAddressViewModels = await App.LabelAddressesRepo.GetAllLabelAddressViewModelsAsObservableCollectionAsync();
                MasterListView.ItemsSource = LabelAddressViewModels;
                LabelAddressViewModel existingLabelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == newGuid);
                if (existingLabelAddressViewModel != null)
                {
                    MasterListView.ScrollIntoView(existingLabelAddressViewModel, ScrollIntoViewAlignment.Leading);
                    MasterListView.SelectedItem = existingLabelAddressViewModel as LabelAddressViewModel;
                    selectedLabelAddressViewModel = existingLabelAddressViewModel as LabelAddressViewModel;
                }
            }
            else
            {
                await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mainPage.NotifyUser("Text on line1 is mandatory.\nSorry, can't save when text is missing on line1.\nEnter some text on line1. Try again!", NotifyType.ErrorMessage);
                });
            }
            DatabaseStatusTextBlock.Text = LabelAddressViewModels.Count < 2 ?
                $"Database contains {LabelAddressViewModels.Count} address." :
                $"Database contains {LabelAddressViewModels.Count} addresses.";

            RecordStatusTextBlock.Text = numberAddedRecords < 2 ?
                $"Added {numberAddedRecords} address." :
                $"Added {numberAddedRecords} addresses.";

            ShowButtonsWhenAddressCountIsChanged();
        }

        private void CancelNewAddressAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowButtonsWhenAddressCountIsChanged();
        }

        private async void DeleteAddressAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (selectedLabelAddressViewModel != null && MasterListView.SelectedItem == selectedLabelAddressViewModel)
            {
                DeleteAddressAppBarButton.Visibility = Visibility.Collapsed;

                // disable these
                ShowSearchAppBarButton.Visibility = Visibility.Collapsed;
                HideSearchAppBarButton.Visibility = Visibility.Collapsed;
                SearchTextBox.Visibility = Visibility.Collapsed;

                string guid = selectedLabelAddressViewModel.Guid;

                var firstDeletedId = MasterListView.SelectedIndex; // index for first MasterListView record we select to delete

                LabelAddressViewModels.Remove(selectedLabelAddressViewModel);
                int numberDeletedRecord = await App.LabelAddressesRepo.DeleteLabelAddressByGuidAsync(guid);

                selectedLabelAddressViewModel = null;

                LabelAddressViewModels = await App.LabelAddressesRepo.GetAllLabelAddressViewModelsAsObservableCollectionAsync();
                MasterListView.ItemsSource = LabelAddressViewModels;

                #region find suitable record for ScrollIntoView
                // hope that LabelAddressViewModels records are in same order as MasterListView records?
                var i = 0;
                string suitableGuid = string.Empty;
                foreach (var item in LabelAddressViewModels)
                {
                    if (i == firstDeletedId)
                    {
                        suitableGuid = item.Guid;
                        break;
                    }
                    i++;
                }
                LabelAddressViewModel existingLabelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == suitableGuid);
                if (existingLabelAddressViewModel != null)
                {
                    MasterListView.ScrollIntoView(existingLabelAddressViewModel, ScrollIntoViewAlignment.Leading);
                    //MasterListView.SelectedItem = existingLabelAddressViewModel as LabelAddressViewModel;
                    //selectedLabelAddressViewModel = existingLabelAddressViewModel as LabelAddressViewModel;
                }
                if (existingLabelAddressViewModel == null)
                {
                    if (firstDeletedId > 0)
                    {
                        i = 0;
                        firstDeletedId--; // deleted record might be last record, decrease with one
                        suitableGuid = string.Empty;
                        foreach (LabelAddressViewModel labelAddressViewModel in LabelAddressViewModels)
                        {
                            if (i == firstDeletedId)
                            {
                                suitableGuid = labelAddressViewModel.Guid;
                                break;
                            }
                            i++;
                        }
                        existingLabelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == suitableGuid);
                        if (existingLabelAddressViewModel != null)
                        {
                            MasterListView.ScrollIntoView(existingLabelAddressViewModel, ScrollIntoViewAlignment.Leading);
                            //MasterListView.SelectedItem = existingLabelAddressViewModel as LabelAddressViewModel;
                            //selectedLabelAddressViewModel = existingLabelAddressViewModel as LabelAddressViewModel;
                        }
                    }
                }
                #endregion find suitable record for ScrollIntoView

                DatabaseStatusTextBlock.Text = LabelAddressViewModels.Count < 2 ?
                    $"Database contains {LabelAddressViewModels.Count} address." :
                    $"Database contains {LabelAddressViewModels.Count} addresses.";

                RecordStatusTextBlock.Text = numberDeletedRecord < 2 ?
                    $"Deleted {numberDeletedRecord} address." :
                    $"Deleted {numberDeletedRecord} addresses.";
            }

            ShowButtonsWhenAddressCountIsChanged();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                var searchText = textBox.Text.ToLower();
                var searchResults = LabelAddressViewModels.Where(l => (l.Line1.ToLower().Contains(searchText) ||
                l.Line2.ToLower().Contains(searchText) ||
                l.Line3.ToLower().Contains(searchText) ||
                l.Line4.ToLower().Contains(searchText) ||
                l.Line5.ToLower().Contains(searchText) ||
                l.Line6.ToLower().Contains(searchText) ||
                l.Line7.ToLower().Contains(searchText) ||
                l.Line8.ToLower().Contains(searchText)));

                MasterListView.ItemsSource = searchResults;

                DatabaseStatusTextBlock.Text = LabelAddressViewModels.Count < 2 ?
                    $"Database contains {LabelAddressViewModels.Count} address." :
                    $"Database contains {LabelAddressViewModels.Count} addresses.";

                RecordStatusTextBlock.Text = searchResults.Count() < 2 ?
                    $"Search gives {searchResults.Count()} address." :
                    $"Search gives {searchResults.Count()} addresses.";
            }
        }

        private async void CopyAddressAppBarButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MasterListView.SelectionMode == ListViewSelectionMode.Single)
            {
                if (MasterListView.SelectedItem != null)
                {
                    LabelAddressViewModel existingLlabelAddressViewModel = MasterListView.SelectedItem as LabelAddressViewModel;
                    if (existingLlabelAddressViewModel != null)
                    {
                        int numberAddedRecords = 0;
                        if (!string.IsNullOrWhiteSpace(existingLlabelAddressViewModel.Line1))
                        {
                            string newGuid = Guid.NewGuid().ToString();

                            LabelAddress newLabelAddress = new LabelAddress
                            {
                                // set id here and not in database
                                Guid = newGuid,
                                Line1 = existingLlabelAddressViewModel.Line1,
                                Line2 = existingLlabelAddressViewModel.Line2,
                                Line3 = existingLlabelAddressViewModel.Line3,
                                Line4 = existingLlabelAddressViewModel.Line4,
                                Line5 = existingLlabelAddressViewModel.Line5,
                                Line6 = existingLlabelAddressViewModel.Line6,
                                Line7 = existingLlabelAddressViewModel.Line7,
                                Line8 = existingLlabelAddressViewModel.Line8
                            };
                            // add to database
                            numberAddedRecords = await App.LabelAddressesRepo.AddLabelAddressAsync(newLabelAddress);

                            LabelAddressViewModels = await App.LabelAddressesRepo.GetAllLabelAddressViewModelsAsObservableCollectionAsync();
                            MasterListView.ItemsSource = LabelAddressViewModels;
                            LabelAddressViewModel existingLabelAddressViewModel = LabelAddressViewModels.FirstOrDefault(l => l.Guid == newGuid);
                            if (existingLabelAddressViewModel != null)
                            {
                                MasterListView.ScrollIntoView(existingLabelAddressViewModel, ScrollIntoViewAlignment.Leading);
                                MasterListView.SelectedItem = existingLabelAddressViewModel as LabelAddressViewModel;
                                selectedLabelAddressViewModel = existingLabelAddressViewModel as LabelAddressViewModel;
                            }
                        }
                        else
                        {
                            await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                mainPage.NotifyUser("Text on line1 is mandatory.\nSorry, can't copy when text is missing on line1.\nEnter some text on line1. Try again!", NotifyType.ErrorMessage);
                            });
                        }
                        DatabaseStatusTextBlock.Text = LabelAddressViewModels.Count < 2 ?
                            $"Database contains {LabelAddressViewModels.Count} address." :
                            $"Database contains {LabelAddressViewModels.Count} addresses.";

                        RecordStatusTextBlock.Text = numberAddedRecords < 2 ?
                            $"Added {numberAddedRecords} address." :
                            $"Added {numberAddedRecords} addresses.";

                        ShowButtonsWhenAddressCountIsChanged();
                    }
                }
            }
        }
    }
}
