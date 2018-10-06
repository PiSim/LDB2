namespace LabDbContext.EntityExtensions
{
    public static class MeasurementUnitExtension
    {
        #region Methods

        public static void Create(this MeasurementUnit entry)
        {
            // Inserts a MeasurementUnit entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.MeasurementUnits.Add(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}