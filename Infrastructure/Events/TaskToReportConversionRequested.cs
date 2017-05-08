using Prism.Events;

namespace Infrastructure.Events
{
    public class TaskToReportConversionRequested : PubSubEvent<DBManager.Task>
    {
        
    }
}
