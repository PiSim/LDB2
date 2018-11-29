using LInst;
using Prism.Events;

namespace Infrastructure.Events
{
    public class CalibrationIssued : PubSubEvent<CalibrationReport>
    {
    }
}