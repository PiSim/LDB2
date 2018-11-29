using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LInst
{
    public class InstrumentFile
    {
        public int ID { get; set; }

        public int InstrumentID { get; set; }

        public Instrument Instrument { get; set; }

        public string Path { get; set; }
    }
}
