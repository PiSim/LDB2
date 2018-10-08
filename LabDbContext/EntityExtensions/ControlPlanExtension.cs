using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public partial class ControlPlan
    {
        #region Methods

        public void Create()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.ControlPlans.Add(this);
                entities.SaveChanges();
            }
        }

        public void Delete()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                ControlPlan tempEntry = entities.ControlPlans.FirstOrDefault(cp => cp.ID == ID);
                if (tempEntry != null)
                {
                    entities.Entry(tempEntry)
                            .State = System.Data.Entity.EntityState.Deleted;
                    entities.SaveChanges();
                }

                ID = 0;
            }
        }

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

        public void Update()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.ControlPlans.AddOrUpdate(this);
                foreach (ControlPlanItem cpi in control_plan_items_b)
                    entities.ControlPlanItems.AddOrUpdate(cpi);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}