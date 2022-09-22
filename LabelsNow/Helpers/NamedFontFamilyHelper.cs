using LabelsNow.Models;
using Microsoft.Graphics.Canvas.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace LabelsNow.Helpers
{
    public sealed class NamedFontFamilyHelper
    {
        public static Task<List<NamedFontFamily>> GetNamedFontFamilies()
        {
            List<NamedFontFamily> namedFontFamilies = new List<NamedFontFamily>();

            // NuGet package Win2D.uwp
            List<string> fontFamilies = CanvasTextFormat.GetSystemFontFamilies().OrderBy(f => f).ToList();

            foreach (var fontFamilyName in fontFamilies)
            {
                namedFontFamilies.Add(new NamedFontFamily
                {
                    PaperFontFamilyName = fontFamilyName,
                    FontFamily = new FontFamily(fontFamilyName)
                });
            }

            return Task.FromResult(namedFontFamilies);
        }
    }
}
