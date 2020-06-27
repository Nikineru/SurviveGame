using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class BoostName : Attribute
{
    public string Name { get; set; }
    public BoostName(string name) 
    {
        Name = name;
    }
}
