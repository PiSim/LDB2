using DBManager;
using Prism.Events;

namespace Infrastructure.Events
{
    public class TaskCreated : PubSubEvent<Task>
    {
    }
}
