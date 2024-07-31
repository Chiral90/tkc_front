[System.Serializable]
public class UnitInfo
{
    public int unit_index;
    public string unit_nickname;
    public int unit_type;
    public int troops_quantity;
    public float unit_attack;
    public float unit_defence;
    public int unit_morale;
    // 0: 대기, 1: 전투, 2~: 상태 이상 
    public int unit_status;
    public string unit_location;
    public float unit_location_x;
    public float unit_location_y;
    public float unit_location_z;

    public string UnitType
    {
        get
        {
            return CurrentInfo.currentChampion.unit_types[this.unit_type];
        }
    }
}