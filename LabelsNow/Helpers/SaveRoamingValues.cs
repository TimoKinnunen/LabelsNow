using System.Threading.Tasks;

namespace LabelsNow.Helpers
{
    public class SaveRoamingValues
    {
        public static void SaveValues()
        {
            Task.Run(() => Save());
        }

        private static void Save()
        {
            App.ApplicationDataRoamingSettings.Values["PaperNumberOfLabelRows"] = App.PaperNumberOfLabelRows.ToString();
            App.ApplicationDataRoamingSettings.Values["PaperNumberOfLabelColumns"] = App.PaperNumberOfLabelColumns.ToString();
            App.ApplicationDataRoamingSettings.Values["PaperLeftAndRightMargin"] = App.PaperLeftAndRightMargin.ToString();
            App.ApplicationDataRoamingSettings.Values["PaperTopAndBottomMargin"] = App.PaperTopAndBottomMargin.ToString();
            App.ApplicationDataRoamingSettings.Values["PaperFontColorName"] = App.PaperFontColorName.ToString();
            App.ApplicationDataRoamingSettings.Values["PaperFontWeightName"] = App.PaperFontWeightName.ToString();
            App.ApplicationDataRoamingSettings.Values["PaperFontFamilyName"] = App.PaperFontFamilyName.ToString();
            App.ApplicationDataRoamingSettings.Values["AddNewAddressAppBarButton"] = App.LabelTextTopMargin.ToString();
            App.ApplicationDataRoamingSettings.Values["LabelTextBottomMargin"] = App.LabelTextBottomMargin.ToString();
            App.ApplicationDataRoamingSettings.Values["LabelTextLeftMargin"] = App.LabelTextLeftMargin.ToString();
            App.ApplicationDataRoamingSettings.Values["LabelTextRightMargin"] = App.LabelTextRightMargin.ToString();
            App.ApplicationDataRoamingSettings.Values["PaperFontSize"] = App.PaperFontSize.ToString();
        }
    }
}
