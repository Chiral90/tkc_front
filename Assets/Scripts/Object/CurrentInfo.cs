using System;
using System.Collections;
using System.Linq;
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
            Debug.Log(string.Format("currentID: {0} / currentChampion: {1}", currentID, currentChampion.champ_name));
        }
    }
    void Awake()
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
        
        var _userData = GetUserData(); 
        try
        {
            while (_userData.MoveNext())
            {
                Debug.Log(_userData.Current);
                if (_userData.Current.ToString().Contains("champ_name")
                && _userData.Current.ToString().Contains("champ_type")
                && _userData.Current.ToString().Contains("leadership")
                && _userData.Current.ToString().Contains("own_castles"))
                {
                    currentChampion = JsonUtility.FromJson<ChampionInfo>(_userData.Current.ToString());
                }
                if (currentChampion == null)
                {
                    GameObject.Find("Canvas").transform.Find("BackGround").transform.Find("Create Champion").gameObject.SetActive(true);
                }
            }  
        }
        catch (NullReferenceException e) 
        {
            Debug.Log(e.Data.ToString());
            GameObject.Find("Canvas").transform.Find("BackGround").transform.Find("Create Champion").gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (currentID == null || currentID.Equals(""))
            {
                GetId();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            SceneManager.LoadScene(0);
        }
        
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
            webRequest.SetRequestHeader("Cookie", string.Format("id={0}", CurrentInfo.currentID));
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError)
            {

            }
            else
            {
                Debug.Log(webRequest.responseCode);
                Debug.Log(webRequest.downloadHandler.text);
                yield return webRequest.downloadHandler.text;
            }
        }
    }
}
