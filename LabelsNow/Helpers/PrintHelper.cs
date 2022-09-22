using LabelsNow.Models;
using LabelsNow.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace LabelsNow.Helpers
{
    public class PrintHelper
    {
        // on this page we can send messages
        private MainPage mainPage;

        // on this page we have a hidden canvas used to hold pages we wish to print
        private HomePage homePage;

        /// <summary>
        /// PrintDocument is used to prepare the pages for printing.
        /// Prepare the pages to print in the handlers for the Paginate, GetPreviewPage, and AddPages events.
        /// </summary>
        private PrintDocument printDoc;

        /// <summary>
        /// Marker interface for document source
        /// </summary>
        private IPrintDocumentSource printDocumentSource;

        private PrintManager printManager;

        /// <summary>
        /// A list of UIElements used to store the print preview pages.  This gives easy access
        /// to any desired preview page.
        /// </summary>
        private List<UIElement> printPreviewPages;

        private List<LabelAddress> LabelAddressesInDatabase;

        /// <summary>
        ///  A hidden canvas used to hold pages we wish to print
        /// </summary>
        private Canvas PrintCanvas
        {
            get
            {
                return homePage.FindName("PrintCanvas") as Canvas;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PrintHelper()
        {
            printPreviewPages = new List<UIElement>();

            mainPage = MainPage.CurrentMainPage;

            homePage = HomePage.CurrentHomePage;
        }

        /// <summary>
        /// This function registers the app for printing with Windows and sets up the necessary event handlers for the print process.
        /// </summary>
        public virtual void RegisterForPrinting()
        {
            printDoc = new PrintDocument();
            printDocumentSource = printDoc.DocumentSource;
            printDoc.Paginate += CreatePrintPreviewPages;
            printDoc.GetPreviewPage += GetPrintPreviewPage;
            printDoc.AddPages += AddPrintPages;

            printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested += PrintTaskRequested;
        }

        /// <summary>
        /// This function unregisters the app for printing with Windows.
        /// </summary>
        public virtual void UnregisterForPrinting()
        {
            if (printDoc == null)
            {
                return;
            }

            printDoc.Paginate -= CreatePrintPreviewPages;
            printDoc.GetPreviewPage -= GetPrintPreviewPage;
            printDoc.AddPages -= AddPrintPages;

            // Remove the handler for printing initialization.
            printManager = PrintManager.GetForCurrentView();
            printManager.PrintTaskRequested -= PrintTaskRequested;

            // Clear the print canvas of preview pages
            PrintCanvas.Children.Clear();
        }

        public async Task ShowPrintUIAsync()
        {
            // Catch and print out any errors reported
            try
            {
                await PrintManager.ShowPrintUIAsync();
            }
            catch (Exception e)
            {
                await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    mainPage.NotifyUser("Error printing: " + e.Message + ", hr=" + e.HResult, NotifyType.ErrorMessage);

                    // Clear the cache of preview pages
                    printPreviewPages.Clear();

                    // Clear the print canvas of preview pages
                    PrintCanvas.Children.Clear();
                });
            }
        }

        /// <summary>
        /// Method that will get print content for the print preview
        /// </summary>
        /// <param name="page">The page to print</param>
        public virtual void TransferContent(List<LabelAddress> labelAddressesInDatabase)
        {
            LabelAddressesInDatabase = labelAddressesInDatabase;
        }

        /// <summary>
        /// This is the event handler for PrintManager.PrintTaskRequested.
        /// </summary>
        /// <param name="sender">PrintManager</param>
        /// <param name="e">PrintTaskRequestedEventArgs </param>
        protected virtual void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("Address labels by LabelsNow", sourceRequested =>
            {
                IList<string> displayedOptions = printTask.Options.DisplayedOptions;

                // Choose the printer options to be shown.
                // The order in which the options are appended determines the order in which they appear in the UI
                displayedOptions.Clear();
                displayedOptions.Add(StandardPrintTaskOptions.MediaSize);
                displayedOptions.Add(StandardPrintTaskOptions.Orientation);
                //displayedOptions.Add(StandardPrintTaskOptions.Bordering);
                //displayedOptions.Add(StandardPrintTaskOptions.PrintQuality);
                displayedOptions.Add(StandardPrintTaskOptions.CustomPageRanges);
                displayedOptions.Add(StandardPrintTaskOptions.Copies);

                try
                {
                    printTask.Options.MediaSize = App.PrintMediaSize;
                    printTask.Options.Orientation = App.PrintOrientation;
                    //printTask.Options.Bordering = PrintBordering.Borderless;
                }
                catch
                {
                    // Preset the default value of the printer option
                    printTask.Options.MediaSize = App.PrintMediaSize = PrintMediaSize.IsoA4;
                    printTask.Options.Orientation = App.PrintOrientation = PrintOrientation.Portrait;
                }

                // Print Task event handler is invoked when the print job is completed.
                printTask.Completed += async (s, args) =>
                {
                    // Notify the user when the print operation fails.
                    if (args.Completion == PrintTaskCompletion.Failed)
                    {
                        await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            mainPage.NotifyUser("Failed to print.", NotifyType.ErrorMessage);

                            //LabelAddressesInDatabase.Clear();

                            // Clear the cache of preview pages
                            printPreviewPages.Clear();

                            // Clear the print canvas of preview pages
                            //PrintCanvas.Children.Clear();
                        });
                    }

                    if (args.Completion == PrintTaskCompletion.Canceled)
                    {
                        await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            //mainPage.NotifyUser("You cancelled to print.", NotifyType.ErrorMessage);

                            //LabelAddressesInDatabase.Clear();

                            // Clear the cache of preview pages
                            printPreviewPages.Clear();

                            // Clear the print canvas of preview pages
                            //PrintCanvas.Children.Clear();
                        });
                    }

                    if (args.Completion == PrintTaskCompletion.Submitted)
                    {
                        await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            //mainPage.NotifyUser("You submitted to print.", NotifyType.ErrorMessage);

                            //LabelAddressesInDatabase.Clear();

                            // Clear the cache of preview pages
                            printPreviewPages.Clear();

                            // Clear the print canvas of preview pages
                            //PrintCanvas.Children.Clear();
                        });
                    }

                    if (args.Completion == PrintTaskCompletion.Abandoned)
                    {
                        await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            //mainPage.NotifyUser("You abandoned to print.", NotifyType.ErrorMessage);

                            //LabelAddressesInDatabase.Clear();

                            // Clear the cache of preview pages
                            printPreviewPages.Clear();

                            // Clear the print canvas of preview pages
                            //PrintCanvas.Children.Clear();
                        });
                    }
                };

                sourceRequested.SetSource(printDocumentSource);
            });
        }

        //private int pageNumber=0;
        /// <summary>
        /// This is the event handler for PrintDocument.Paginate. It creates print preview pages for the app.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Paginate Event Arguments</param>
        protected async virtual void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            if (sender is PrintDocument printDocument)
            {
                // Get the PrintTaskOptions
                PrintTaskOptions printTaskOptions = e.PrintTaskOptions;

                PrintPageDescription printPageDescription = printTaskOptions.GetPageDescription(0);

                bool sameMediaSizeAndOrientation = false;
                if (App.PrintMediaSizeName == printTaskOptions.MediaSize.ToString() && App.PrintOrientationName == printTaskOptions.Orientation.ToString())
                {
                    // When size of PrintPreview window is changed CreatePrintPreviewPages is called million times! Prevent it from happening!
                    sameMediaSizeAndOrientation = true;
                }

                App.PrintMediaSize = printTaskOptions.MediaSize;
                App.PrintOrientation = printTaskOptions.Orientation;

                App.PrintMediaSizeName = printTaskOptions.MediaSize.ToString();
                App.PrintOrientationName = printTaskOptions.Orientation.ToString();

                App.PaperHeight = printPageDescription.PageSize.Height;
                App.PaperWidth = printPageDescription.PageSize.Width;

                //App.PaperHeight = printPageDescription.ImageableRect.Height;
                //App.PaperWidth = printPageDescription.ImageableRect.Width;

                if (!sameMediaSizeAndOrientation || printPreviewPages.Count == 0)
                {
                    homePage.DrawPaperGrid();

                    lock (printPreviewPages)
                    {
                        printPreviewPages.Clear();
                    }

                    List<UIElement> printPreviewPagesFromDatabase = new List<UIElement>();
                    try
                    {
                        printPreviewPagesFromDatabase = await FillPrintPreviewPages();
                    }
                    catch (Exception ex)
                    {
                        await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            mainPage.NotifyUser($"Failed to create print preview pages. {ex.Message}.", NotifyType.ErrorMessage);
                        });
                    }

                    lock (printPreviewPages)
                    {
                        try
                        {
                            printPreviewPages.AddRange(printPreviewPagesFromDatabase);

                            // Report the number of preview pages created
                            printDocument.SetPreviewPageCount(printPreviewPages.Count, PreviewPageCountType.Final);
                        }
                        catch
                        {
                        }
                    }

                    await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        mainPage.NotifyUser($"Paper size is '{App.PrintMediaSizeName}' and orientation is '{App.PrintOrientationName}'. {App.NumberOfPages} print preview pages.", NotifyType.StatusMessage);
                    });
                }
            }
        }

        private async Task<List<UIElement>> FillPrintPreviewPages()
        {
            List<UIElement> printPreviewPagesFromDatabase = new List<UIElement>();

            // don't print label colored border lines
            // save values
            var labelBorderThickness = App.LabelBorderThickness;
            var labelBorderBrush = App.LabelBorderBrush;

            // set temporary values
            App.LabelBorderThickness = 0;
            App.LabelBorderBrush = App.PaperBackgroundColor;

            for (int pageNumber = 0; pageNumber < App.NumberOfPages; pageNumber++)
            {
                List<LabelAddress> LabelAddressesForOnePage = LabelAddressesInDatabase.Skip(pageNumber * App.NumberOfLabelsPerPage).Take(App.NumberOfLabelsPerPage).ToList();
                if (LabelAddressesForOnePage.Count > 0)
                {
                    Grid grid = await GridHelper.PreparePaperGrid(LabelAddressesForOnePage, PrintCanvas);

                    // Add the grid to the page preview collection which will be sent to PrintHelper
                    printPreviewPagesFromDatabase.Add(grid);
                }
            }

            // restore values
            App.LabelBorderThickness = labelBorderThickness;
            App.LabelBorderBrush = labelBorderBrush;

            // Clean Canvas on HomePage
            PrintCanvas.Children.Clear();

            return printPreviewPagesFromDatabase;
        }

        /// <summary>
        /// This is the event handler for PrintDocument.GetPrintPreviewPage. It provides a specific print preview page,
        /// in the form of an UIElement, to an instance of PrintDocument. PrintDocument subsequently converts the UIElement
        /// into a page that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Arguments containing the preview requested page</param>
        protected async virtual void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            if (sender is PrintDocument printDocument)
            {
                try
                {
                    printDocument.SetPreviewPage(e.PageNumber, printPreviewPages[e.PageNumber - 1]);
                }
                catch (OutOfMemoryException oex)
                {
                    await mainPage.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        mainPage.NotifyUser($"Failed to get print preview page. {oex.Message}.", NotifyType.ErrorMessage);
                    });
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// This is the event handler for PrintDocument.AddPages. It provides all pages to be printed, in the form of
        /// UIElements, to an instance of PrintDocument. PrintDocument subsequently converts the UIElements
        /// into a pages that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Add page event arguments containing a print task options reference</param>
        protected virtual void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            // Loop over all of the preview pages and add each one to  add each page to be printied
            for (int i = 0; i < printPreviewPages.Count; i++)
            {
                // We should have all pages ready at this point...
                this.printDoc.AddPage(printPreviewPages[i]);
            }

            PrintDocument printDoc = sender as PrintDocument;

            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }
    }
}
