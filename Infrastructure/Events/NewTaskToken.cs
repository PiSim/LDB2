using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public class NewTaskToken
    {
        private readonly Batch _batch;
        private readonly Project _project;
        private readonly Specification _specification;
        private readonly SpecificationVersion _specVersion;

        public NewTaskToken()
        {

        }

        public NewTaskToken(Batch batch)
        {
            _batch = batch;
        }

        public NewTaskToken(Project prj)
        {
            _project = prj;
        }
        
        public NewTaskToken(Specification spec)
        {
            _specification = spec;
        }

        public NewTaskToken(SpecificationVersion specVersion)
        {
            _specVersion = specVersion;
        }

        public Batch Batch
        {
            get { return _batch; }
        }      

        public Project Project
        {
            get { return _project; }
        }

        public Specification Specification
        {
            get { return _specification; }
        }

        public SpecificationVersion SpecificationVersion
        {
            get { return _specVersion; }
        }
    }
}
