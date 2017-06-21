using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class TaskExtension
    {

        public static void Create(this Task entry)
        {
            // Deletes given task instance

            using (DBEntities entities = new DBEntities())
            {
                entities.Tasks.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Task entry)
        {
            // Deletes given task instance

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.Tasks
                        .First(tsk => tsk.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
            }
        }

        public static void Load(this Task entry)
        {
            // Loads all relevant Related Entities for given task instance

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                Task tempEntry = entities.Tasks.Include(tsk => tsk.Batch.Material.Aspect)
                                                .Include(tsk => tsk.Batch.Material.MaterialLine)
                                                .Include(tsk => tsk.Batch.Material.MaterialType)
                                                .Include(tsk => tsk.Batch.Material.Project.Oem)
                                                .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                                .Include(tsk => tsk.Batch.Material.Recipe.Master)
                                                .Include(tsk => tsk.Requester)
                                                .Include(tsk => tsk.SpecificationVersion.Specification.Standard.CurrentIssue)
                                                .Include(tsk => tsk.SpecificationVersion.Specification.Standard.Organization)
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.SubTaskItems))
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Test.Report))
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Requirement.Method.Standard))
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Requirement.Method.Property))
                                                .First(tsk => tsk.ID == entry.ID);

                entry.AllItemsAssigned = tempEntry.AllItemsAssigned;
                entry.Batch = tempEntry.Batch;
                entry.batchID = tempEntry.batchID;
                entry.EndDate = tempEntry.EndDate;
                entry.IsComplete = tempEntry.IsComplete;
                entry.Notes = tempEntry.Notes;
                entry.PipelineOrder = tempEntry.PipelineOrder;
                entry.PriorityModifier = tempEntry.PriorityModifier;
                entry.Progress = tempEntry.Progress;
                entry.Reports = tempEntry.Reports;
                entry.Requester = tempEntry.Requester;
                entry.RequesterID = tempEntry.RequesterID;
                entry.SpecificationVersion = tempEntry.SpecificationVersion;
                entry.SpecificationVersionID = tempEntry.SpecificationVersionID;
                entry.StartDate = tempEntry.StartDate;
                entry.TaskItems = tempEntry.TaskItems;
            }
        }

        public static void SetAssigned(this Task entry,
                                        bool isAssigned)
        {
            // Sets the Assignment flag of a Task entry

            using (DBEntities entities = new DBEntities())
            {
                entry.AllItemsAssigned = isAssigned;
                entities.Tasks.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
        
        public static void SetComplete(this Task entry,
                                        bool isComplete)
        {
            // Sets the Assignment flag of a Task entry

            using (DBEntities entities = new DBEntities())
            {
                entry.IsComplete = isComplete;
                entities.Tasks.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        public static void Update(this Task entry)
        {
            // Updates the DB values of a Task entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Tasks.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
    }
}
