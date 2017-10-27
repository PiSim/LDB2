using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Wrappers
{
    public class SampleLogChoiceWrapper
    {
        private string _code,
                        _longText;
        private int _archiveModifier,
                    _longTermModifier;

        public SampleLogChoiceWrapper(string longText,
                                    string code,
                                    int archiveModifier,
                                    int longTermModifier)
        {
            _code = code;
            _longText = longText;
            _archiveModifier = archiveModifier;
            _longTermModifier = longTermModifier;
        }

        public int ArchiveModifier
        {
            get { return _archiveModifier; }
        }

        public string Code
        {
            get { return _code; }
        }

        public int LongTermModifier
        {
            get { return _longTermModifier; }
        }

        public string LongText
        {
            get { return _longText; }
        }
    }
}
