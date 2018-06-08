using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports
{
    public class TestWrapper : BindableBase
    {
        private Test _testInstance;

        public TestWrapper(Test instance) : base()
        {
            _testInstance = instance;
        }

        public bool CanDelete
        {
            get { return !IsComplete; }
        }

        public string CompletionDate
        {
            get 
            { 
                return (_testInstance.Date == null) ? null : _testInstance.Date.Value.ToShortDateString();
            }
        }

        public double Duration
        {
            get
            {
                return _testInstance.Duration;
            }
        }

        public bool IsComplete
        {
            get { return _testInstance.IsComplete; }
            set
            {
                _testInstance.IsComplete = value;
                if (value)
                    _testInstance.Date = DateTime.Now.Date;
                else
                     _testInstance.Date = null;
                    
                RaisePropertyChanged("CompletionDate");
            }
        }
        
        public string Method
        {
            get { return _testInstance.Method.Standard.Name; }
        }

        public string Notes
        {
            get { return _testInstance.Notes; }
            set { _testInstance.Notes = value; }
        }

        public string Property
        {
            get { return _testInstance.Method.Property.Name; }
        }

        public List<SubTest> SubTests
        {
            get { return _testInstance.SubTests.ToList(); }
        }

        public Test TestInstance
        {
            get { return _testInstance; }
        }

    }
}
