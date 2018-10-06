namespace LabDbContext
{
    public partial class ControlPlanItem
    {
        #region Methods

        public void Create()
        {
            // Inserts the Instance in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.ControlPlanItems.Add(this);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}