using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingInfo
{
    public string building_name;
    public string castellan;
    public int team;
    public int building_type;
    public int population;
    public int food;
    public int morale;
    public List<string> stationed;
}
