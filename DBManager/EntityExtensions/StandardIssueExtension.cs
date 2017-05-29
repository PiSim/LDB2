using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class StandardIssueExtension
    {
        public static void Create(this StandardIssue entry)
        {
            // Inserts a new StandardIssue Entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.StandardIssues.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this StandardIssue entry)
        {
            // Deletes a StandardIssue Entry from the DB

            using (DBEntities entities = new DBEntities())
            {
                StandardIssue tempEntry = entities.StandardIssues.First(stdi => stdi.ID == entry.ID);
                entities.Entry(tempEntry).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }


        public static void Load(this StandardIssue entry)
        {
            // Loads relevant RelatedEntities for StandardIssue entry

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                StandardIssue tempEntry = entities.StandardIssues.Include(stdi => stdi.StandardFiles)
                                                                .First(stdi => stdi.ID == entry.ID);

                entry.IsCurrent = tempEntry.IsCurrent;
                entry.Issue = tempEntry.Issue;
                entry.Standard = tempEntry.Standard;
                entry.StandardFiles = tempEntry.StandardFiles;
                entry.StandardID = tempEntry.StandardID;
                entities.SaveChanges();
            }

        }
    }
}
