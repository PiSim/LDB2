﻿using DBManager;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for SpecificationEditView.xaml
    /// </summary>
    public partial class SpecificationEditView : UserControl
    {
        public SpecificationEditView(DBEntities entities, Specification instance)
        {
            DataContext = new ViewModels.SpecificationEditViewModel(entities, instance);
            InitializeComponent();
        }
    }
}
