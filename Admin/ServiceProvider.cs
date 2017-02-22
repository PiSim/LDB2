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
    
}