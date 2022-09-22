using LabelsNow.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace LabelsNow.Helpers
{
    public static class NamedColorHelper
    {
        public static Task<List<NamedColor>> GetNamedColors()
        {
            List<NamedColor> namedColors = new List<NamedColor>();

            // Use reflection to get all the properties of the Colors class.
            IEnumerable<PropertyInfo> propertyInfos = typeof(Colors).GetRuntimeProperties();

            var colors = propertyInfos.Select(c => new
            {
                ColorName = c.Name,
                SolidColorBrush = new SolidColorBrush((Color)c.GetValue(null))
            });

            foreach (var color in colors)
            {
                namedColors.Add(new NamedColor
                {
                    ColorName = color.ColorName,
                    SolidColorBrush = color.SolidColorBrush
                });
            }

            return Task.FromResult(namedColors.ToList());
        }
    }
}
