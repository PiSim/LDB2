using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class ControlPlan
    {

        public void Create()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ControlPlans.Add(this);
                entities.SaveChanges();
            }
        }

        public void Delete()
        {
            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                if (includeRequirement)
                    return entities.ControlPlanItems
                                    .Include(cpi => cpi.Requirement.Method.Property)
                                    .Include(cpi => cpi.Requirement.Method.Standard)
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
            using (DBEntities entities = new DBEntities())
            {
                entities.ControlPlans.AddOrUpdate(this);
                foreach (ControlPlanItem cpi in control_plan_items_b)
                    entities.ControlPlanItems.AddOrUpdate(cpi);

                entities.SaveChanges();
            }
        }
    }
}
