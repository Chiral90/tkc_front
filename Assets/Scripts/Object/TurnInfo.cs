using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.Rendering;
using UnityEngine;

[System.Serializable]
public class TurnInfo
{
    public int currentRound;
    public int currentTurn;
    public string startTime;
    public DateTime getStartTime()
    {
        return DateTime.ParseExact(this.startTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    }
    public string getRemain()
    {
        DateTime _now = DateTime.Now;
        DateTime _start = getStartTime();
        TimeSpan _diff = _start.AddMinutes(5) - _now;
        // TimeSpan result = TimeSpan.ParseExact((DateTime.Now - getStartTime()).ToString(), "mmss", CultureInfo.InvariantCulture);
        // result = DateTime.Now - getStartTime();
        string result = ((_diff < TimeSpan.Zero) ? "-" : "") + _diff.ToString(@"mm\:ss");
        Debug.Log(result);
        // return result;
        return result;
    }
}
