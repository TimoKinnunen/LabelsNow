using LabelsNow.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Text;

namespace LabelsNow.Helpers
{
    public static class NamedFontWeightHelper
    {
        public static Task<List<NamedFontWeight>> GetNamedFontWeights()
        {
            List<NamedFontWeight> namedFontWeights = new List<NamedFontWeight>();

            // Use reflection to get all the properties of the Colors class.
            IEnumerable<PropertyInfo> propertyInfos = typeof(FontWeights).GetRuntimeProperties();

            var fontweights = propertyInfos.Select(c => new
            {
                FontWeightName = c.Name,
                FontWeight = (FontWeight)c.GetValue(null)
            });

            foreach (var fontweight in fontweights)
            {
                namedFontWeights.Add(new NamedFontWeight
                {
                    FontWeightName = fontweight.FontWeightName,
                    FontWeight = fontweight.FontWeight
                });
            }

            return Task.FromResult(namedFontWeights.ToList());
        }
    }
}
