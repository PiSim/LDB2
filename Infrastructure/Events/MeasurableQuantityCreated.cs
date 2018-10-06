using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class MeasurableQuantityCreated : PubSubEvent<MeasurableQuantity>
    {
    }
}