using LabDbContext;

namespace Infrastructure.Events
{
    public class NewTaskToken
    {
        #region Constructors

        public NewTaskToken()
        {
        }

        public NewTaskToken(Batch batch)
        {
            Batch = batch;
        }

        public NewTaskToken(Project prj)
        {
            Project = prj;
        }

        public NewTaskToken(Specification spec)
        {
            Specification = spec;
        }

        public NewTaskToken(SpecificationVersion specVersion)
        {
            SpecificationVersion = specVersion;
        }

        #endregion Constructors

        #region Properties

        public Batch Batch { get; }

        public Project Project { get; }

        public Specification Specification { get; }

        public SpecificationVersion SpecificationVersion { get; }

        #endregion Properties
    }
}