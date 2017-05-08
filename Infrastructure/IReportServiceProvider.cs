using DBManager;
using Infrastructure.Wrappers;
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
        void ApplyControlPlan(IEnumerable<ReportItemWrapper> reqList, ControlPlan conPlan);
        List<Test> GenerateTestList(List<ISelectableRequirement> reqList);
    }
}
