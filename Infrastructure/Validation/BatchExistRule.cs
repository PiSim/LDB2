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
    public class BatchExistRule : ValidationRule
    {
        public BatchExistRule()
        {

        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string batchNumber = (string)value;

            if (string.IsNullOrEmpty(batchNumber))
                return new ValidationResult(false, "Inserire un numero di batch");

            Batch tempBatch = MaterialService.GetBatch(batchNumber);

            if (tempBatch == null)
                return new ValidationResult(false, "Il batch " + batchNumber + " non esiste");

            else
                return ValidationResult.ValidResult;
        }
    }
}
