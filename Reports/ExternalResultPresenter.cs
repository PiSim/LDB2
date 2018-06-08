using DBManager;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Reports
{
    public class ExternalResultPresenter
    {
        private Method _method;
        private Dictionary<int, Test> _resultDictionary;

        public ExternalResultPresenter(Method mtd,
                                        IEnumerable<Test> testList)
        {
            _method = mtd;
            _resultDictionary = new Dictionary<int, Test>();
            foreach (Test tst in testList)
                _resultDictionary.Add(tst.TestRecord.BatchID,
                                    tst);
        }

        public string MethodName => _method.Name;

        /// Named "SubTests" for consistency among Datagrids used to display
        /// the test results
        public IEnumerable<SubMethod> SubTests => _method.SubMethods.ToList();

        public Test this[int batchID] => _resultDictionary[batchID];  
    }
    
    public class ExternalResultPresenterTestExtractor : IValueConverter
    {
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
    }
}
