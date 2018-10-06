using Infrastructure;
using LabDbContext;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Threading;

namespace User.ViewModels
{
    public class CurrentUserMainViewModel : BindableBase
    {
        #region Fields

        private DBIdentity _identity;

        #endregion Fields

        #region Constructors

        public CurrentUserMainViewModel() : base()
        {
            _identity = Thread.CurrentPrincipal.Identity as DBIdentity;
        }

        #endregion Constructors

        #region Properties

        public List<Report> ReportList => new List<Report>(_identity.User.Person.Reports);
        private string Name => _identity.User.Person.Name;

        #endregion Properties
    }
}