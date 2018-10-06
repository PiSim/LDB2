using Infrastructure.Wrappers;
using System.Collections.Generic;

namespace Infrastructure
{
    public static class SampleLogActions
    {
        #region Properties

        public static IEnumerable<SampleLogChoiceWrapper> ActionList { get; } = new List<SampleLogChoiceWrapper>()
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

        #endregion Properties
    }
}