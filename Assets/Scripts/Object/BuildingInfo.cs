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
    // public string stationed;
    // public string[] Stationed{
    //     get { return this.stationed.Split(','); }
    // }
}
