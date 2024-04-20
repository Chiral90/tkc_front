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

    public IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
            }
            else if (webRequest.GetResponseHeader("id") is not null)
            {
                yield return webRequest.GetResponseHeader("id");
            }
            else
            {
                Debug.Log(webRequest.responseCode);
                string result = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                yield return result;
            }
        }
    }
    public IEnumerator PostRequest(string uri, WWWForm obj)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, obj))  // 보낼 주소와 데이터 입력
        {
            webRequest.SetRequestHeader("Cookie", string.Format("id={0}", CurrentInfo.currentID));
            Debug.Log("POST champdata... / id : " + CurrentInfo.currentID);
            yield return webRequest.SendWebRequest();  // 응답 대기

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);    // 데이터 출력
            }
        }
    }
}
