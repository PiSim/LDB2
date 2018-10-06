namespace Infrastructure.Events
{
    public class NavigationToken
    {
        #region Constructors

        public NavigationToken(string viewName,
                                object instance = null,
                                string regionName = null)
        {
            RegionName = regionName;

            ObjectInstance = instance;
            ViewName = viewName;
        }

        #endregion Constructors

        #region Properties

        public object ObjectInstance { get; }

        public string RegionName { get; }

        public string ViewName { get; }

        #endregion Properties
    }
}