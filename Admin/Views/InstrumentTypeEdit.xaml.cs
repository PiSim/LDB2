using LabDbContext;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Logica di interazione per InstrumentTypeEdit.xaml
    /// </summary>
    public partial class InstrumentTypeEdit : UserControl, INavigationAware
    {
        #region Constructors

        public InstrumentTypeEdit()
        {
            InitializeComponent();
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
            (DataContext as ViewModels.InstrumentTypeEditViewModel).InstrumentTypeInstance
                = ncontext.Parameters["ObjectInstance"] as InstrumentType;
        }

        #endregion Methods
    }
}