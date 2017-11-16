

namespace Infrastructure.Events
{
    public class EntityChangedToken
    {
        public enum EntityChangedAction
        {
            Created,
            Updated,
            Deleted
        }

        private object _entity;
        private EntityChangedAction _action;

        public EntityChangedToken(object entity,
                                    EntityChangedAction action)
        {
            _entity = entity;
            _action = action;
        }

        public object Entity => _entity;

        public EntityChangedAction Action => _action;
    }
}
