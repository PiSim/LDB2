using LabDbContext;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Scripts
{
    public class RemoveMaterialDuplicatesScript : ScriptBase
    {
        #region Constructors

        public RemoveMaterialDuplicatesScript() : base()
        {
            _name = "RemoveMaterialDuplicates";
        }

        #endregion Constructors

        #region Methods

        public override void Run()
        {
            using (LabDbEntities context = new LabDbEntities())
            {
                Material lastMaterial;
                List<List<Material>> duplicates = new List<List<Material>>();
                List<Material> currentGroup;

                IList<Material> matList = context.Materials.AsNoTracking()
                                                                .OrderBy(mat => mat.AspectID)
                                                                .ThenBy(mat => mat.LineID)
                                                                .ThenBy(mat => mat.TypeID)
                                                                .ThenBy(mat => mat.RecipeID)
                                                                .ToList();
                {
                    lastMaterial = matList[0];
                    currentGroup = new List<Material>();

                    foreach (Material mat in matList.Skip(1))
                    {
                        if (mat.AspectID == lastMaterial.AspectID &&
                            mat.LineID == lastMaterial.LineID &&
                            mat.TypeID == lastMaterial.TypeID &&
                            mat.RecipeID == lastMaterial.RecipeID)
                            currentGroup.Add(mat);
                        else
                        {
                            if (currentGroup.Count > 1)
                                duplicates.Add(currentGroup);
                            currentGroup = new List<Material>() { mat };
                        }

                        lastMaterial = mat;
                    }

                    foreach (List<Material> duplicateGroup in duplicates)
                    {
                        Material parent = duplicateGroup[0];
                        foreach (Material toSubstitute in duplicateGroup.Skip(1))
                        {
                            foreach (Batch btc in toSubstitute.Batches)
                                btc.MaterialID = parent.ID;

                            context.Entry(toSubstitute).State = System.Data.Entity.EntityState.Deleted;
                        }
                    }

                    context.SaveChanges();
                }
            }
        }

        #endregion Methods
    }
}