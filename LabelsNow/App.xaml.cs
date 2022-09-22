using LabelsNow.Helpers;
using LabelsNow.Models;
using LabelsNow.Repository;
using LabelsNow.Views;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace LabelsNow
{
    sealed partial class App : Application
    {
        public static string PrintMediaSizeName { get; set; } = PrintMediaSize.IsoA4.ToString();
        public static PrintMediaSize PrintMediaSize { get; set; } = PrintMediaSize.IsoA4;
        public static string PrintOrientationName { get; set; } = PrintOrientation.Portrait.ToString();
        public static PrintOrientation PrintOrientation { get; set; } = PrintOrientation.Portrait;

        public static double PaperWidth { get; set; } = 21.0 * 96 / 2.54; // emulate ISOA4 standing paper size in pixels
        public static double PaperHeight { get; set; } = 29.7 * 96 / 2.54; // emulate ISOA4 standing paper size in pixels

        public static int PaperNumberOfLabelRows { get; set; } = 8;
        public static int PaperNumberOfLabelColumns { get; set; } = 3;

        public static int NumberOfRecordsInDatabase { get; set; } = 0;

        public static int NumberOfLabelsPerPage { get; set; } = 0;

        public static int NumberOfPages { get; set; } = 0;

        public static double LabelWidth { get; set; } = 21.0 * 96 / 2.54; // emulate ISOA4 standing paper size in pixels
        public static double LabelHeight { get; set; } = 29.7 * 96 / 2.54; // emulate ISOA4 standing paper size in pixels

        public static SolidColorBrush PaperBackgroundColor { get; set; } = new SolidColorBrush(Colors.White);
        public static double LabelBorderThickness { get; set; } = 1.2;
        public static SolidColorBrush LabelBorderBrush { get; set; } = new SolidColorBrush(Colors.LightGray);

        public static double PaperFontSize { get; set; }

        public static double PaperLeftAndRightMargin { get; set; }
        public static double PaperTopAndBottomMargin { get; set; }

        public static double LabelTextTopMargin { get; set; }
        public static double LabelTextBottomMargin { get; set; }
        public static double LabelTextLeftMargin { get; set; }
        public static double LabelTextRightMargin { get; set; }

        public static string PaperFontFamilyName { get; set; }
        public static FontFamily FontFamily { get; set; }

        public static string PaperFontColorName { get; set; }
        public static SolidColorBrush PaperForegroundSolidColorBrush { get; set; } = new SolidColorBrush(Colors.Black);

        public static string PaperFontWeightName { get; set; }
        public static FontWeight FontWeight { get; set; }

        public static List<NamedColor> NamedColors { get; set; }
        public static List<NamedFontWeight> NamedFontWeights { get; set; }
        public static List<NamedFontFamily> NamedFontFamilies { get; set; }

        public static ApplicationDataContainer ApplicationDataRoamingSettings { get; set; }
        public static ApplicationDataContainer ApplicationDataLocalSettings { get; set; }

        public static LabelAddressesRepository LabelAddressesRepo { get; private set; }

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            ApplicationDataRoamingSettings = ApplicationData.Current.RoamingSettings;
            ApplicationDataLocalSettings = ApplicationData.Current.LocalSettings;

            LoadSavedRoamingValues.LoadRoamingValues();

            LabelAddressesRepo = new LabelAddressesRepository();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                NamedColors = await NamedColorHelper.GetNamedColors();
                NamedFontWeights = await NamedFontWeightHelper.GetNamedFontWeights();
                NamedFontFamilies = await NamedFontFamilyHelper.GetNamedFontFamilies();

                // If table does not exist it will be created and seeded, otherwise it is connected to
                await LabelAddressesRepo.CreateTableLabelAddressAndSeedAsync();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            SaveRoamingValues.SaveValues();
            deferral.Complete();
        }
    }
}

