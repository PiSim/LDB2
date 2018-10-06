using Prism.Events;

namespace Infrastructure.Events
{
    public class PersonChanged : PubSubEvent<EntityChangedToken>
    {
    }
}