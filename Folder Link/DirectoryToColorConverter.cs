using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Folder_Link
{
    public class DirectoryToColorConverter: IValueConverter 
    {
        private static Dictionary<string, int> colorAssignment = new Dictionary<string, int>();
        private static Color[] colorsCollection=new Color[]{
                                                            Colors.LightBlue,
                                                            Colors.LightPink,
                                                            Colors.LightSalmon,
                                                            Colors.LightGreen,
                                                            Colors.Orange
                                                            };



        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                throw new ArgumentNullException();
            value = ((DirectoryInfo)value).FullName;

            if (!colorAssignment.ContainsKey((string)value))
            {
                colorAssignment.Add((string)value, colorAssignment.Count);
            }
            int colorIndex = colorAssignment[(string)value];
            Color assignedColor = colorsCollection[colorIndex % colorsCollection.Length];


            return new SolidColorBrush(assignedColor);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
