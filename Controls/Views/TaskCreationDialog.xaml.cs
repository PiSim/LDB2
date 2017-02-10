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
using System.Windows.Shapes;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for TaskCreationDialog.xaml
    /// </summary>
    public partial class TaskCreationDialog : Window
    {
        private Task _taskInstance;
        
        public TaskCreationDialog(DBEntities entities)
        {
            DataContext = new ViewModels.TaskCreationViewModel(entitites, this);
            InitializeComponent();
        }
        
        public Task TaskInstance
        {
            get { return _taskInstance; }
            set { _taskInstance = value; }
        }
    }
}
