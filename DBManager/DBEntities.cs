using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBEntities : LabDBEntities
    {
        public Sample CreateSampleForBatch(Batch batch, string actionCode)
        {
            Sample output = new Sample();
            output.batch = batch;
            output.date = DateTime.Now;
            output.code = actionCode;
            Samples.Add(output);
            return output;
        }
        
        public List<Requirement> GenerateRequirementList(SpecificationVersion version)
        {
            if (version.IsMain == 1)
                return new List<Requirement>(version.Requirements);

            else
            {
                List<Requirement> output = new List<Requirement>(
                    version.Specification.SpecificationVersions.First(sv => sv.IsMain == 1).Requirements);
                foreach (Requirement requirement in version.Requirements)
                {
                    int ii = output.FindIndex(rr => rr.Method.ID == requirement.Method.ID);
                    output[ii] = requirement;
                }

                return output;
            }
        }

        public Aspect GetAspect(string code,
                                bool new_instance_if_not_found = true)
        {
            Aspect output = Aspects.FirstOrDefault(asp => asp.Code == code);

            if (output == null && new_instance_if_not_found)
            {
                output = new Aspect();
                output.Code = code;
            }

            return output;
        }

        public Batch GetBatchByNumber(string number, bool new_instance_if_none_found = true)
        {
            Batch output = base.Batches.FirstOrDefault(bb => bb.Number == number);

            if (output == null && new_instance_if_none_found)
            {
                output = new Batch();
                output.Number = number;
                Batches.Add(output);
            }

            return output;
        }

        public Construction GetConstruction(string type,
                                            string line,
                                            string aspectCode,
                                            bool new_instance_if_not_found = true)
        {
            Construction output = Constructions.FirstOrDefault(cons =>
                cons.Type == type &&
                cons.Line == line &&
                cons.Aspect.Code == aspectCode);

            if (output == null && new_instance_if_not_found)
            {
                output = new Construction();
                output.Type = type;
                output.Line = line;
                output.Aspect = GetAspect(aspectCode);
            }

            return output;
        }

        public Material GetMaterial(string type,
                                    string line,
                                    string aspectCode,
                                    string recipeCode,
                                    bool new_instance_if_not_found = true)
        {
            Material output = Materials.FirstOrDefault(mat =>
               mat.Construction.Type == type &&
               mat.Construction.Line == line &&
               mat.Construction.Aspect.Code == aspectCode &&
               mat.Recipe.Code == recipeCode);

            if (output == null && new_instance_if_not_found)
            {
                output = new Material();
                output.Construction = GetConstruction(type, line, aspectCode);
                output.Recipe = GetRecipe(recipeCode);
            }

            return output;
        }

        public Recipe GetRecipe(string code,
                                bool new_instance_if_not_found = true)
        {
            Recipe output = Recipes.FirstOrDefault(rec => rec.Code == code);

            if (output == null && new_instance_if_not_found)
            {
                output = new Recipe();
                output.Code = code;
            }

            return output;
        }

    }
}
