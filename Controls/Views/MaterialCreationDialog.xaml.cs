using Microsoft.Practices.Prism.Mvvm;
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

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for MaterialCreationDialog.xaml
    /// </summary>
    public partial class MaterialCreationDialog : Window, IView
    {
        public MaterialCreationDialog()
        {
            InitializeComponent();
        }

        public string MaterialAspect
        {
            get { return (DataContext as ViewModels.MaterialCreationDialogViewModel).Aspect; }
        }

        public string MaterialLine
        {
            get { return (DataContext as ViewModels.MaterialCreationDialogViewModel).Line; }
        }

        public string MaterialRecipe
        {
            get { return (DataContext as ViewModels.MaterialCreationDialogViewModel).Recipe; }
        }

        public string MaterialType
        {
            get { return (DataContext as ViewModels.MaterialCreationDialogViewModel).Type; }
        }
    }
}
