using System;
using System.Collections.Generic;

public class ProductNames
{
    public List<string> names { get; set; }

    public ProductNames()
    {
        names = new List<string>()
        {
            "IPHONE 15",
            "SAMSUNG S23 ULTRA",
            "NOKIA M23"
        };
    }

    public List<string> GetNames() => names;
}