using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class InstrumentTypeCreated : PubSubEvent<InstrumentType>
    {
    }
}