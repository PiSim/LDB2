using Infrastructure.Tokens;
using Prism.Events;

namespace Infrastructure.Events
{
    public class TaskCreationRequested : PubSubEvent<NewTaskToken>
    {
    }
}
