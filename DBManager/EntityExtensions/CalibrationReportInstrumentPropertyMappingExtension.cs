using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class CalibrationReportInstrumentPropertyMappingExtension
    {
        public static void Update(this CalibrationReportInstrumentPropertyMapping entry)
        {
            // Updates a CAlibrationReportInstrumentPropertyMapping entry

            using (DBEntities entities = new DBEntities())
            {
                entities.CalibrationReportInstrumentPropertyMappings.AddOrUpdate(entry);

                entities.SaveChanges();
            }

        }
    }
}
