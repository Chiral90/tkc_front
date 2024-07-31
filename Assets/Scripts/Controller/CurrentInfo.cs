#define DEV_STATE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentInfo : MonoBehaviour
{
    // public static string serverURI = "http://192.168.0.4:8181";
    public static string serverURI = "http://192.168.0.2:8181";     // v16
    public static string currentID;
    public static ChampionInfo currentChampion;
    public static TurnInfoManager currentTurnInfo;
    public static BuildingInfo currentBattleLocationInfo;
    Api api;

    #region Singleton
    private static CurrentInfo _instance;
    public static CurrentInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("CurrentInfo is NULL");
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;

        // DontDestroyOnLoad(this);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            string _id;
            //Debug.Log(Application.absoluteURL);
            //var _cookiesRes = GetId();
            //while (_cookiesRes.MoveNext())
            //{
            //    Debug.Log(_cookiesRes.Current.ToString());

                //_id = _cookiesRes.Current.ToString();
            //}
            _id = "dozos";

            currentID = _id;
            // Debug.Log("CurrentInfo Start");
            SetChampInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // try
        // {
        //     if (currentID == null || currentID.Equals(""))
        //     {
        //         GetId();
        //     }
        // }
        // catch (Exception e)
        // {
        //     Debug.Log(e.Message);
        //     SceneManager.LoadScene(0);
        // }
    }

    void SetUserPanel()
    {
        // Debug.Log(currentChampion.champ_name);
        // Debug.Log(currentChampion.champ_type.ToString());
        var _panel = GameObject.Find("userCanvas").transform.Find("userPanel").gameObject;
        // Debug.Log(_panel.name);
        var _nameObj = _panel.transform.Find("Champion Name").GetComponent<TMP_Text>();
        var _typeObj = _panel.transform.Find("Champion Type").GetComponent<TMP_Text>();
        var _lsObj = _panel.transform.Find("Leadership").GetComponent<TMP_Text>();
        _nameObj.text = currentChampion.champ_name;
        _typeObj.text = currentChampion.ChampType;
        _lsObj.text = currentChampion.leadership.ToString();
    }
    void SetChampInfo()
    {
        api.routePath = "/champ";
#if DEV_STATE
        currentID = "dozos";
        currentChampion = new ChampionInfo();
        currentChampion.createNewChampion("chiral", 3, 1);
        // CurrentInfo.currentChampion.own_castles.Append("castle1");
        currentChampion.location = "castle1";
        currentChampion.leadership = 100;
        UnitInfo unit1 = new UnitInfo();
        SetUserPanel();
#else
        GetChampInfo();
        GetUnitInfo();
#endif
    }
    void GetChampInfo()
    {
        var result = StaticCoroutine.StartStaticCoroutine(api.GetRequest(onSuccess: (result) =>
        {
            // do something
            currentChampion = JsonUtility.FromJson<ChampionInfo>(result);
            SetUserPanel();

        }, onFailure: (error) =>
        {
            // do something
            Debug.LogError(error);
            GameObject.Find("Canvas").transform.Find("BackGround").transform.Find("Create Champion").gameObject.SetActive(true);
        }));
    }
    void GetUnitInfo()
    {
        var result = StaticCoroutine.StartStaticCoroutine(api.GetRequest(onSuccess: (result) =>
        {
            // do something
            Debug.Log(result);
            List<UnitInfo> unitData = JsonHelper.FromJson<UnitInfo>(result).ToList();
            currentChampion.units = unitData;

        }, onFailure: (error) =>
        {
            // do something
            Debug.LogError(error);
            GameObject.Find("Canvas").transform.Find("BackGround").transform.Find("Create Champion").gameObject.SetActive(true);
            UnitInfo _u = new UnitInfo();
            
        }));
    }
}
