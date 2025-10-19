using UnityEngine;
using System;

[Serializable]
public class CropInfo
{
    public string name;
    public string pluralName;
    public int value;
    public int growth;
    public int quantity;
    public string type;

    public CropInfo(string n, string s, int v, int g, int q, string t)
    {
        name = n; pluralName = s; value = v; growth = g; quantity = q; type = t;
    }
}
