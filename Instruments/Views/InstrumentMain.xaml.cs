using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Controls;

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for InstrumentsMainView.xaml
    /// </summary>
    public partial class InstrumentMain : UserControl, IView
    {
        #region Constructors

        public InstrumentMain(LabDbEntities entities)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}