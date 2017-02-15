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
    /// Interaction logic for SpecificationCreationDialog.xaml
    /// </summary>
    public partial class SpecificationCreationDialog : Window
    {
        private Specification _specificationInstance;

        public SpecificationCreationDialog(DBEntities entities, 
                                            SpecificationServiceProvider serviceProvider)
        {
            DataContext = new ViewModels.SpecificationCreationViewModel(entities
                                                                        , serviceProvider
                                                                        ,this);
            InitializeComponent();
        }

        public Specification SpecificationInstance
        {
            get { return _specificationInstance; }
            set { _specificationInstance = value; }
        }
    }
}
