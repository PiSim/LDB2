using Prism.Events;

namespace Infrastructure.Events
{
    public class TaskCompleted : PubSubEvent<LabDbContext.Task>
    {
    }
}