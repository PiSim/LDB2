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
        void ApplyControlPlan(IEnumerable<ISelectableRequirement> reqList, ControlPlan conPlan);
        List<Requirement> GenerateRequirementList(SpecificationVersion version);
        List<Test> GenerateTestList(List<ISelectableRequirement> reqList);
    }
}
