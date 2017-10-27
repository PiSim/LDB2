using DBManager;
using DBManager.EntityExtensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class ProjectStatsWrapper : BindableBase
    {
        private Project _projectInstance;

        public ProjectStatsWrapper(Project projectInstance)
        {
            _projectInstance = projectInstance;
        }

        public string Description
        {
            get { return _projectInstance.Description; }
        }

        public string Name
        {
            get { return _projectInstance.Name; }
        }

        public double? TotalExternalCost
        {
            get { return _projectInstance.GetPurchaseOrders().Sum(po => po.Total); }
        }

        public double? TotalWorkHours
        {
            get { return _projectInstance.GetTests().Sum(tst => tst.Duration); }
        }
    }
}
