using Prism.Events;

namespace Infrastructure.Events
{
    public class StandardChanged : PubSubEvent<EntityChangedToken>
    {
    }
}