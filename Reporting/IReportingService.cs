﻿using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting
{
    public interface IReportingService
    {
        void PrintBatchReport(IEnumerable<Batch> batchList);
    }
}