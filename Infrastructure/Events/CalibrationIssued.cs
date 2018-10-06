using LabDbContext;
using Prism.Events;

namespace Infrastructure.Events
{
    public class CalibrationIssued : PubSubEvent<CalibrationReport>
    {
    }
}