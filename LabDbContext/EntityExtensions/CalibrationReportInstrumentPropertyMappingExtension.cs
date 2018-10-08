using System.Data.Entity.Migrations;

namespace LabDbContext.EntityExtensions
{
    public static class CalibrationReportInstrumentPropertyMappingExtension
    {
        #region Methods

        public static void Update(this CalibrationReportInstrumentPropertyMapping entry)
        {
            // Updates a CAlibrationReportInstrumentPropertyMapping entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.CalibrationReportInstrumentPropertyMappings.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}