using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tokens
{
    public class NewTaskToken
    {
        private readonly Batch _batch;
        private readonly Person _requester;
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
        
        public Batch Batch
        {
            get { return _batch; }
        }      

        public Project Project
        {
            get { return _project; }
        }

        public Person Requester
        {
            get { return _requester; }
        }
    }
}
