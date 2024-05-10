using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    #region Singleton
    private static ButtonAction _instance;
    public static ButtonAction Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ButtonAction is NULL");
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

        DontDestroyOnLoad(this);
    }
    #endregion
    Api api;
    void Start()
    {
        
    }
    void Update()
    {

    }
    public void startGame()
    {
        Debug.Log("Start Game : " + CurrentInfo.currentChampion.champ_name);
        SceneManager.LoadScene("SampleScene");
    }

    public void createChampion()
    {
        GameObject.Find("Canvas").transform.Find("Champion Data").gameObject.SetActive(true);
    }

    public void confirmCreateChampion()
    {
        string _id = CurrentInfo.currentID;

        var _nameObj = GameObject.Find("ChampionName").GetComponent<TMP_InputField>();
        var _typeObj = GameObject.Find("ChampionType").GetComponent<TMP_Dropdown>();
        var _teamObj = GameObject.Find("ChampionTeam").GetComponent<TMP_Dropdown>();
        string _name = _nameObj.text;
        int _type = _typeObj.value;
        int _team = _teamObj.value;
        //validate name, type
        ChampionInfo _champ = new ChampionInfo();
        _champ.createNewChampion(_name, _type, _team);
        string _champStr = JsonUtility.ToJson(_champ);
        Debug.Log(_champStr);
        api.routePath = "/champ/create";
        WWWForm _champWWW = _champ.createWWWForm();
        StartCoroutine(api.PostRequest(_champWWW, onSuccess: (result) =>
        {
            Debug.Log(result);
            GameObject _button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            _button.transform.parent.parent.Find("Create Champion").gameObject.SetActive(false);
            _button.transform.parent.gameObject.SetActive(false);
        }, onFailure: (error) =>
        {
            Debug.Log(error);
        }));
    }

    public void cancelCreateChampion()
    {
        GameObject.Find("Canvas").transform.Find("Champion Data").gameObject.SetActive(false);
    }

    public void openChamplist()
    {
        GameObject _button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject _subPanel = _button.transform.parent.gameObject.transform.parent.Find("Building Panel Sub").gameObject;
        Debug.Log(string.Format("subPanel: {0}", _subPanel.activeSelf));
        if (!_subPanel.activeSelf)
        {
            _subPanel.SetActive(true);
        }
        _subPanel.transform.Find("Building Detail Panel").gameObject.SetActive(false);
        _subPanel.transform.Find("Building Champ List Panel").gameObject.SetActive(true);
        // print champion list
        UIPopup _mainPanelData = _subPanel.transform.parent.gameObject.GetComponent<UIPopup>();
        GameObject _content = _subPanel.transform.Find("Building Champ List Panel").transform.Find("Building Stationed Champions").transform.Find("Viewport").transform.Find("Content").gameObject;
        GameObject champData = _content.transform.Find("champ1").gameObject;
        api.routePath = "/champ/champs/" + String.Join(",", _mainPanelData.data.stationed);
        Debug.Log("/champ/champs/" + String.Join(",", _mainPanelData.data.stationed));
        StartCoroutine(api.GetRequest(onSuccess: (result) =>
        {
            var data = result;
            Debug.Log(data);
            // do something
        }, onFailure: (error) =>
        {
            Debug.Log(error);
            // do something
        }));
        foreach (var c in _mainPanelData.data.stationed)
        {
            //GameObject champData = 
        }
    }
    public void openDetail()
    {
        GameObject _button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject _subPanel = _button.transform.parent.gameObject.transform.parent.Find("Building Panel Sub").gameObject;
        Debug.Log(string.Format("subPanel: {0}", _subPanel.activeSelf));
        if (!_subPanel.activeSelf)
        {
            _subPanel.SetActive(true);
        }
        _subPanel.transform.Find("Building Champ List Panel").gameObject.SetActive(false);
        _subPanel.transform.Find("Building Detail Panel").gameObject.SetActive(true);
        UIPopup _mainPanelData = _subPanel.transform.parent.gameObject.GetComponent<UIPopup>();
        _subPanel.transform.Find("Building Detail Panel").transform.Find("Building Food").GetComponent<TextMeshProUGUI>().text = _mainPanelData.data.food.ToString();
        _subPanel.transform.Find("Building Detail Panel").transform.Find("Building Morale").GetComponent<TextMeshProUGUI>().text = _mainPanelData.data.morale.ToString();
    }
    public void enterBuilding()
    {
        GameObject _button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string text = _button.GetComponentInChildren<TextMeshProUGUI>().text;
        string targetName = _button.transform.parent.Find("Building Name").GetComponent<TextMeshProUGUI>().text;
        // 침입
        if (text.Equals("침입"))
        {
            // Siege
            Debug.Log("Siege");
        }
        // 점령
        else if (text.Equals("점령"))
        {
            // Occupation
            Debug.Log("Occupation");
            //string beforeLocation = CurrentInfo.currentChampion.location;
            //CurrentInfo.currentChampion.location = targetName;
            // 다음 턴에 이동
        }
        // 진입
        else if (text.Equals("진입"))
        {
            // Stationed
            Debug.Log("Stationed");
        }
        else if (text.Equals("잠입"))
        {
            // Spy
            Debug.Log("Spy");
        }
        calcDistance(targetName);
    }

    void siege()
    {
        
    }
    void calcDistance(string targetName)
    {
        // 현재 위치
        string current = CurrentInfo.currentChampion.location;
        // 목표 위치
        string target = targetName;
        string roadName = current + "To" + target;
        Debug.Log(roadName);
        // 거리 계산 / 타일 계산
        GameObject road = GameObject.Find("Map").transform.Find("Roads").transform.Find(roadName).gameObject;
        LineRenderer _r = road.GetComponent<LineRenderer>();

        //Get old Position Length
        Vector3[] newPos = new Vector3[_r.positionCount];
        //Get old Positions
        _r.GetPositions(newPos);
        float len = 0.0f;
        foreach (var c in newPos)
        {
            Debug.Log(c);
            len += Vector3.Magnitude(c);
        }
        Debug.Log(len);
    }
}
