﻿using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Controls;

namespace User.Views
{
    /// <summary>
    /// Interaction logic for UserMainView.xaml
    /// </summary>
    public partial class CurrentUserMain : UserControl, IView
    {
        #region Constructors

        public CurrentUserMain()
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}