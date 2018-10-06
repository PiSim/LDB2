using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class ReportCompleted : PubSubEvent<Report>
    {
    }
}