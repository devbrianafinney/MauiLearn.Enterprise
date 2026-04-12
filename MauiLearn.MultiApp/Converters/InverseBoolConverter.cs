using CommunityToolkit.Maui.Converters;
using System.Globalization;

namespace MauiLearn.MultiApp.Converters
{
    //public class InverseBoolConverter : ICommunityToolkitMultiValueConverter
    //{

        //     public object Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
        //         => value is bool b ? !b : false;
        //     object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
        //         => value is bool b ? !b : false;
   // }



public class InverseBoolConverter : ICommunityToolkitMultiValueConverter
    {
        public object[]? ConvertBack(object? value, Type[] targetTypes, object? parameter, CultureInfo? culture)
        {
            // Invert the boolean if possible, otherwise default to false
            bool inverted = value is bool b && b == true ? false : true;

            // Return the inverted value for each target type
            var results = new object[targetTypes.Length];
            for (int i = 0; i < targetTypes.Length; i++)
            {
                results[i] = inverted;
            }

            return results;
        }

        public object? Convert(object[]? values, Type targetType, object? parameter, CultureInfo? culture)
        {
            // Example: just return the first value
            return values != null && values.Length > 0 ? values[0] : null;
        }
    }

}
