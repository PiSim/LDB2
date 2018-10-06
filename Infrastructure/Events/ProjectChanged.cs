using Prism.Events;

namespace Infrastructure.Events
{
    public class ProjectChanged : PubSubEvent<EntityChangedToken>
    {
    }
}