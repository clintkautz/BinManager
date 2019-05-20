using System;
using System.Drawing;
using Esri.ArcGISRuntime.Mapping;

namespace BinManager.Settings
{
    public static class GlobalSettings
    {
        public static string EmployeeNum;
        public static Viewpoint UpdateViewpoint;

        public static string EmployeeNumRequiredMsg = "Employee # is required";
        public static string InvalidUserPassMsg = "Invalid username/password";
        public static string NewEntryPageTitle = "New Entry";
        public static string MapViewPageTitle = "Map View";
    }
}
