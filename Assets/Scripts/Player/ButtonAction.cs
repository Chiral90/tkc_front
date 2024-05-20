using System;
using System.Collections;
using TMPro;
using UnityEditor.PackageManager;
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

        // DontDestroyOnLoad(this);
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
        Debug.Log("1 : " + this.gameObject.activeInHierarchy);
        Debug.Log("2 : " + this.gameObject.activeSelf);
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
        // string _champStr = JsonUtility.ToJson(_champ);
        // Debug.Log(_champStr);
        api.routePath = "/champ/create";
        WWWForm _champWWW = _champ.createWWWForm();
        var result = StaticCoroutine.StartStaticCoroutine(api.PostRequest(_champWWW, onSuccess: (result) =>
        {
            // Debug.Log(result);
            GameObject _button = EventSystem.current.currentSelectedGameObject;
            GameObject.Find("Canvas").transform.Find("BackGround").Find("Create Champion").gameObject.SetActive(false);
            _button.transform.parent.gameObject.SetActive(false);
        }, onFailure: (error) =>
        {
            Debug.LogError(error);
        }));
    }

    public void cancelCreateChampion()
    {
        GameObject.Find("Canvas").transform.Find("Champion Data").gameObject.SetActive(false);
    }

    public void openChamplist()
    {
        try
        {
            Debug.Log("1 : " + this.gameObject.activeInHierarchy);
            Debug.Log("2 : " + this.gameObject.activeSelf);
            GameObject _button = EventSystem.current.currentSelectedGameObject;
            GameObject _subPanel = _button.transform.parent.gameObject.transform.parent.Find("Building Panel Sub").gameObject;
            // Debug.Log(string.Format("subPanel: {0}", _subPanel.activeSelf));
            switchActive(_subPanel);
            _subPanel.transform.Find("Building Detail Panel").gameObject.SetActive(false);
            _subPanel.transform.Find("Building Champ List Panel").gameObject.SetActive(true);
            // print champion list
            UIPopup _mainPanelData = _subPanel.transform.parent.gameObject.GetComponent<UIPopup>();
            GameObject _content = _subPanel.transform.Find("Building Champ List Panel").transform.Find("Building Stationed Champions").transform.Find("Viewport").transform.Find("Content").gameObject;
            GameObject champData = _content.transform.Find("champ").gameObject;
            int idx = 0;
            foreach (ChampionInfo c in _mainPanelData.data.stationed)
            {
                if (idx == 0)
                {
                    champData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = c.champ_name;
                    champData.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = c.ChampType;
                    champData.transform.Find("Leadership").GetComponent<TextMeshProUGUI>().text = c.leadership.ToString();
                }
                else
                {
                    GameObject _champData = Instantiate(champData, _content.transform);
                    _champData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = c.champ_name;
                    _champData.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = c.ChampType;
                    _champData.transform.Find("Leadership").GetComponent<TextMeshProUGUI>().text = c.leadership.ToString();
                }
                idx++;
            }
        } catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    public void openDetail()
    {
        try
        {
            Debug.Log("1 : " + this.gameObject.activeInHierarchy);
            Debug.Log("2 : " + this.gameObject.activeSelf);
            GameObject _button = EventSystem.current.currentSelectedGameObject;
            GameObject _subPanel = _button.transform.parent.gameObject.transform.parent.Find("Building Panel Sub").gameObject;
            // Debug.Log(string.Format("subPanel: {0}", _subPanel.activeSelf));
            switchActive(_subPanel);
            _subPanel.transform.Find("Building Champ List Panel").gameObject.SetActive(false);
            _subPanel.transform.Find("Building Detail Panel").gameObject.SetActive(true);
            UIPopup _mainPanelData = _subPanel.transform.parent.gameObject.GetComponent<UIPopup>();
            _subPanel.transform.Find("Building Detail Panel").transform.Find("Building Food").GetComponent<TextMeshProUGUI>().text = _mainPanelData.data.food.ToString();
            _subPanel.transform.Find("Building Detail Panel").transform.Find("Building Morale").GetComponent<TextMeshProUGUI>().text = _mainPanelData.data.morale.ToString();
        } catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    public void enterBuilding()
    {
        Debug.Log("1 : " + this.gameObject.activeInHierarchy);
        Debug.Log("2 : " + this.gameObject.activeSelf);
        GameObject _button = EventSystem.current.currentSelectedGameObject;
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
        else if (text.Equals("선택"))
        {
            Debug.Log("Select location");
            updateLocation(targetName);
            return;
        }
        updateDestination(targetName);
        calcDistance(targetName);
        // try {
        //     updateDestination(targetName);
        //     calcDistance(targetName);
        // } catch (Exception e) {
        //     Debug.LogError(e);
        // }
    }

    void calcDistance(string targetName)
    {
        // 현재 위치
        string current = CurrentInfo.currentChampion.location;
        // 목표 위치
        string target = targetName;
        if (!current.Equals(""))
        {
            string roadName = current + "To" + target;
            Debug.Log("Select Road: " + roadName);
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
                len += Vector3.Magnitude(c);
            }
            // Debug.Log(len);
        }
    }
    void switchActive(GameObject g)
    {
        if (!g.activeInHierarchy)
        {
            g.SetActive(true);
        }
    }
    void updateDestination(string destination)
    {
        Debug.Log("updateDest");
        api.routePath = string.Format("/champ/updateDestination/{0}", destination);
        WWWForm _f = CurrentInfo.currentChampion.createWWWForm();
        var result = StaticCoroutine.StartStaticCoroutine(api.PostRequest(_f, onSuccess: (result) =>
        {
            // do something
            Debug.Log(result);
        }, onFailure: (error) =>
        {
            // do something
        }));
    }
    void updateLocation(string location)
    {
        Debug.Log("updateLoca");
        api.routePath = string.Format("/champ/updateLocation/{0}", location);
        WWWForm _f = CurrentInfo.currentChampion.createWWWForm();
        var result = StaticCoroutine.StartStaticCoroutine(api.PostRequest(_f, onSuccess: (result) =>
        {
            // do something
            Debug.Log(result);
        }, onFailure: (error) =>
        {
            // do something
        }));
    }
    // IEnumerator updateDest(string destination)
    // {
    //     Debug.Log("updateDest");
    //     WWWForm _f = CurrentInfo.currentChampion.createWWWForm();
    //     yield return StaticCoroutine.StartPostStaticCoroutine(string.Format("/champ/updateDestination/{0}", destination), _f);
    // }
    // IEnumerator updateLoca(string location)
    // {
    //     Debug.Log("updateLoca");
    //     WWWForm _f = CurrentInfo.currentChampion.createWWWForm();
    //     yield return StaticCoroutine.StartPostStaticCoroutine(string.Format("/champ/updateLocation/{0}", location), _f);
    // }
}
