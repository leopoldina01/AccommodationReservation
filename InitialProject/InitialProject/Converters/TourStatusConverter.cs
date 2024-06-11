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
    public class TourStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case TourStatus.NOT_STARTED:
                    return "Not started";
                case TourStatus.ACTIVE:
                    return "Active";
                case TourStatus.FINISHED:
                    return "Finished";
                case TourStatus.CANCELED:
                    return "Canceled";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
