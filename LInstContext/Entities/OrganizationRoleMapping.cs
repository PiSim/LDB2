namespace LInst
{
    public class OrganizationRoleMapping
    {
        #region Properties

        public int ID { get; set; }

        public bool IsSelected { get; set; }
        public Organization Organization { get; set; }
        public int OrganizationID { get; set; }
        public OrganizationRole OrganizationRole { get; set; }
        public int OrganizationRoleID { get; set; }

        #endregion Properties
    }
}