using LabelsNow.Data;
using LabelsNow.Models;
using LabelsNow.ViewModels;
using LabelsNow.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;

namespace LabelsNow.Repository
{
    public class LabelAddressesRepository
    {
        // on this page we can send messages
        private MainPage mainPage;

        public SQLiteAsyncConnection Database { get; set; }

        public LabelAddressesRepository()
        {
            Database = new SQLiteAsyncConnection(Path.Combine(ApplicationData.Current.LocalFolder.Path, "LabelsNow.sqlite"));

            mainPage = MainPage.CurrentMainPage;
        }

        //private async Task DeleteDatabaseTableLabelAddress()
        //{
        //    int x = await Database.DropTableAsync<LabelAddress>();
        //}

        public async Task CreateTableLabelAddressAndSeedAsync()
        {
            try
            {
                CreateTableResult createTableResult = await Database.CreateTableAsync<LabelAddress>();
                switch (createTableResult)
                {
                    case CreateTableResult.Created:
                        // Seed the table
                        foreach (LabelAddressViewModel labelAddressViewModel in TestData.GetTestData())
                        {
                            await AddLabelAddressViewModelAsync(labelAddressViewModel);
                        }
                        break;
                    case CreateTableResult.Migrated:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mainPage.NotifyUser($"Failed to create database file. {ex.Message}.", NotifyType.ErrorMessage);
                });

            }
        }

        public async Task AddLabelAddressViewModelAsync(LabelAddressViewModel labelAddressViewModel)
        {
            string newGuid = Guid.NewGuid().ToString();
            int result = await Database.InsertAsync(new LabelAddress
            {
                Guid = newGuid,
                Line1 = labelAddressViewModel.Line1,
                Line2 = labelAddressViewModel.Line2,
                Line3 = labelAddressViewModel.Line3,
                Line4 = labelAddressViewModel.Line4,
                Line5 = labelAddressViewModel.Line5,
                Line6 = labelAddressViewModel.Line6,
                Line7 = labelAddressViewModel.Line7,
                Line8 = labelAddressViewModel.Line8,
            });
        }

        public async Task<int> AddLabelAddressAsync(LabelAddress labelAddress)
        {
            int numberAddedRecords = 0;

            try
            {
                numberAddedRecords = await Database.InsertAsync(labelAddress);
            }
            catch
            {
            }

            return numberAddedRecords;
        }

        public async Task<int> UpdateLabelAddressAsync(LabelAddress labelAddress)
        {
            int numberUpdatedRecords = 0;

            try
            {
                numberUpdatedRecords = await Database.UpdateAsync(labelAddress);
            }
            catch
            {
            }

            return numberUpdatedRecords;
        }

        public async Task<int> DeleteLabelAddressByGuidAsync(string guid)
        {
            int numberDeletedRecords = await Database.Table<LabelAddress>().Where(i => i.Guid == guid).DeleteAsync();

            return numberDeletedRecords;
        }

        public async Task<LabelAddress> GetLabelAddressByGuidAsync(string guid)
        {
            return await Database.Table<LabelAddress>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
        }

        public async Task<LabelAddressViewModel> GetLabelAddressViewModelByGuidAsync(string guid)
        {
            LabelAddressViewModel labelAddressViewModel = new LabelAddressViewModel();
            LabelAddress labelAddress = await Database.Table<LabelAddress>().Where(i => i.Guid == guid).FirstOrDefaultAsync();
            if (labelAddress != null)
            {
                labelAddressViewModel.Guid = labelAddress.Guid;
                labelAddressViewModel.Line1 = labelAddress.Line1;
                labelAddressViewModel.Line2 = labelAddress.Line2;
                labelAddressViewModel.Line3 = labelAddress.Line3;
                labelAddressViewModel.Line4 = labelAddress.Line4;
                labelAddressViewModel.Line5 = labelAddress.Line5;
                labelAddressViewModel.Line6 = labelAddress.Line6;
                labelAddressViewModel.Line7 = labelAddress.Line7;
                labelAddressViewModel.Line8 = labelAddress.Line8;
            }

            return labelAddressViewModel;
        }

        public async Task<List<LabelAddress>> GetAllLabelAddressesAsync()
        {
            return await Database.Table<LabelAddress>().OrderBy(l => l.Line1).ToListAsync();
        }

        public async Task<ObservableCollection<LabelAddressViewModel>> GetAllLabelAddressViewModelsAsObservableCollectionAsync()
        {
            List<LabelAddress> labelAddressesInDatabase = await GetAllLabelAddressesAsync();
            ObservableCollection<LabelAddressViewModel> observableCollection = new ObservableCollection<LabelAddressViewModel>();
            foreach (var labelAddress in labelAddressesInDatabase)
            {
                observableCollection.Add(new LabelAddressViewModel
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
                });
            }

            return observableCollection;
        }

        public async Task<string> GetNumberOfLabelAddressesAsync()
        {
            int numberOfLabelAddresses = await Database.Table<LabelAddress>().CountAsync();
            return numberOfLabelAddresses.ToString();
        }
    }
}
