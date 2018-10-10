using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public partial class ControlPlan
    {
        #region Methods
        

        public IList<ControlPlanItem> GetControlPlanItems(bool includeRequirement = false)
        {
            // Returns all the items in a Control Plan

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                if (includeRequirement)
                    return entities.ControlPlanItems
                                    .Include(cpi => cpi.Requirement.MethodVariant.Method.Property)
                                    .Include(cpi => cpi.Requirement.MethodVariant.Method.Standard)
                                    .Where(cpi => cpi.ControlPlanID == ID)
                                    .ToList();
                else
                    return entities.ControlPlanItems
                                    .Where(cpi => cpi.ControlPlanID == ID)
                                    .ToList();
            }
        }

        #endregion Methods
    }
}