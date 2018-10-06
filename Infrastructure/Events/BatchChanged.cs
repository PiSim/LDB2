using Prism.Events;

namespace Infrastructure.Events
{
    public class BatchChanged : PubSubEvent<EntityChangedToken>
    {
    }
}