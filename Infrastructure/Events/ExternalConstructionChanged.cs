using Prism.Events;

namespace Infrastructure.Events
{
    public class ExternalConstructionChanged : PubSubEvent<EntityChangedToken>
    {
    }
}