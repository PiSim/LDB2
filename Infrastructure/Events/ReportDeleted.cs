using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class ReportDeleted : PubSubEvent<Report>
    {
    }
}