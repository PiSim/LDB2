﻿using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per BatchCreationDialog.xaml
    /// </summary>
    public partial class BatchCreationDialog : Window, IView
    {
        #region Constructors

        public BatchCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Batch BatchInstance => (DataContext as ViewModels.BatchCreationDialogViewModel).BatchInstance;

        #endregion Properties
    }
}