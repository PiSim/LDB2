using Prism.Events;

namespace Infrastructure.Events
{
    public class AspectChanged : PubSubEvent<EntityChangedToken>
    {
    }
}