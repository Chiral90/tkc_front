using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentInfo : MonoBehaviour
{
    public static string serverURI = "http://192.168.0.2:8181";
    public static string currentID;
    public static ChampionInfo currentChampion;
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            //Debug.Log(string.Format("currentID: {0} / currentChampion: {1}", currentID, currentChampion.champ_name));
            // if (currentChampion.champ_name != "")
            // {
            //     setUserPanel();
            // }
        }
        else
        {
            string _id;
            //Debug.Log(Application.absoluteURL);
            //var _cookiesRes = GetId();
            //while (_cookiesRes.MoveNext())
            //{
            //    Debug.Log(_cookiesRes.Current.ToString());

                //_id = _cookiesRes.Current.ToString();
            //}
            _id = "dozosos";

            currentID = _id;
            StartCoroutine(GetUserData());
            Debug.Log("Next Line of the Start Coroutine...");
            //var _userData = GetUserData();
            try
            {
                 
            }
            catch (NullReferenceException e) 
            {
                Debug.Log(e.Data.ToString());
                GameObject.Find("Canvas").transform.Find("BackGround").transform.Find("Create Champion").gameObject.SetActive(true);
            }
        }
    }
    void Awake()
    {
        Debug.Log("Awake");
        DontDestroyOnLoad(this);
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

    void setUserPanel()
    {
        Debug.Log(CurrentInfo.currentChampion.champ_name);
        Debug.Log(CurrentInfo.currentChampion.champ_type.ToString());
        var _panel = GameObject.Find("userCanvas").transform.Find("userPanel").gameObject;
        Debug.Log(_panel.name);
        var _nameObj = _panel.transform.Find("Champion Name").GetComponent<TMP_Text>();
        var _typeObj = _panel.transform.Find("Champion Type").GetComponent<TMP_Text>();
        var _lsObj = _panel.transform.Find("Leadership").GetComponent<TMP_Text>();
        _nameObj.text = CurrentInfo.currentChampion.champ_name;
        _typeObj.text = CurrentInfo.currentChampion.champ_type.ToString();
        _lsObj.text = CurrentInfo.currentChampion.leadership.ToString();
    }

    IEnumerator GetId()
    {
        //get id from browser cookies
        using (UnityEngine.Networking.UnityWebRequest webRequest = UnityEngine.Networking.UnityWebRequest.Get(Application.absoluteURL))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
            {

            } else if (webRequest.GetResponseHeader("id") != null)
            {
                currentID = webRequest.GetResponseHeader("id");
            }
            else
            {
                Debug.Log("else");
                yield return "";
            }
        }
    }

    IEnumerator GetUserData()
    {
        //get champ data from server
        using (UnityEngine.Networking.UnityWebRequest webRequest = UnityEngine.Networking.UnityWebRequest.Get(serverURI + "/champ"))
        {
            //webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Cookie", string.Format("id={0}", CurrentInfo.currentID));
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
            {

            }
            else
            {
                if (webRequest.isDone)
                {
                    Debug.Log(webRequest.responseCode);
                    byte[] resultRaw = webRequest.downloadHandler.data; //
                    string result = System.Text.Encoding.Default.GetString(resultRaw); //
                    currentChampion = JsonUtility.FromJson<ChampionInfo>(result);
                    Debug.Log(currentChampion.champ_name);
                }
                else
                {
                    Debug.Log("Error... data couldn't get");
                }
            }
        }
    }
}
