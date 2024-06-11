using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using InitialProject.Localization;

namespace InitialProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public App()
        {
            SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt+QHJqXE1hXk5Hd0BLVGpAblJ3T2ZQdVt5ZDU7a15RRnVfRF1kSX1WcUZiX3xYeA==;Mgo+DSMBPh8sVXJ1S0R+VVpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jT35UdkZmXnxfdnFVTg==;ORg4AjUWIQA/Gnt2VFhiQlhPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXhTcUVgXHhdcXJTQ2g=;MjM0NTI3MkAzMjMxMmUzMDJlMzBIdGRyZU9qV3NaTjJUOC9seXlBblBENWhYMU5KakJBamxIL2x2aDRDdTEwPQ==;MjM0NTI3M0AzMjMxMmUzMDJlMzBkWHV5Y3pJZlpJRDZxOUZ1Z0VvZ1cwcHRwWHpjazlnM0M1L1ZpeU1yZ0dBPQ==;NRAiBiAaIQQuGjN/V0d+Xk9NfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5VdkJjWnpZc3VSQmRU;MjM0NTI3NUAzMjMxMmUzMDJlMzBqQW8rNitER1ZNV2hBUDlGQ04yMXJoYnpoSzB4d2NvMEYzK2lsU0syOTZFPQ==;MjM0NTI3NkAzMjMxMmUzMDJlMzBhdmdQSmJrT3ZlTlVFWDhLZ2o1WnZzNmhPdzlvcGpoNkxEa0l3Vi9NRTFvPQ==;Mgo+DSMBMAY9C3t2VFhiQlhPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9gSXhTcUVgXHhdcXxXRGY=;MjM0NTI3OEAzMjMxMmUzMDJlMzBtekZZRHYvYlVYUnlzTXhraFptbUNVR3FCZlNCY29uWnFiTVEzaXVWc2ZjPQ==;MjM0NTI3OUAzMjMxMmUzMDJlMzBEbWdxd2JkekE1TnZKZ0tiZEtKZUVsZ3U1bGU4VS9ucVRJd3VxejJ5Q0JzPQ==;MjM0NTI4MEAzMjMxMmUzMDJlMzBqQW8rNitER1ZNV2hBUDlGQ04yMXJoYnpoSzB4d2NvMEYzK2lsU0syOTZFPQ==");
        }

        public ResourceDictionary ThemeDictionary
        {
            // You could probably get it via its name with some query logic as well.
            get { return Resources.MergedDictionaries[0]; }
        }

        public void ChangeTheme(Uri uri)
        {
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }
        public void ChangeLanguage(string lang)
        {
            TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(lang);
        }
    }
}
