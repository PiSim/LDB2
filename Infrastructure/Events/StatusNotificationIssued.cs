using Prism.Events;

namespace Infrastructure.Events
{
    public class StatusNotificationIssued : PubSubEvent<string>
    {
    }
}