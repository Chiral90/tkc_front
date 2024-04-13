using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequest : MonoBehaviour
{
    private static WebRequest _instance;
    public string serverURI = "http://192.168.0.2:8180";

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
            else
            {
                Debug.Log(webRequest.responseCode);
                string result = webRequest.downloadHandler.text;
                BuildingInfo json = JsonUtility.FromJson<BuildingInfo>(result);
                yield return json;
            }
        }
    }
    public IEnumerator PostRequest(string uri, object obj)
    {
        WWWForm form = new WWWForm();
        //string id = "아이디";
        //string pw = "비밀번호";
        //form.AddField("Username", id);
        //form.AddField("Password", pw);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))  // 보낼 주소와 데이터 입력
        {
            yield return webRequest.SendWebRequest();  // 응답 대기

            if (webRequest.error == null)
            {
                Debug.Log(webRequest.downloadHandler.text);    // 데이터 출력
            }
            else
            {
                Debug.Log("error");
            }
        }
    }
}
