using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Controls;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for StatusBarView.xaml
    /// </summary>
    public partial class StatusBar : UserControl, IView
    {
        #region Constructors

        public StatusBar()
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}