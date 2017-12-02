using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class UserSettings
    {
        public static string CalibrationReportPath => Properties.Settings.Default.CalibrationReportPath;
        public static string ExternalReportPath => Properties.Settings.Default.ExternalReportPath;
        public static string ReportPath => Properties.Settings.Default.ReportPath;
    }
}
