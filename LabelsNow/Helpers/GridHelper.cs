using LabelsNow.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LabelsNow.Helpers
{
    public static class GridHelper
    {
        private static List<LabelAddress> LabelAddresses { get; set; }

        public async static Task<Grid> PreparePaperGrid(List<LabelAddress> LabelAddressesForOnePage, Canvas PrintCanvas)
        {
            Grid paperGrid;
            StackPanel labelStackPanel;

            if (LabelAddressesForOnePage.Count == 0)
            {
                throw new ArgumentNullException("Data for address labels are missing. Add some addresses and try again!");
            }

            LabelAddresses = LabelAddressesForOnePage;

            /* A double value as described above, followed by one of the following unit specifiers: px, in, cm, pt. 
            px (default) is device-independent units (1/96th inch per unit) 
            in is inches; 1in==96px 
            cm is centimeters; 1cm==(96/2.54) px 
            pt is points; 1pt==(96/72) px */

            //double PrintPageWidth = 793; // emulate ISOA4 A4 210 × 297 mm standing paper size
            //double PrintPageHeight = 1122; // emulate ISOA4 A4 210 × 297 mm standing paper size
            //double App.PaperWidth = 21.0 * 96 / 2.54; // emulate ISOA4 standing paper size in pixels
            //double App.PaperHeight = 29.7 * 96 / 2.54; // emulate ISOA4 standing paper size in pixels

            paperGrid = await MakePaperGrid();

            App.LabelWidth = (App.PaperWidth - paperGrid.Padding.Left - paperGrid.Padding.Right) / App.PaperNumberOfLabelColumns;
            App.LabelHeight = (App.PaperHeight - paperGrid.Padding.Top - paperGrid.Padding.Bottom) / App.PaperNumberOfLabelRows;

            int labelNumber = 0;
            for (int i = 0; i < App.PaperNumberOfLabelRows; i++)
            {
                for (int j = 0; j < App.PaperNumberOfLabelColumns; j++)
                {
                    labelStackPanel = await MakeLabelStackPanel($"LabelsNow{i}{j}", labelNumber++);
                    Grid.SetRow(labelStackPanel, i);
                    Grid.SetColumn(labelStackPanel, j);

                    CenterVerticallyStackPanelsContent(labelStackPanel);

                    paperGrid.Children.Add(labelStackPanel);
                }
            }

            if (PrintCanvas != null)
            {
                //Add the(newly created) grid to the print canvas which is part of the visual tree and force it to go through visual tree layout.
                PrintCanvas.Children.Add(paperGrid);
                PrintCanvas.InvalidateMeasure();
                PrintCanvas.UpdateLayout();
            }

            return paperGrid;
        }

        private static void CenterVerticallyStackPanelsContent(StackPanel labelStackPanel)
        {
            // center vertically labelStackPanel's content
            // height of labelStackPanel and it's TextBlock children
            if (labelStackPanel.Children.Count > 0)
            {
                labelStackPanel.Measure(new Size(double.MaxValue, double.MaxValue));
                double labelStackPanelDesiredHeight = labelStackPanel.DesiredSize.Height - App.LabelTextTopMargin - App.LabelTextBottomMargin;
                double summaTextBlockDesiredHeight = 0;
                foreach (UIElement item in labelStackPanel.Children)
                {
                    if (item is TextBlock textBlock)
                    {
                        //textBlock.Measure(new Size(double.MaxValue, double.MaxValue));
                        summaTextBlockDesiredHeight += textBlock.ActualHeight;
                    }
                }
                if ((int)labelStackPanelDesiredHeight > (int)summaTextBlockDesiredHeight)
                {
                    foreach (UIElement item in labelStackPanel.Children)
                    {
                        if (item is TextBlock textBlock)
                        {
                            if (textBlock.Name == "TextBlock0")
                            {
                                // center vertically labelStackPanel's content
                                // add top margin to TextBlock which has the name "TextBlock0"
                                textBlock.Margin = new Thickness(textBlock.Margin.Left, ((int)labelStackPanelDesiredHeight - (int)summaTextBlockDesiredHeight) / 2, 0, 0);

                                textBlock.InvalidateMeasure();
                                textBlock.UpdateLayout();
                                break;
                            }
                        }
                    }
                }
            }
        }

        private static Task<Grid> MakePaperGrid()
        {
            Grid paperGrid = new Grid()
            {
                Name = "LabelsNowPaperGrid",
                Width = App.PaperWidth,
                Height = App.PaperHeight,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Background = App.PaperBackgroundColor,
                Padding = new Thickness(App.PaperLeftAndRightMargin, App.PaperTopAndBottomMargin, App.PaperLeftAndRightMargin, App.PaperTopAndBottomMargin)
            };

            // columns first
            for (int j = 0; j < App.PaperNumberOfLabelColumns; j++)
            {
                paperGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // and then rows
            for (int i = 0; i < App.PaperNumberOfLabelRows; i++)
            {
                paperGrid.RowDefinitions.Add(new RowDefinition());
            }

            return Task.FromResult(paperGrid);
        }

        private async static Task<StackPanel> MakeLabelStackPanel(string stackPanelName, int labelNumber)
        {
            TextBlock textBlock;

            StackPanel stackPanel = new StackPanel
            {
                Name = stackPanelName,
                Width = App.LabelWidth,
                Height = App.LabelHeight,
                BorderBrush = App.LabelBorderBrush,
                BorderThickness = new Thickness(App.LabelBorderThickness),
                Padding = new Thickness(0, App.LabelTextTopMargin, 0, App.LabelTextBottomMargin),
                CornerRadius = new CornerRadius(5)
            };

            LabelAddress labelAddress = null;
            try
            {
                labelAddress = LabelAddresses[labelNumber++];
            }
            catch
            {
            }

            if (labelAddress != null)
            {
                if (!string.IsNullOrEmpty(labelAddress.Line1))
                {
                    textBlock = await MakeTextBlock(labelAddress.Line1, $"TextBlock0");
                    stackPanel.Children.Add(textBlock);
                }
                if (!string.IsNullOrEmpty(labelAddress.Line2))
                {
                    textBlock = await MakeTextBlock(labelAddress.Line2, $"TextBlock1");
                    stackPanel.Children.Add(textBlock);
                }
                if (!string.IsNullOrEmpty(labelAddress.Line3))
                {
                    textBlock = await MakeTextBlock(labelAddress.Line3, $"TextBlock2");
                    stackPanel.Children.Add(textBlock);
                }
                if (!string.IsNullOrEmpty(labelAddress.Line4))
                {
                    textBlock = await MakeTextBlock(labelAddress.Line4, $"TextBlock3");
                    stackPanel.Children.Add(textBlock);
                }
                if (!string.IsNullOrEmpty(labelAddress.Line5))
                {
                    textBlock = await MakeTextBlock(labelAddress.Line5, $"TextBlock4");
                    stackPanel.Children.Add(textBlock);
                }
                if (!string.IsNullOrEmpty(labelAddress.Line6))
                {
                    textBlock = await MakeTextBlock(labelAddress.Line6, $"TextBlock5");
                    stackPanel.Children.Add(textBlock);
                }
                if (!string.IsNullOrEmpty(labelAddress.Line7))
                {
                    textBlock = await MakeTextBlock(labelAddress.Line7, $"TextBlock6");
                    stackPanel.Children.Add(textBlock);
                }
                if (!string.IsNullOrEmpty(labelAddress.Line8))
                {
                    textBlock = await MakeTextBlock(labelAddress.Line8, $"TextBlock7");
                    stackPanel.Children.Add(textBlock);
                }
            }

            return stackPanel;
        }

        private static Task<TextBlock> MakeTextBlock(string text, string textBlockName)
        {
            TextBlock textBlock = new TextBlock()
            {
                Name = textBlockName,
                Text = text,
                FontFamily = App.FontFamily,
                FontWeight = App.FontWeight,
                Foreground = App.PaperForegroundSolidColorBrush,
                FontSize = App.PaperFontSize,
                TextAlignment = TextAlignment.Left,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.WrapWholeWords,
                Margin = new Thickness(App.LabelTextLeftMargin, 0, App.LabelTextRightMargin, 0)
            };

            return Task.FromResult(textBlock);
        }
    }
}
