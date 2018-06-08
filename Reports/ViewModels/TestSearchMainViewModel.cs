using DBManager;
using Infrastructure.Queries;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    public class TestSearchMainViewModel : BindableBase
    {
        DelegateCommand _runQuery;
        DataAccessService _dataService;
        IEnumerable<Test> _resultList;
        string _methodName;

        public TestSearchMainViewModel(DataAccessService dataService)
        {
            _dataService = dataService;

            _runQuery = new DelegateCommand(
                () =>
                {
                    IQuery<Test> testQuery = new TestQuery()
                    {
                        MethodName = _methodName
                    };

                    _resultList = _dataService.GetQueryResults(testQuery);
                    OnPropertyChanged("ResultList");
                });
        }

        public DelegateCommand RunQueryCommand => _runQuery;

        public IEnumerable<Test> ResultList => _resultList;

        public string MethodName
        {
            get => _methodName;
            set => _methodName = value;
        }
    }
}
