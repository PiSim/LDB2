﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;

namespace LabDB2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Thread.CurrentPrincipal = new Infrastructure.DBPrincipal();
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
