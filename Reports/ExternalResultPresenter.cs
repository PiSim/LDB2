using Infrastructure;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace Reports
{
    public class ExternalResultPresenter
    {
        #region Fields

        private MethodVariant _methodVariant;
        private Dictionary<int, Test> _resultDictionary;
        private IEnumerable<Test> _testList;

        #endregion Fields

        #region Constructors

        public ExternalResultPresenter(MethodVariant methodVariant,
                                        IEnumerable<Test> testList)
        {
            _methodVariant = methodVariant;
            _testList = testList;
            _resultDictionary = new Dictionary<int, Test>();
            foreach (Test tst in _testList)
                _resultDictionary.Add(tst.TestRecord.BatchID,
                                    tst);
        }

        #endregion Constructors

        #region Properties

        public string MethodName => _methodVariant.StandardName;

        public string MethodVariantName => _methodVariant.Name;

        /// Named "SubTests" for consistency among Datagrids used to display
        /// the test results
        public IEnumerable<SubMethod> SubTests => _methodVariant?.Method?.SubMethods.ToList();

        public IEnumerable<Test> TestList => _testList;

        #endregion Properties

        #region Indexers

        public Test this[int batchID] => _resultDictionary[batchID];

        #endregion Indexers
    }

    public class ExternalResultPresenterTestExtractor : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() != typeof(DataGridCell))
                throw new InvalidOperationException();

            DataGridCell _value = (DataGridCell)value;

            ExternalResultPresenter context = _value.DataContext as ExternalResultPresenter;

            int BatchID = (int)DataGridColumnTagExtension.GetTag(_value.Column);

            return context[BatchID].SubTests.ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}