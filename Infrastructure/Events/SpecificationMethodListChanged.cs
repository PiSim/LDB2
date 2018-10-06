using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class SpecificationMethodListChanged : PubSubEvent<Specification>
    {
    }
}