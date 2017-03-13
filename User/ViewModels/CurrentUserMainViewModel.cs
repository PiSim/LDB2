using DBManager;
using Prism.Mvvm;
using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace User.ViewModels
{
    public class CurrentUserMainViewModel : BindableBase
    {
        public CurrentUserMainViewModel() : base()
        { 
            
        }

        private DBManager.User CurrentUser
        {
            get { return (Thread.CurrentPrincipal as  DBPrincipal).CurrentUser;  }
        }

        private string Name
        {
            get { return CurrentUser.Person.Name; }
        }

        public List<Report> ReportList
        {
            get { return new List<Report>(CurrentUser.Person.Reports); }
        }
    }
}
