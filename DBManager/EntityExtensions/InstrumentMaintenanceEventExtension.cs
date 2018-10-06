namespace LabDbContext.EntityExtensions
{
    public static class InstrumentMaintenanceEventExtension
    {
        #region Methods

        public static void Create(this InstrumentMaintenanceEvent entry)
        {
            //Inserts a new InstrumentMaintenanceEvent entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.InstrumentMaintenanceEvents.Add(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}