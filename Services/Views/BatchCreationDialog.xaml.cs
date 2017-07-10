﻿using DBManager;
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

namespace Services.Views
{
    /// <summary>
    /// Logica di interazione per BatchCreationDialog.xaml
    /// </summary>
    public partial class BatchCreationDialog : Window, IView
    {
        public BatchCreationDialog()
        {
            InitializeComponent();
        }

        public Batch BatchInstance
        {
            get { return (DataContext as ViewModels.BatchCreationDialogViewModel).BatchInstance; }
        }
    }
}