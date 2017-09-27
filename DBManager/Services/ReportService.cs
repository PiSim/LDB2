using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class ReportService
    {        
        public static IEnumerable<ExternalReport> GetExternalReports()
        {
            // Returns all external report instances

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalReports.Include(exrep => exrep.ExternalLab)
                                                .OrderBy(exrep => exrep.InternalNumber)
                                                .ToList();
            }
        }

        public static int GetNextReportNumber()
        {
            // Returns the next available ReportNumber

            using (DBEntities entities = new DBEntities())
            {
                return entities.Reports
                                .Max(rep => rep.Number) + 1;
            }
        }

        public static int GetNextExternalReportNumber(int year)
        {
            // Returns the next available ExternalReport number for a given year

            using (DBEntities entities = new DBEntities())
            {
                return entities.ExternalReports
                                .Where(erep => erep.Year == year)
                                .Max(erep => erep.Number) + 1;
            }
        }

        public static Report GetReport(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.First(entry => entry.ID == ID);
            }
        }

        public static Report GetReportByNumber(int number)
        {
            // Returns a report instance with the given number, or null if none exists

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.FirstOrDefault(rep => rep.Number == number);
            }
        }

        public static IEnumerable<Report> GetReports()
        {
            // Returns all Report entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.AsNoTracking()
                                        .Include(rep => rep.Author)
                                        .Include(rep => rep.Batch.Material.Aspect)
                                        .Include(rep => rep.Batch.Material.MaterialLine)
                                        .Include(rep => rep.Batch.Material.MaterialType)
                                        .Include(rep => rep.Batch.Material.Project.Oem)
                                        .Include(rep => rep.Batch.Material.Recipe.Colour)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard.Organization)
                                        .Where(rep => true)
                                        .OrderByDescending(rep => rep.Number)
                                        .ToList();
            }
        }
    }
}
