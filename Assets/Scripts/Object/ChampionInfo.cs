using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChampionInfo
{
    public string champ_name;
    // 0: 지, 1: 체, 2: 덕, 3: 첩보, 4: 보급
    public int champ_type;
    public int leadership;
    public int team;
    // public string[] own_castles;
    public string location;
    [NonSerialized] public string tmpLocation;
    [NonSerialized] public List<UnitInfo> units;
    [NonSerialized] public string[] unit_types = { "보병", "기병", "창병", "전차병", "궁병" };

    public void createNewChampion(string name, int type, int team)
    {
        this.champ_name = name;
        this.champ_type = type;
        this.leadership = 0;
        //this.ownCastles = null;
        this.team = team;
        // this.own_castles = new string[] {};
        this.location = "";
    }

    public WWWForm createWWWForm()
    {
        WWWForm _f = new WWWForm();
        _f.AddField("champ_name", this.champ_name, System.Text.Encoding.UTF8);
        _f.AddField("champ_type", this.champ_type);
        _f.AddField("leadership", this.leadership);
        _f.AddField("team", this.team);
        _f.AddField("location", this.location);
        // if (this.own_castles.Length != 0)
        // {
        //     _f.AddField("own_castles", "[" + String.Join(", ", this.own_castles) + "]", System.Text.Encoding.UTF8);
        // }
        // else
        // {
        //     _f.AddField("own_castles", "");
        // }
        return _f;
        
    }
    public string ChampType {
        get
        {
            string typeName = "";
            switch (this.champ_type)
            {
                case 0:
                    typeName = "지장";
                    break;
                case 1:
                    typeName = "무장";
                    break;
                case 2:
                    typeName = "덕장";
                    break;
                case 3:
                    typeName = "첩보";
                    break;
                case 4:
                    typeName = "보급";
                    break;
                default:
                    typeName = "오류";
                    break;
            }
            return typeName;
        }
    }
    public void SetDefaultUnit()
    {
        UnitInfo _u = new UnitInfo();
        _u.unit_index = 0;
        if (this.champ_type == 0)
        {
            _u.unit_type = -1;
        }
        else if (this.champ_type == 1)
        {
            _u.unit_type = 2;
        }
        else if (this.champ_type == 2)
        {
            _u.unit_type = 1;
        }
        else if (this.champ_type == 3)
        {
            _u.unit_type = 0;
        }
        else if (this.champ_type == 4)
        {
            _u.unit_type = 0;
        }
        _u.troops_quantity = this.leadership;
        _u.unit_attack = this.leadership;
        _u.unit_defence = this.leadership;
        _u.unit_morale = this.leadership;
        _u.unit_status = 0;
        float c = CalcLeadershipCoefficient(_u);
        _u.unit_attack *= c;
    }
    public float CalcLeadershipCoefficient(UnitInfo u)
    {
        float c = 1;
        // unit type
        if (u.unit_type == 0)
        {
            c = 0;
        }
        else if (u.unit_type == 1)
        {
            c = 1;
        }
        else if (u.unit_type == 2)
        {
            c = 1.25f;
        }
        else if (u.unit_type == 3)
        {
            c = 1f;
        }
        else if (u.unit_type == 4)
        {
            c = 1;
        }
        // unit status

        // unit morale
        return c;
    }
}