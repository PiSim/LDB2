using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class MaintenanceEventCreated : PubSubEvent<InstrumentMaintenanceEvent>
    {
    }
}