namespace Infrastructure
{
    public static class UserSettings
    {
        #region Properties

        public static string CalibrationReportPath => Properties.Settings.Default.CalibrationReportPath;
        public static string ExternalReportPath => Properties.Settings.Default.ExternalReportPath;
        public static string ReportPath => Properties.Settings.Default.ReportPath;

        #endregion Properties
    }
}