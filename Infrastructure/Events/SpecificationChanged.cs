using Prism.Events;

namespace Infrastructure.Events
{
    public class SpecificationChanged : PubSubEvent<EntityChangedToken>
    {
    }
}