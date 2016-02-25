using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceCalculator.Converters
{
    class DecimalPercentageConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                      System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(double) || value == null)
                return value;

            string str = value.ToString();

            if (String.IsNullOrWhiteSpace(str))
                return 0M;

            str = str.TrimEnd(culture.NumberFormat.PercentSymbol.ToCharArray());

            decimal result = 0M;
            if (decimal.TryParse(str, out result))
            {
                result /= 100;
            }

            return result;
        }
    }
}
