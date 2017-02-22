using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBEntities : LabDBEntities
    {
        public Requirement AddOverride(SpecificationVersion version,
                                        Requirement toBeOverridden)
        {
            Requirement output = new Requirement();
            output.Description = toBeOverridden.Description;
            output.IsOverride = 1;
            output.Method = toBeOverridden.Method;
            output.Overridden = toBeOverridden;
            output.SpecificationVersions = version;
            
            foreach (SubRequirement subReq in toBeOverridden.SubRequirements)
            {
                SubRequirement tempSub = new SubRequirement();
                
                tempSub.Requirement = output;
                tempSub.Name = subReq.Name;
                tempSub.RequiredValue = subReq.RequiredValue;
                
                output.SubRequirements.Add(tempSub);
            }
            
            version.Requirements.Add(output);
            return output;
        }

        public void AddTest(Specification specification, Method method)
        {
            Requirement tempReq = new Requirement();
            tempReq.Method = method;
            tempReq.IsOverride = 0;
            tempReq.Name = "";
            tempReq.Description = "";
            tempReq.Position = 0;
            
            foreach (MethodMeasurement measure in method.Measurements)
            {
                SubRequirement tempSub = new SubRequirement();
                tempSub.Name = measure.Name;
                tempReq.SubRequirements.Add(tempSub);
            }

            specification.SpecificationVersions.First(ver => ver.IsMain == 1).Requirements.Add(tempReq);
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

        public void GenerateTestList(Report report,
                                    IEnumerable<Requirement> requirements)
        {
            Test tempTest;
            SubTest tempSub;
            foreach (Requirement rq in requirements)
            {
                tempTest = new Test();
                tempTest.Batch = report.Batch;
                tempTest.IsComplete = 0;
                tempTest.Method = rq.Method;
                tempTest.Person = report.Author;
                tempTest.Report = report;
                foreach (SubRequirement subReq in rq.SubRequirements)
                {
                    tempSub = new SubTest();
                    tempSub.SubRequirement = subReq;
                    tempTest.SubTests.Add(tempSub);
                }

                report.Tests.Add(tempTest);
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
            Batch output = Batches.FirstOrDefault(bb => bb.Number == number);

            if (output == null && new_instance_if_none_found)
            {
                output = new Batch();
                output.Number = number;
            }

            return output;
        }

        public Construction GetConstruction(string type,
                                            string line,
                                            string aspectCode,
                                            bool new_instance_if_not_found = true)
        {
            Construction output = Constructions.FirstOrDefault(cons =>
                cons.MaterialType.Code == type &&
                cons.Line == line &&
                cons.Aspect.Code == aspectCode);

            if (output == null && new_instance_if_not_found)
            {
                output = new Construction();
                output.MaterialType = MaterialTypes.First(tt => tt.Code == type);
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
               mat.Construction.MaterialType.Code == type &&
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
        
        public Requirement RemoveOverride(Requirement toBeRemoved)
        {
            Requirement output = toBeRemoved.Overridden;
            Requirements.Remove(toBeRemoved);
            return output;
        }
    }
}
