using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports
{
    class TestWrapper : BindableBase
    {
        private bool _canModify;
        private Test _testInstance;

        public TestWrapper(Test instance) : base()
        {
            _canModify = !instance.IsComplete;
            _testInstance = instance;
        }

        public bool CanModify
        {
            get { return _canModify; }
        }

        public string CompletionDate
        {
            get { return _testInstance.Date.ToString(); }
        }

        public bool IsComplete
        {
            get { return _testInstance.IsComplete; }
            set
            {
                _testInstance.IsComplete = value;
                _testInstance.Date = DateTime.Now.Date;
                OnPropertyChanged("CompletionDate");
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

        public ICollection<SubTest> SubTests
        {
            get { return _testInstance.SubTests; }
        }

    }
}
