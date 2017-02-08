using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;


public class ServiceProvider
{
    private DBEntities _entities;
    
    public ServiceProvider(DBEntities entities)
    {
        _entities = entities;
    }
    
    public void BuildExternalReportFiles()
    {
        List<ExternalReport> _reportList = new List<ExternalReport>(_entities.ExternalReports);

        foreach (ExternalReport ext in _reportList)
        {
            string[] fileIDs = ext.fileID.Split((char)007);
            
            foreach (string id in fileIDs)
            {
                int parsedID;
                
                if (!int.TryParse(id, out parsedID))
                    continue;
                    
                File tempFile = _entities.Files1.FirstOrDefault(file => file.ID == parsedID);
                
                if (tempFile == null)
                    continue;
                
                ExternalReportFile tempExt = new ExternalReportFile();
                ext.ExternalReportFiles.Add(tempExt);
                tempExt.Path = tempFile.path;
                tempExt.Description = tempFile.description;
                if (tempExt.Description == null)
                    tempExt.Description = "";                
            }
        }

        _entities.SaveChanges();
    }

    public void BuildMethodFiles()
    {
        List<Method> _methodList = new List<Method>(_entities.Methods);

        foreach (Method mtd in _methodList)
        {
            mtd.Oem = mtd.Standard.Organization;
            mtd.Name = mtd.Standard.Name;
            MethodIssue tempIssue = new MethodIssue();
            tempIssue.Issue = mtd.Standard.Issue;

            File stdFile = _entities.Files1.FirstOrDefault(f => f.ID == mtd.Standard.fileID);
            if (stdFile != null)
            {
                MethodFile tempMtdFile = new MethodFile();
                tempMtdFile.Description = stdFile.description;
                if (tempMtdFile.Description == null)
                    tempMtdFile.Description = "";

                tempMtdFile.Path = stdFile.path;
                tempIssue.MethodFiles.Add(tempMtdFile);
            }

            mtd.CurrentIssue = tempIssue;
        }

        _entities.SaveChanges();
    }

    public void BuildSpecificationFiles()
    {
        List<Specification> _specList = new List<Specification>(_entities.Specifications);

        foreach (Specification spc in _specList)
        {
            spc.Oem = spc.Standard.Organization;
            spc.Name = spc.Standard.Name;
            SpecificationIssue tempIssue = new SpecificationIssue();
            tempIssue.Issue = spc.Standard.Issue;

            File stdFile = _entities.Files1.FirstOrDefault(f => f.ID == spc.Standard.fileID);
            if (stdFile != null)
            {
                SpecificationFile tempMtdFile = new SpecificationFile();
                tempMtdFile.Description = stdFile.description;
                if (tempMtdFile.Description == null)
                    tempMtdFile.Description = "";

                tempMtdFile.Path = stdFile.path;
                tempIssue.SpecificationFiles.Add(tempMtdFile);
            }

            spc.CurrentIssue = tempIssue;
        }

        _entities.SaveChanges();
    }

}