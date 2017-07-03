using DBManager;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public class SampleLogCreated : PubSubEvent<Sample>
    {
    }
}
