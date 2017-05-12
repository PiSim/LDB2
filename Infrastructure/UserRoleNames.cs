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
        public static string ExternalReportAdmin { get { return "EXREPORT_ADMIN"; } }
        public static string ReportAdmin { get { return "REPORT_ADMIN"; } }
        public static string ReportEdit { get { return "REPORT_EDIT"; } }
        public static string SpecificationAdmin { get { return "SPEC_ADMIN"; } }
        public static string TaskAdmin { get { return "TASK_ADMIN"; } }
        public static string TaskEdit { get { return "TASK_EDIT"; } }
    }
}
