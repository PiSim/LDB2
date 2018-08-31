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

        public double Duration
        {
            get
            {
                return _testInstance.Duration;
            }
        }
        
        public string Method
        {
            get { return _testInstance.MethodVariant.StandardName; }
        }

        public string Notes
        {
            get { return _testInstance.Notes; }
            set { _testInstance.Notes = value; }
        }

        public string Property
        {
            get { return _testInstance.MethodVariant.PropertyName; }
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
