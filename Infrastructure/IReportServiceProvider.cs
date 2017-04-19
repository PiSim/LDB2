﻿using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IReportServiceProvider
    {
        PurchaseOrder AddPOToExternalReport(ExternalReport target);
        void ApplyControlPlan(List<ReportItemWrapper> reqList, ControlPlan conPlan);
    }
}
