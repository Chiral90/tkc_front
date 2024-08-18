using System;

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
    public ChampionInfo[] stationed;
    //240603 add
    //0: common, 1: battle
    public int status;
    // public string stationed;
    // public string[] Stationed{
    //     get { return this.stationed.Split(','); }
    // }
    [NonSerialized] public string[] enterTypes = { "선택", "진입", "점령", "잠입", "침입", "전투" };
}
