using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Logica di interazione per MeasurableQuantityEdit.xaml
    /// </summary>
    public partial class MeasurableQuantityEdit : UserControl, IView, INavigationAware
    {
        #region Constructors

        public MeasurableQuantityEdit()
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
            (DataContext as ViewModels.MeasurableQuantityEditViewModel).MeasurableQuantityInstance
                = ncontext.Parameters["ObjectInstance"] as MeasurableQuantity;
        }

        #endregion Methods
    }
}