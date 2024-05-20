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
}