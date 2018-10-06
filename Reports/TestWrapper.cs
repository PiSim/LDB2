using LabDbContext;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Reports
{
    public class TestWrapper : BindableBase
    {
        #region Constructors

        public TestWrapper(Test instance) : base()
        {
            TestInstance = instance;
        }

        #endregion Constructors

        #region Properties

        public double Duration => TestInstance.Duration;

        public string Method => TestInstance.MethodVariant.StandardName;

        public string Notes
        {
            get { return TestInstance.Notes; }
            set { TestInstance.Notes = value; }
        }

        public string Property => TestInstance.MethodVariant.PropertyName;

        public List<SubTest> SubTests => TestInstance.SubTests.ToList();

        public Test TestInstance { get; }

        #endregion Properties
    }
}