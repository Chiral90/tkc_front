using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    private static WebRequest _instance;
    public static WebRequest Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WebRequest>();
                if (_instance == null)
                {
                    Debug.LogError("WebRequest 씬에 존재하지 않습니다.");
                }
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetRequest());
        //StartCoroutine(PostRequest());
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // public IEnumerator GetRequest(string uri)
    // {
    //     using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    //     {
    //         yield return webRequest.SendWebRequest();

    //         if (webRequest.result == UnityWebRequest.Result.ConnectionError)
    //         {
    //             Debug.LogError(webRequest.error);
    //         }
    //         else
    //         {
    //             Debug.Log(webRequest.responseCode);
    //             byte[] resultRaw = webRequest.downloadHandler.data; //
    //             string result = System.Text.Encoding.UTF8.GetString(resultRaw); //
    //             Debug.Log(result);
    //             yield return result;
    //         }
    //     }
    // }
    public IEnumerator PostRequest(string uri, WWWForm obj)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, obj))  // 보낼 주소와 데이터 입력
        {
            webRequest.SetRequestHeader("Cookie", string.Format("id={0}", CurrentInfo.currentID));
            yield return webRequest.SendWebRequest();  // 응답 대기

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                yield return webRequest.responseCode;
                Debug.Log(webRequest.downloadHandler.text);    // 데이터 출력
            }
        }
    }
    public BuildingInfo GetBuildingData(string uri)
    {
        BuildingInfo bInfo = null;
        StartCoroutine(GetRequest(uri, (request) =>
        {
            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {
                byte[] resultRaw = request.downloadHandler.data; //
                string result = System.Text.Encoding.UTF8.GetString(resultRaw); //
                Debug.Log(result);
                bInfo = JsonUtility.FromJson<BuildingInfo>(result);
            }
            else
            {
                Debug.Log("[Error]:" + request.responseCode + request.error);
                bInfo = null;
            }
        }));
        return bInfo;
    }
    IEnumerator GetRequest(string url, Action<UnityWebRequest> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        callback(request);
    }
}
