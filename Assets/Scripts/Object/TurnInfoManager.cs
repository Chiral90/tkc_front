#define DEV_STATE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnInfoManager : MonoBehaviour
{
    TurnInfo _turnInfo;
    Api api;

    private void Start()
    {
        _turnInfo = TurnInfo.Instance;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SetCurrentRound();
            StartCoroutine(TurnProgress());
        }
    }
    DateTime GetStartTime()
    {
        return DateTime.ParseExact(_turnInfo.startTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
    }
    public string GetRemain()
    {
        DateTime _now = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
        DateTime _start = GetStartTime();
        TimeSpan _diff = _start.AddMinutes(5) - _now;
        // TimeSpan result = TimeSpan.ParseExact((DateTime.Now - getStartTime()).ToString(), "mmss", CultureInfo.InvariantCulture);
        // result = DateTime.Now - getStartTime();
        string result = ((_diff < TimeSpan.Zero) ? "-" : "") + _diff.ToString(@"mm\:ss");
        return result;
    }
    void SetTurnPanel()
    {
        var _panel = GameObject.Find("userCanvas").transform.Find("userPanel").gameObject;
        var _roundObj = _panel.transform.Find("Round").GetComponent<TMP_Text>();
        var _turnObj = _panel.transform.Find("Turn").GetComponent<TMP_Text>();
        var _remainObj = _panel.transform.Find("Remain").GetComponent<TMP_Text>();
        _roundObj.text = _turnInfo.currentRound.ToString();
        _turnObj.text = _turnInfo.currentTurn.ToString();
        _remainObj.text = GetRemain();
    }
    // Set Time Remain
    IEnumerator TurnProgress()
    {
        if (GetRemain().Equals("00:00"))
        {
            Debug.Log("Finish Turn");
            FinishTurn();
            yield return new WaitForSeconds(1);
        }
        else
        {
            SetTurnPanel();
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(TurnProgress());
    }
    // Finish Turn
    void FinishTurn()
    {
        // _turnInfo.currentTurn += 1;
        // _turnInfo.startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        GetDataOfNewTurn();
        SetTurnPanel();
        // Send Data Server
    }
    // Get New Turn Data
    void GetDataOfNewTurn()
    {
#if DEV_STATE
        
#else
        api.routePath = "/getRound";
        var result = StaticCoroutine.StartStaticCoroutine(api.GetRequest(onSuccess: (result) =>
        {
            // do something
            _turnInfo = JsonUtility.FromJson<TurnInfo>(result);

            //Debug.Log(String.Format("Round:{0} / Turn:{1} / StartTime:{2}", _c.currentRound, _c.currentTurn, _c.startTime));
            SetTurnPanel();
        }, onFailure: (error) =>
        {
            // do something
        }));
#endif
    }
    void SetCurrentRound()
    {
#if DEV_STATE
        _turnInfo.currentRound = 1;
        _turnInfo.currentTurn = 1;
        _turnInfo.startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        SetTurnPanel();
#else
        api.routePath = "/getRound";
        var result = StaticCoroutine.StartStaticCoroutine(api.GetRequest(onSuccess: (result) =>
        {
            // do something
            _turnInfo = JsonUtility.FromJson<TurnInfo>(result);

            Debug.Log(
                String.Format(
                    "Round:{0} / Turn:{1} / StartTime:{2}", _turnInfo.currentRound, _turnInfo.currentTurn, _turnInfo.startTime
                    )
                );
            SetTurnPanel();
        }, onFailure: (error) =>
        {
            // do something
        }));
#endif
    }
}

[System.Serializable]
public class TurnInfo
{
    public int currentRound;
    public int currentTurn;
    public string startTime;

    #region Singleton
    private static TurnInfo _instance;
    public static TurnInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("TurnInfo is NULL");
                _instance = new TurnInfo();
            }

            return _instance;
        }
    }
    #endregion
}