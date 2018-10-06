using Prism.Events;

namespace Infrastructure.Events
{
    public class ExternalReportChanged : PubSubEvent<EntityChangedToken>
    {
    }
}