using Prism.Events;

namespace Infrastructure.Events
{
    public class ColorChanged : PubSubEvent<EntityChangedToken>
    {
    }
}