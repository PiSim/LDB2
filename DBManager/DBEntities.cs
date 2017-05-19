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
        
        public Requirement RemoveOverride(Requirement toBeRemoved)
        {
            Requirement output = toBeRemoved.Overridden;
            Requirements.Remove(toBeRemoved);
            return output;
        }
    }
}
