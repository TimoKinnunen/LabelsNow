using LabelsNow.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace LabelsNow.Views
{
    public sealed partial class DatabasePage : Page
    {
        private MainPage mainPage;

        public DatabasePage()
        {
            InitializeComponent();

            Loaded += Page_Loaded;

            mainPage = MainPage.CurrentMainPage;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<LabelAddress> LabelAddressesInDatabase = await App.LabelAddressesRepo.GetAllLabelAddressesAsync();
            int numberOfRecordsInDatabase = LabelAddressesInDatabase.Count;
            if (numberOfRecordsInDatabase > 0)
            {
                OutputTextBlock.Text = numberOfRecordsInDatabase < 2 ? $"Database contains {numberOfRecordsInDatabase} address." : $"Database contains {numberOfRecordsInDatabase} addresses.";
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
        #endregion MenuAppBarButton

        private async void BackupAddressesAsJsonButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations
            OutputTextBlock.Text = "Backup started.";

            List<LabelAddress> LabelAddressesInDatabase = await App.LabelAddressesRepo.GetAllLabelAddressesAsync();
            var numberBackedUpAddresses = LabelAddressesInDatabase.Count;

            if (numberBackedUpAddresses == 0)
            {
                OutputTextBlock.Text = "Database is empty.";
                return;
            }

            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("JSON", new List<string>() { ".json" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = $"LabelsNowBackup{DateTime.Now:yyyyMMdd_HH:mm:ss}";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                DatabasePageProgressRing.IsIndeterminate = true;
                DatabasePageProgressRing.Visibility = Visibility.Visible;

                // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                string json = JsonConvert.SerializeObject(LabelAddressesInDatabase);

                // write to file
                await FileIO.WriteTextAsync(file, json);
                // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    OutputTextBlock.Text = numberBackedUpAddresses < 2 ? $"File {file.Name} was saved. Backed up {numberBackedUpAddresses} address." : $"File {file.Name} was saved. Backed up {numberBackedUpAddresses} addresses.";
                }
                else if (status == FileUpdateStatus.CompleteAndRenamed)
                {
                    OutputTextBlock.Text = numberBackedUpAddresses < 2 ? $"File {file.Name} was renamed and saved. Backed up {numberBackedUpAddresses} address." : $"File {file.Name} was renamed and saved. Backed up {numberBackedUpAddresses} addresses.";
                }
                else
                {
                    OutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
                }

                DatabasePageProgressRing.Visibility = Visibility.Collapsed;
                DatabasePageProgressRing.IsIndeterminate = false;
            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
        }

        private async void RestoreBackupFromJsonButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations
            OutputTextBlock.Text = "Restore backup started.";

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".json");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                DatabasePageProgressRing.IsIndeterminate = true;
                DatabasePageProgressRing.Visibility = Visibility.Visible;

                int numberDuplicateRecords = 0;
                int numberAddedRecords = 0;

                string json = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);

                List<LabelAddress> LabelAddressesInDatabase = JsonConvert.DeserializeObject<List<LabelAddress>>(json);

                if (LabelAddressesInDatabase != null)
                {
                    foreach (LabelAddress labelAddress in LabelAddressesInDatabase)
                    {
                        LabelAddress newLabelAddress = new LabelAddress
                        {
                            Guid = labelAddress.Guid,
                            Line1 = labelAddress.Line1,
                            Line2 = labelAddress.Line2,
                            Line3 = labelAddress.Line3,
                            Line4 = labelAddress.Line4,
                            Line5 = labelAddress.Line5,
                            Line6 = labelAddress.Line6,
                            Line7 = labelAddress.Line7,
                            Line8 = labelAddress.Line8
                        };
                        int numberAddedRecord = await App.LabelAddressesRepo.AddLabelAddressAsync(newLabelAddress);
                        if (numberAddedRecord == 0)
                        {
                            // constraint error because primary key exists in database
                            numberDuplicateRecords++;
                        }
                        numberAddedRecords += numberAddedRecord;
                    }
                    OutputTextBlock.Text = numberDuplicateRecords == 0 ? $"Added {numberAddedRecords} addresses out of {LabelAddressesInDatabase.Count} addresses in file." :
                         $"Added {numberAddedRecords} addresses out of {LabelAddressesInDatabase.Count} addresses in file and found {numberDuplicateRecords} duplicates (=same primary key) when restoring. Duplicates are not restored.";
                }
                else
                {
                    OutputTextBlock.Text = "Operation failed. File content should originate from LabelsNow app. Primary key (Guid) and maximum of 8 lines of text are expected to be found in a record in backup. Line1 is mandatory.";
                }

                DatabasePageProgressRing.Visibility = Visibility.Collapsed;
                DatabasePageProgressRing.IsIndeterminate = false;
            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
        }

        private async void DeleteAllAddressesInDatabaseButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations
            OutputTextBlock.Text = "Delete all addresses in database started. Please wait.";

            List<LabelAddress> LabelAddressesInDatabase = await App.LabelAddressesRepo.GetAllLabelAddressesAsync();

            if (LabelAddressesInDatabase.Count > 0)
            {
                ContentDialog textOnLine1IsMandatoryDialog = new ContentDialog()
                {
                    Title = "Database data will be deleted.",
                    Content = "Are you sure and want to delete all data? Make a backup first, if needed.",
                    PrimaryButtonText = "Cancel",
                    SecondaryButtonText = "Delete"
                };
                var contentDialogResult = await textOnLine1IsMandatoryDialog.ShowAsync();

                switch (contentDialogResult)
                {
                    case ContentDialogResult.Primary:
                        OutputTextBlock.Text = "Operation cancelled.";
                        break;
                    case ContentDialogResult.Secondary:
                        DatabasePageProgressRing.IsIndeterminate = true;
                        DatabasePageProgressRing.Visibility = Visibility.Visible;

                        int numberDeletedRecord = 0;
                        int numberDeletedRecords = 0;
                        foreach (LabelAddress labelAddress in LabelAddressesInDatabase)
                        {
                            string guid = labelAddress.Guid;
                            numberDeletedRecord = await App.LabelAddressesRepo.DeleteLabelAddressByGuidAsync(guid);
                            numberDeletedRecords += numberDeletedRecord;
                        }

                        OutputTextBlock.Text = numberDeletedRecords < 2 ? $"Deleted {numberDeletedRecords} address." : $"Deleted {numberDeletedRecords} addresses.";

                        DatabasePageProgressRing.Visibility = Visibility.Collapsed;
                        DatabasePageProgressRing.IsIndeterminate = false;
                        break;
                }
            }
            else
            {
                OutputTextBlock.Text = "Database is already empty. No action needed.";
            }
        }

        private async void SaveAddressesAsTabDelimitedTxtButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations
            OutputTextBlock.Text = "Save addresses as tab-delimited .txt-file started.";

            List<LabelAddress> LabelAddressesInDatabase = await App.LabelAddressesRepo.GetAllLabelAddressesAsync();
            var numberBackedUpAddresses = LabelAddressesInDatabase.Count;

            if (numberBackedUpAddresses == 0)
            {
                OutputTextBlock.Text = "Database is empty.";
                return;
            }

            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Tab-delimited", new List<string>() { ".txt" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = $"LabelsNowTabDelimited{DateTime.Now:yyyyMMdd_HH:mm:ss}";
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                DatabasePageProgressRing.IsIndeterminate = true;
                DatabasePageProgressRing.Visibility = Visibility.Visible;

                // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                StringBuilder stringBuilder = new StringBuilder();

                foreach (LabelAddress labelAddress in LabelAddressesInDatabase)
                {
                    stringBuilder.Append($"{labelAddress.Line1}\t");
                    stringBuilder.Append($"{labelAddress.Line2}\t");
                    stringBuilder.Append($"{labelAddress.Line3}\t");
                    stringBuilder.Append($"{labelAddress.Line4}\t");
                    stringBuilder.Append($"{labelAddress.Line5}\t");
                    stringBuilder.Append($"{labelAddress.Line6}\t");
                    stringBuilder.Append($"{labelAddress.Line7}\t");
                    stringBuilder.AppendLine($"{labelAddress.Line8}\t");
                }
                // write to file
                await FileIO.WriteTextAsync(file, stringBuilder.ToString(), Windows.Storage.Streams.UnicodeEncoding.Utf8);
                // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    OutputTextBlock.Text = numberBackedUpAddresses < 2 ? $"File {file.Name} was saved. Saved {numberBackedUpAddresses} address." : $"File {file.Name} was saved. Saved {numberBackedUpAddresses} addresses.";
                }
                else if (status == FileUpdateStatus.CompleteAndRenamed)
                {
                    OutputTextBlock.Text = numberBackedUpAddresses < 2 ? $"File {file.Name} was renamed and saved. Saved {numberBackedUpAddresses} address." : $"File {file.Name} was renamed and saved. Saved {numberBackedUpAddresses} addresses.";
                }
                else
                {
                    OutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
                }

                DatabasePageProgressRing.Visibility = Visibility.Collapsed;
                DatabasePageProgressRing.IsIndeterminate = false;
            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
        }

        private async void ReadAddressesFromTabDelimitedTxtButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Clear previous returned file name, if it exists, between iterations
            OutputTextBlock.Text = "Read addresses from tab-delimited .txt-file started.";

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".txt");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                DatabasePageProgressRing.IsIndeterminate = true;
                DatabasePageProgressRing.Visibility = Visibility.Visible;

                int numberAddedRecords = 0;
                int numberSuspiciousRecords = 0;

                var tabDelimiter = new char[] { '\t' };
                var newlineDelimiter = Environment.NewLine.ToCharArray();

                string fileContent = null;
                try
                {
                    fileContent = await FileIO.ReadTextAsync(file);
                }
                catch (Exception ex)
                {
                    OutputTextBlock.Text = $"Operation failed. {ex.Message}";

                    DatabasePageProgressRing.Visibility = Visibility.Collapsed;
                    DatabasePageProgressRing.IsIndeterminate = false;

                    return;
                }

                string[] addressesInFile = fileContent.Split(newlineDelimiter);


                foreach (var addressInFile in addressesInFile)
                {
                    if (string.IsNullOrEmpty(addressInFile))
                    {
                        continue;
                    }
                    string[] lines = addressInFile.Split(tabDelimiter);
                    LabelAddress newLabelAddress = new LabelAddress()
                    {
                        Guid = Guid.NewGuid().ToString(),
                        Line1 = string.Empty,
                        Line2 = string.Empty,
                        Line3 = string.Empty,
                        Line4 = string.Empty,
                        Line5 = string.Empty,
                        Line6 = string.Empty,
                        Line7 = string.Empty,
                        Line8 = string.Empty
                    };
                    // don't know how many columns there is?
                    for (int i = 0; i < Math.Min(8, lines.Length); i++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (string.IsNullOrEmpty(lines[0]))
                                {
                                    newLabelAddress.Line1 = "Line1 is mandatory";
                                    numberSuspiciousRecords++;
                                }
                                else
                                {
                                    newLabelAddress.Line1 = lines[0];
                                }
                                break;
                            case 1:
                                newLabelAddress.Line2 = lines[1];
                                break;
                            case 2:
                                newLabelAddress.Line3 = lines[2];
                                break;
                            case 3:
                                newLabelAddress.Line4 = lines[3];
                                break;
                            case 4:
                                newLabelAddress.Line5 = lines[4];
                                break;
                            case 5:
                                newLabelAddress.Line6 = lines[5];
                                break;
                            case 6:
                                newLabelAddress.Line7 = lines[6];
                                break;
                            case 7:
                                newLabelAddress.Line8 = lines[7];
                                break;
                            default:
                                break;
                        }

                    }

                    int numberAddedRecord = await App.LabelAddressesRepo.AddLabelAddressAsync(newLabelAddress);
                    numberAddedRecords += numberAddedRecord;
                }
                OutputTextBlock.Text = numberSuspiciousRecords == 0 ? $"Added {numberAddedRecords} addresses." : $"Added {numberAddedRecords} addresses. Suspicious addresses {numberSuspiciousRecords}, e.g. 'Line1 is mandatory'.";

                DatabasePageProgressRing.Visibility = Visibility.Collapsed;
                DatabasePageProgressRing.IsIndeterminate = false;
            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
        }
    }
}
