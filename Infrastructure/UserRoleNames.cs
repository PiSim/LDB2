using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UserRoleNames
    {
        public static string Admin { get { return "ADMIN"; } }
        public static string BatchAdmin { get { return "BATCH_ADMIN"; } }
        public static string BatchEdit { get { return "BATCH_EDIT"; } }
        public static string ExternalReportAdmin { get { return "EXREPORT_ADMIN"; } }
        public static string ExternalReportView { get { return "EXREPORT_VIEW"; } }
        public static string InstrumentAdmin { get { return "INSTRUMENT_ADMIN"; } }
        public static string InstrumentView { get { return "INSTRUMENT_VIEW"; } }
        public static string MaterialAdmin { get { return "MATERIAL_ADMIN"; } }
        public static string MaterialEdit { get { return "MATERIAL_EDIT"; } }
        public static string ProjectAdmin { get { return "PRJ_ADMIN"; } }
        public static string ProjectEdit { get { return "PRJ_EDIT"; } }
        public static string ProjectView { get { return "PRJ_VIEW"; } }
        public static string ReportAdmin { get { return "REPORT_ADMIN"; } }
        public static string ReportEdit { get { return "REPORT_EDIT"; } }
        public static string ReportView { get { return "REPORT_VIEW"; } }
        public static string SampleEdit { get { return "SAMPLE_EDIT"; } }
        public static string SpecificationAdmin { get { return "SPEC_ADMIN"; } }
        public static string SpecificationEdit { get { return "SPEC_EDIT"; } }
        public static string SpecificationView { get { return "SPEC_VIEW"; } }
        public static string TaskAdmin { get { return "TASK_ADMIN"; } }
        public static string TaskEdit { get { return "TASK_EDIT"; } }
    }
}
