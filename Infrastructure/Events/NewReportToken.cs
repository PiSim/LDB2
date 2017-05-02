using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public class NewReportToken
    {
        private Batch _batch;

        public NewReportToken()
        {

        }

        public NewReportToken(Batch target)
        {
            _batch = target;
        }

        public Batch TargetBatch
        {
            get { return _batch; }
        }
    }
}
