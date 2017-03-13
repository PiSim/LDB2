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
        private DBPrincipal _principal;
        private DBIdentity _identity;

        public CurrentUserMainViewModel(DBPrincipal principal) : base()
        {
            _principal = principal;
            _identity = principal.Identity as DBIdentity;
        }

        private string Name
        {
            get { return _identity.User.Person.Name; }
        }

        public List<Report> ReportList
        {
            get { return new List<Report>(_identity.User.Person.Reports); }
        }
    }
}
