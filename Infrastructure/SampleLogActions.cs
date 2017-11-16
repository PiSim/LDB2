using Infrastructure.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class SampleLogActions
    {
        private static readonly IEnumerable<SampleLogChoiceWrapper> _choiceList = new List<SampleLogChoiceWrapper>()
        {
            new SampleLogChoiceWrapper("Arrivato in laboratorio",
                                        "A",
                                        1,
                                        0),
            new SampleLogChoiceWrapper("Buttato",
                                        "B",
                                        -1,
                                        0),
            new SampleLogChoiceWrapper("Finito",
                                        "F",
                                        -1,
                                        0),
            new SampleLogChoiceWrapper("Portato in Cotex",
                                        "C",
                                        -1,
                                        1),
            new SampleLogChoiceWrapper("Ripreso da Cotex",
                                        "R",
                                        1,
                                        -1)
        };

        public static IEnumerable<SampleLogChoiceWrapper> ActionList => _choiceList;
    }
}
