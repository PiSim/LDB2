using DBManager;
using System;
using System.Collections.Generic;

public class SamplesWrapper
{
    private Sample _instance;
    private static readonly Dictionary<string,string> _actionDictionary = new Dictionary<string,string>()
    {
        {"A", "Arrivato in laboratorio"},
        {"B", "Buttato"},
        {"F", "Finito"},
        {"S", "Spedito"},
        {"M", "Masterizzato"}
    };
    
    public SamplesWrapper(Sample instance)
    {
        _instance = instance;        
    }
    
    public string Action
    {
        get { return _actionDictionary[_instance.Code]; }
    }
    
    public string Date
    {
        get { return _instance.Date.ToShortDateString();}
    }
    
}