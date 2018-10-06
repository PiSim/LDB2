using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Logica di interazione per InstrumentTypeEdit.xaml
    /// </summary>
    public partial class PropertyEdit : UserControl, IView, INavigationAware
    {
        #region Constructors

        public PropertyEdit()
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
            (DataContext as ViewModels.PropertyEditViewModel).PropertyInstance
                = ncontext.Parameters["ObjectInstance"] as Property;
        }

        #endregion Methods
    }
}