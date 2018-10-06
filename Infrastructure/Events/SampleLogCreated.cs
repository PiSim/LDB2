using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class SampleLogCreated : PubSubEvent<Sample>
    {
    }
}