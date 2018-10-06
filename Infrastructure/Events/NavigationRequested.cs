using Prism.Events;

namespace Infrastructure.Events
{
    public class NavigationRequested : PubSubEvent<NavigationToken>
    {
    }
}