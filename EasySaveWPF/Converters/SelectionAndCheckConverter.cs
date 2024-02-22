using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasySaveWPF.Converters;

public class SelectionAndCheckConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] == null || values[1] == null) return Visibility.Collapsed;

        var isSelected = (bool)values[0];
        var isChecked = values[1] as bool?;

        return isSelected || isChecked == true ? Visibility.Visible : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
