using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class MethodExtension
    {

        public static void AddSubMethod(this Method entry,
                                        SubMethod subMethodEntity)
        {
            // Adds a submethod to a method entry and updates all the related requirements

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.SubMethods.Add(subMethodEntity);

                subMethodEntity.MethodID = entry.ID;

                IEnumerable<Requirement> tempReqList = entities.Requirements.Where(req => req.MethodID == entry.ID)
                                                                            .ToList();

                foreach (Requirement req in tempReqList)
                {
                    SubRequirement tempSR = new SubRequirement();
                    tempSR.RequiredValue = "";
                    tempSR.Requirement = req;
                    subMethodEntity.SubRequirements.Add(tempSR);
                    req.SubRequirements.Add(tempSR);
                }

                entities.SaveChanges();
            }
        }

        public static void Create(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Methods.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Methods.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static IEnumerable<StandardIssue> GetIssues(this Method entry)
        {
            // Returns all Issue entities for a given Method entry

            if (entry == null)
                return new List<StandardIssue>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.StandardIssues.Where(stdi => stdi.StandardID == entry.StandardID)
                                                .ToList();
            }
        }

        public static IEnumerable<Test> GetResults(this Method entry)
        {
            // Returns all Tests for a given Method entry

            if (entry == null)
                return new List<Test>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tests.Include(tst => tst.SubTests)
                                    .Where(tst => tst.MethodID == entry.ID)
                                    .ToList();
            }
        }

        public static IEnumerable<SubMethod> GetSubMethods(this Method entry)
        {
            // Returns All SubMethod entities for a Method

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.SubMethods.Where(smtd => smtd.MethodID == entry.ID)
                                        .ToList();
            }
        }

        public static void Load(this Method entry)
        {
            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                Method tempEntry = entities.Methods.Include(mtd => mtd.AssociatedInstruments
                                                    .Select(inst => inst.InstrumentType))
                                                    .Include(mtd => mtd.ExternalReports
                                                    .Select(extr => extr.ExternalLab))
                                                    .Include(mtd => mtd.Property)
                                                    .Include(mtd => mtd.Standard.CurrentIssue)
                                                    .Include(mtd => mtd.Standard.Organization)
                                                    .Include(mtd => mtd.Tests
                                                    .Select(tst => tst.SubTests))
                                                    .Include(mtd => mtd.Tests
                                                    .Select(tst => tst.Report))
                                                    .First(spec => spec.ID == entry.ID);

                entry.AssociatedInstruments = tempEntry.AssociatedInstruments;
                entry.CostUnits = tempEntry.CostUnits;
                entry.Description = tempEntry.Description;
                entry.ExternalReports = tempEntry.ExternalReports;
                entry.Property = tempEntry.Property;
                entry.PropertyID = tempEntry.PropertyID;
                entry.Requirements = tempEntry.Requirements;
                entry.Standard = tempEntry.Standard;
                entry.StandardID = tempEntry.StandardID;
                entry.SubMethods = tempEntry.SubMethods;
                entry.Tests = tempEntry.Tests;
                entry.UM = tempEntry.UM;
            }
        }

        public static void LoadSubMethods(this Method entry)
        {
            // Loads related sub methods in a given method entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Method tempEntry = entities.Methods.Include(mtd => mtd.SubMethods)
                                                    .First(mtd => mtd.ID == entry.ID);

                entry.SubMethods = tempEntry.SubMethods;
            }
        }

        public static void Update(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Methods.AddOrUpdate(entry);

                foreach (SubMethod smtd in entry.SubMethods)
                    entities.SubMethods.AddOrUpdate(smtd);
                
                entities.SaveChanges();
            }
        }
    }
}
