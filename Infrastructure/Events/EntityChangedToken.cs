namespace Infrastructure.Events
{
    public class EntityChangedToken
    {
        #region Constructors

        public EntityChangedToken(object entity,
                                    EntityChangedAction action)
        {
            Entity = entity;
            Action = action;
        }

        #endregion Constructors

        #region Enums

        public enum EntityChangedAction
        {
            Created,
            Updated,
            Deleted
        }

        #endregion Enums

        #region Properties

        public EntityChangedAction Action { get; }
        public object Entity { get; }

        #endregion Properties
    }
}