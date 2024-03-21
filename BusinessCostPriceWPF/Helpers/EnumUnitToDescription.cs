using BusinessCostPriceAPI.Client.Models;
using System.Globalization;
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
            if(value is Unit)
            {
                switch ((Unit)value)
                {
                    case Unit.Kilogram:
                        return lower ? "Kg" : "Kilogramme";
                    case Unit.Liter:
                        return lower ? "L" : "Litre";
                    case Unit.Piece:
                        return lower ? "Pce" : "Pièce";
                    case Unit.Dozen:
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
