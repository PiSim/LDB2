namespace Admin
{
    public class ScriptBase
    {
        #region Fields

        protected string _name,
                        _description;

        #endregion Fields

        #region Properties

        public string Description => _description;
        public string Name => _name;

        #endregion Properties

        #region Methods

        public virtual void Run()
        {
        }

        #endregion Methods
    }
}