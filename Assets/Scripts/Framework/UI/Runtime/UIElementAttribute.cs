using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class UIElementAttribute : Attribute
{
    public string prefabPath;

    public UIElementAttribute(string prefabPath)
    {
        this.prefabPath = prefabPath; 
    }
}
