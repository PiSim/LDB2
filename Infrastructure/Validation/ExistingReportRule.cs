using DBManager;
using DBManager.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Infrastructure.Validation
{
    public class ExistingReportRule : ValidationRule
    {
        public ExistingReportRule()
        {

        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Report tempReport;
            int repNumber;
            int.TryParse((string)value, out repNumber);

            if (repNumber == 0)
                return new ValidationResult(false, "Numero Report non valido");

            tempReport = ReportService.GetReportByNumber(repNumber);

            if (tempReport == null)
                return new ValidationResult(true, "");

            else
                return new ValidationResult(false, "Esiste già un report con questo numero");
        }
    }
}
