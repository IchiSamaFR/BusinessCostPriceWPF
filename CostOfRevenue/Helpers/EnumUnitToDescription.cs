using CostOfRevenue.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CostOfRevenue.Helpers
{
    internal class EnumUnitToDescription : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Enums.Unit)
            {
                switch ((Enums.Unit)value)
                {
                    case Enums.Unit.kilogram:
                        return "Kilogramme";
                    case Enums.Unit.liter:
                        return "Litre";
                    case Enums.Unit.piece:
                        return "Pièce";
                    case Enums.Unit.dozen:
                        return "Douzaine";
                    default:
                        break;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is not String enumString)
            {
                throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");
            }

            return Enum.Parse(typeof(Wpf.Ui.Appearance.ThemeType), enumString);
        }
    }
}
