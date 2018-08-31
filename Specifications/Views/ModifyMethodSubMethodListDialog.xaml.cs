using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Specifications.Views
{
    /// <summary>
    /// Logica di interazione per ModifyMethodSubMethodListDialog.xaml
    /// </summary>
    public partial class ModifyMethodSubMethodListDialog : Window
    {
        public ModifyMethodSubMethodListDialog()
        {
            InitializeComponent();
        }

        public ICollection<SubMethod> SubMethodList => (DataContext as ViewModels.ModifyMethodSubMethodListDialogViewModel).SubMethodList;

        public Method OldVersion
        {
            get => (DataContext as ViewModels.ModifyMethodSubMethodListDialogViewModel).OldVersion;
            set => (DataContext as ViewModels.ModifyMethodSubMethodListDialogViewModel).OldVersion = value;
        }
    }
}
