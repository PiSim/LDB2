using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class InstrumentUtilizationAreaMainViewModel : BindableBase
    {
        private DelegateCommand _createArea,
                                _deleteArea;
        private IDataService _dataService;
        private InstrumentUtilizationArea _selectedArea;

        public InstrumentUtilizationAreaMainViewModel(IDataService dataService)
        {
            _dataService = dataService;

            _createArea = new DelegateCommand(
                () =>
                {
                    Controls.Views.StringInputDialog inputDialog = new Controls.Views.StringInputDialog();
                    inputDialog.Message = "Inserire il nome della nuova area:";

                    if (inputDialog.ShowDialog() == true)
                    {
                        InstrumentUtilizationArea newArea = new InstrumentUtilizationArea()
                        {
                            Name = inputDialog.InputString,
                            Plant = "1"
                        };

                        newArea.Create();

                        RaisePropertyChanged("UtilizationAreaList");
                    }

                });

            _deleteArea = new DelegateCommand(
                () =>
                {
                    _selectedArea.Delete();
                    SelectedArea = null;
                },
                () => _selectedArea != null);
        }

        public DelegateCommand CreateAreaCommand
        {
            get { return _createArea; }
        }

        public DelegateCommand DeleteAreaCommand
        {
            get { return _deleteArea; }
        }

        public InstrumentUtilizationArea SelectedArea
        {
            get { return _selectedArea; }
            set
            {
                _selectedArea = value;
                _deleteArea.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<InstrumentUtilizationArea> UtilizationAreaList => _dataService.GetInstrumentUtilizationAreas();
    }
}
