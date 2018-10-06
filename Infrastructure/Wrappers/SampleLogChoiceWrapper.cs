namespace Infrastructure.Wrappers
{
    public class SampleLogChoiceWrapper
    {
        #region Constructors

        public SampleLogChoiceWrapper(string longText,
                                    string code,
                                    int archiveModifier,
                                    int longTermModifier)
        {
            Code = code;
            LongText = longText;
            ArchiveModifier = archiveModifier;
            LongTermModifier = longTermModifier;
        }

        #endregion Constructors

        #region Properties

        public int ArchiveModifier { get; }

        public string Code { get; }

        public int LongTermModifier { get; }

        public string LongText { get; }

        #endregion Properties
    }
}