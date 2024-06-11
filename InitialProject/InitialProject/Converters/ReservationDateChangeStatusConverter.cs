using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InitialProject.Converters
{
    public class ReservationDateChangeStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case RequestStatus.ON_HOLD:
                    return "On hold";
                case RequestStatus.ACCEPTED:
                    return "Accepted";
                case RequestStatus.DECLINED:
                    return "Declined";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
