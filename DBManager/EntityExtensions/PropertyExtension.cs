namespace LabDbContext
{
    public partial class Property
    {
        #region Methods

        /// <summary>
        /// Inserts the entity in the DB as a new entry
        /// </summary>
        public void Create()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Properties.Add(this);
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}