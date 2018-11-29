using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure.Queries;
using LInst;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Admin.ViewModels
{
    public class InstrumentUtilizationAreaMainViewModel : BindableBase
    {
        #region Fields

        private IDataService<LInstContext> _lInstData;
        private InstrumentUtilizationArea _selectedArea;

        #endregion Fields

        #region Constructors

        public InstrumentUtilizationAreaMainViewModel(IDataService<LInstContext> lInstData)
        {
            _lInstData = lInstData;

            CreateAreaCommand = new DelegateCommand(
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

                        _lInstData.Execute(new InsertEntityCommand<LInstContext>(newArea));

                        RaisePropertyChanged("UtilizationAreaList");
                    }
                });

            DeleteAreaCommand = new DelegateCommand(
                () =>
                {
                    _lInstData.Execute(new DeleteEntityCommand<LInstContext>(_selectedArea));
                    SelectedArea = null;
                },
                () => _selectedArea != null);
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand CreateAreaCommand { get; }

        public DelegateCommand DeleteAreaCommand { get; }

        public InstrumentUtilizationArea SelectedArea
        {
            get { return _selectedArea; }
            set
            {
                _selectedArea = value;
                DeleteAreaCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<InstrumentUtilizationArea> UtilizationAreaList => _lInstData.RunQuery(new InstrumentUtilizationAreasQuery()).ToList();

        #endregion Properties
    }
}