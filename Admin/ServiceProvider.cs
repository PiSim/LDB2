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
            foreach (StandardFile stdFile in mtd.Standard.standard_files)
            {
                MethodFile tempMtdFile = new MethodFile();
                tempMtdFile.Description = stdFile.description;
                tempMtdFile.Path = stdFile.path;
                tempIssue.MethodFiles.Add(tempMtdFile);
            }
            mtd.Issues.Add(tempIssue);
        }

        _entities.SaveChanges();
    }
}