namespace LInst
{
    public class UserRoleMapping
    {
        #region Properties

        public int ID { get; set; }

        public bool IsSelected { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
        public UserRole UserRole { get; set; }
        public int UserRoleID { get; set; }

        #endregion Properties
    }
}