using LabDbContext;

namespace Infrastructure
{
    public interface IProjectService
    {
        #region Methods

        Project CreateProject();

        void UpdateAllCosts();

        #endregion Methods
    }
}