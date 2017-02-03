﻿using DBManager;
using Prism.Events;
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

namespace Projects.Views
{
    /// <summary>
    /// Interaction logic for ProjectMainView.xaml
    /// </summary>
    public partial class ProjectMainView : UserControl
    {
        public ProjectMainView(DBEntities entities, EventAggregator aggregator)
        {
            DataContext = new ViewModels.ProjectMainViewModel(entities, aggregator);
            InitializeComponent();
        }
    }
}
