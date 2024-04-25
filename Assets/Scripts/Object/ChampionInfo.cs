using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChampionInfo
{
    public string champ_name;
    public int champ_type;
    public int leadership;
    public int team;
    public List<string> own_castles;

    public void createNewChampion(string name, int type, int team)
    {
        this.champ_name = name;
        this.champ_type = type;
        this.leadership = 0;
        //this.ownCastles = null;
        this.team = team;
        this.own_castles = new List<string>();
    }

    public WWWForm createWWWForm()
    {
        WWWForm _f = new WWWForm();
        _f.AddField("champ_name", this.champ_name, System.Text.Encoding.UTF8);
        _f.AddField("champ_type", this.champ_type);
        _f.AddField("leadership", this.leadership);
        _f.AddField("team", this.team);
        _f.AddField("own_castles", "[" + String.Join(", ", this.own_castles) + "]", System.Text.Encoding.UTF8);
        return _f;
        
    }
}