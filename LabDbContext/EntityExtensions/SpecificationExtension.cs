using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class SpecificationExtension
    {
        #region Methods
        [Obsolete]
        public static IEnumerable<StandardFile> GetFiles(this Specification entry)
        {
            // Returns all standard files for specification standard

            if (entry == null)
                return new List<StandardFile>();

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.StandardFiles
                                .Where(stdf => stdf.StandardID == entry.StandardID)
                                .ToList();
            }
        }

        #endregion Methods
    }

    public partial class Specification
    {

        #region Methods
        [Obsolete]
        public ControlPlan AddControlPlan(bool asDefault = false)
        {
            // Generates a new control plan for this specification

            using (LabDbEntities entities = new LabDbEntities())
            {
                ControlPlan newEntry = new ControlPlan()
                {
                    IsDefault = asDefault,
                    Name = (asDefault) ? "Completo" : "Nuovo Piano di Controllo",
                    SpecificationID = ID
                };

                SpecificationVersion mainVersion = entities.SpecificationVersions
                                                            .FirstOrDefault(spv => spv
                                                            .SpecificationID == ID &&
                                                            spv.IsMain);

                if (mainVersion == null)
                    throw new InvalidOperationException();

                foreach (Requirement req in mainVersion.Requirements)
                    newEntry.control_plan_items_b.Add(new ControlPlanItem()
                    {
                        IsSelected = asDefault,
                        RequirementID = req.ID
                    });

                entities.ControlPlans.Add(newEntry);

                entities.SaveChanges();

                return newEntry;
            }
        }

        public void AddMethod(Requirement requirementEntry)
        {
            // Adds a requirement to a Specification

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.SpecificationVersions
                        .First(spcv => spcv.SpecificationID == ID && spcv.IsMain)
                        .Requirements
                        .Add(requirementEntry);

                foreach (ControlPlan cp in entities.ControlPlans.Where(cp => cp.SpecificationID == ID))
                    requirementEntry.ControlPlanItems
                                    .Add(new ControlPlanItem()
                                    {
                                        ControlPlan = cp,
                                        IsSelected = cp.IsDefault
                                    });

                entities.SaveChanges();
            }
        }

        public IList<ControlPlan> GetControlPlans()
        {
            // Returns all existing control plans for a specification

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ControlPlans
                                .Where(cp => cp.SpecificationID == ID)
                                .ToList();
            }
        }

        public IList<SpecificationVersion> GetVersions()
        {
            //Returns all version for a given specification entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.SpecificationVersions.Where(specv => specv.SpecificationID == ID)
                                                    .ToList();
            }
        }

        #endregion Methods
    }
}