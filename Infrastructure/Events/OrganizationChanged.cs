using Prism.Events;

namespace Infrastructure.Events
{
    public class OrganizationChanged : PubSubEvent<EntityChangedToken>
    {
    }
}