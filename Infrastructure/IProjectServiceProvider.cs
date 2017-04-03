using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IProjectServiceProvider
    {
        bool AlterProjectInfo(Project project);
        Project CreateNewProject(Person leader = null);
    }
}
