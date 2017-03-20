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
            output.IsOverride = true;
            output.Method = toBeOverridden.Method;
            output.Overridden = toBeOverridden;
            output.SpecificationVersions = version;
            
            foreach (SubRequirement subReq in toBeOverridden.SubRequirements)
            {
                SubRequirement tempSub = new SubRequirement();
                
                tempSub.Requirement = output;
                tempSub.SubMethod = subReq.SubMethod;
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
            tempReq.IsOverride = false;
            tempReq.Name = "";
            tempReq.Description = "";
            tempReq.Position = 0;
            
            foreach (SubMethod measure in method.SubMethods)
            {
                SubRequirement tempSub = new SubRequirement();
                tempSub.SubMethod = measure;
                tempReq.SubRequirements.Add(tempSub);
            }

            specification.SpecificationVersions.First(ver => ver.IsMain).Requirements.Add(tempReq);
        }
        
        public List<Requirement> GenerateRequirementList(SpecificationVersion version)
        {
            if (version.IsMain)
                return new List<Requirement>(version.Requirements);

            else
            {
                List<Requirement> output = new List<Requirement>(
                    version.Specification.SpecificationVersions.First(sv => sv.IsMain).Requirements);
                
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
                tempTest.IsComplete = false;
                tempTest.Method = rq.Method;
                tempTest.MethodIssue = rq.Method.Standard.CurrentIssue;
                tempTest.Notes = rq.Description;
                tempTest.Person = report.Author;
                
                foreach (SubRequirement subReq in rq.SubRequirements)
                {
                    tempSub = new SubTest();
                    tempSub.Name = subReq.SubMethod.Name;
                    tempSub.Requirement = subReq.RequiredValue;
                    tempSub.UM = subReq.SubMethod.UM;
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

        public Requirement RemoveOverride(Requirement toBeRemoved)
        {
            Requirement output = toBeRemoved.Overridden;
            Requirements.Remove(toBeRemoved);
            return output;
        }
    }
}
