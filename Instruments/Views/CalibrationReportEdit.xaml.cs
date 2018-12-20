using Controls.Views;
using LInst;
using Prism.Regions;
using System.Windows.Controls;
using System.Windows.Input;

namespace Instruments.Views
{
    /// <summary>
    /// Logica di interazione per CalibrationReportEdit.xaml
    /// </summary>
    public partial class CalibrationReportEdit : UserControl, INavigationAware
    {
        #region Constructors

        public CalibrationReportEdit(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.CalibrationEditFileListRegion,
                                                typeof(FileListControl));
        }

        #endregion Constructors

        #region Methods

        public bool IsNavigationTarget(NavigationContext ncontext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext ncontext)
        {
        }

        public void OnNavigatedTo(NavigationContext ncontext)
        {
            (DataContext as ViewModels.CalibrationReportEditViewModel).CalibrationInstance =
               ncontext.Parameters["ObjectInstance"] as CalibrationReport;
        }

        private void ReferenceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AddReferenceButton.Command.CanExecute(AddReferenceButton.CommandParameter))
                AddReferenceButton.Command.Execute(AddReferenceButton.CommandParameter);
        }

        #endregion Methods
    }
}