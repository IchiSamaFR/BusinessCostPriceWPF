using BusinessCostPriceWPF.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BusinessCostPriceWPF.Helpers
{
    internal class EnumUnitToDescription : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool lower = false;
            if(parameter != null)
            {
                lower = parameter.ToString().ToLower() == "true";
            }
            if(value is Enums.Unit)
            {
                switch ((Enums.Unit)value)
                {
                    case Enums.Unit.kilogram:
                        return lower ? "Kg" : "Kilogramme";
                    case Enums.Unit.liter:
                        return lower ? "L" : "Litre";
                    case Enums.Unit.piece:
                        return lower ? "Pce" : "Pièce";
                    case Enums.Unit.dozen:
                        return lower ? "Dz" : "Douzaine";
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
