using DataAccess;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Admin.ViewModels
{
    public class InstrumentUtilizationAreaMainViewModel : BindableBase
    {
        #region Fields

        private IDataService<LabDbEntities> _labDbData;
        private InstrumentUtilizationArea _selectedArea;

        #endregion Fields

        #region Constructors

        public InstrumentUtilizationAreaMainViewModel(IDataService<LabDbEntities> labDbData)
        {
            _labDbData = labDbData;

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

                        newArea.Create();

                        RaisePropertyChanged("UtilizationAreaList");
                    }
                });

            DeleteAreaCommand = new DelegateCommand(
                () =>
                {
                    _selectedArea.Delete();
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

        public IEnumerable<InstrumentUtilizationArea> UtilizationAreaList => _labDbData.RunQuery(new InstrumentUtilizationAreasQuery()).ToList();

        #endregion Properties
    }
}