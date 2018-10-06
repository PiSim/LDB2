using Prism.Events;

namespace Infrastructure.Events
{
    public class MethodChanged : PubSubEvent<EntityChangedToken>
    {
    }
}