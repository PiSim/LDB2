using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class ReportCreated : PubSubEvent<Report>
    {
    }
}