using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBEntities : LabDBEntities
    {
        public Sample CreateSampleForBatch(Batch batch, string actionCode)
        {
            Sample output = new Sample();
            output.batch = batch;
            output.date = DateTime.Now;
            output.code = actionCode;
            Samples.Add(output);
            return output;
        }

        public Batch GetBatchByNumber(string number, bool new_instance_if_none_found = true)
        {
            Batch output = base.Batches.FirstOrDefault(bb => bb.Number == number);

            if (output == null && new_instance_if_none_found)
            {
                output = new Batch();
                output.Number = number;
                Batches.Add(output);
            }

            return output;
        }
    }
}
