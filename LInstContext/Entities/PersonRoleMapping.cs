namespace LInst
{
    public class PersonRoleMapping
    {
        #region Properties

        public int ID { get; set; }

        public bool IsSelected { get; set; }
        public Person Person { get; set; }
        public int PersonID { get; set; }
        public PersonRole Role { get; set; }
        public int RoleID { get; set; }

        #endregion Properties
    }
}