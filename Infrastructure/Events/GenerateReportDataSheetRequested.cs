using LabDbContext;
using Prism.Events;
using System;

namespace Infrastructure.Events
{
    [Obsolete]
    public class GenerateReportDataSheetRequested : PubSubEvent<Report>
    {
    }
}