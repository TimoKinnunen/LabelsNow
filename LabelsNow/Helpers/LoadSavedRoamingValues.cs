using System.Threading.Tasks;

namespace LabelsNow.Helpers
{
    public sealed class LoadSavedRoamingValues
    {
        public static void LoadRoamingValues()
        {
            Task.Run(() => GetPaperNumberOfLabelRows(8));
            Task.Run(() => GetPaperNumberOfLabelColumns(3));
            Task.Run(() => GetPaperLeftAndRightMargin(0));
            Task.Run(() => GetPaperTopAndBottomMargin(0));
            Task.Run(() => GetPaperFontColorName("Black"));
            Task.Run(() => GetPaperFontWeightName("Normal"));
            Task.Run(() => GetPaperFontFamilyName("Segoe UI"));
            Task.Run(() => GetPaperFontSize(12));
            Task.Run(() => GetAddNewAddressAppBarButton(8));
            Task.Run(() => GetLabelTextBottomMargin(10));
            Task.Run(() => GetLabelTextLeftMargin(20));
            Task.Run(() => GetLabelTextRightMargin(0));
        }

        private static void GetPaperNumberOfLabelRows(int defaultValue)
        {
            App.PaperNumberOfLabelRows = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["PaperNumberOfLabelRows"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.PaperNumberOfLabelRows = int.Parse(value);
                }
                catch
                {
                }
            }
        }

        private static void GetPaperNumberOfLabelColumns(int defaultValue)
        {
            App.PaperNumberOfLabelColumns = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["PaperNumberOfLabelColumns"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.PaperNumberOfLabelColumns = int.Parse(value);
                }
                catch
                {
                }
            }
        }

        private static void GetPaperLeftAndRightMargin(double defaultValue)
        {
            App.PaperLeftAndRightMargin = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["PaperLeftAndRightMargin"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.PaperLeftAndRightMargin = double.Parse(value);
                }
                catch
                {
                }
            }
        }

        private static void GetPaperTopAndBottomMargin(double defaultValue)
        {
            App.PaperTopAndBottomMargin = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["PaperTopAndBottomMargin"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.PaperTopAndBottomMargin = double.Parse(value);
                }
                catch
                {
                }
            }
        }

        private static void GetPaperFontColorName(string defaultValue)
        {
            App.PaperFontColorName = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["PaperFontColorName"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.PaperFontColorName = value;
                }
                catch
                {
                }
            }
        }

        private static void GetPaperFontWeightName(string defaultValue)
        {
            App.PaperFontWeightName = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["PaperFontWeightName"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.PaperFontWeightName = value;
                }
                catch
                {
                }
            }
        }

        private static void GetPaperFontFamilyName(string defaultValue)
        {
            App.PaperFontFamilyName = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["PaperFontFamilyName"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.PaperFontFamilyName = value;
                }
                catch
                {
                }
            }
        }

        private static void GetPaperFontSize(double defaultValue)
        {
            App.PaperFontSize = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["PaperFontSize"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.PaperFontSize = double.Parse(value);
                }
                catch
                {
                }
            }
        }

        private static void GetAddNewAddressAppBarButton(double defaultValue)
        {
            App.LabelTextTopMargin = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["AddNewAddressAppBarButton"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.LabelTextTopMargin = double.Parse(value);
                }
                catch
                {
                }
            }
        }

        private static void GetLabelTextBottomMargin(double defaultValue)
        {
            App.LabelTextBottomMargin = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["LabelTextBottomMargin"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.LabelTextBottomMargin = double.Parse(value);
                }
                catch
                {
                }
            }
        }

        private static void GetLabelTextLeftMargin(double defaultValue)
        {
            App.LabelTextLeftMargin = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["LabelTextLeftMargin"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.LabelTextLeftMargin = double.Parse(value);
                }
                catch
                {
                }
            }
        }

        private static void GetLabelTextRightMargin(double defaultValue)
        {
            App.LabelTextRightMargin = defaultValue;
            string value = App.ApplicationDataRoamingSettings.Values["LabelTextRightMargin"] as string;
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    App.LabelTextRightMargin = double.Parse(value);
                }
                catch
                {
                }
            }
        }
    }
}
