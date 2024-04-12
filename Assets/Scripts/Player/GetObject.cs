using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.Networking;

public class GetObject : MonoBehaviour
{
    bool onSelected;
    Vector2 pos;
    RaycastHit2D hit;
    public GameObject canvas;
    public GameObject buildingPopup;

    BuildingInfo _binfo = new BuildingInfo();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            if (hit.collider != null)
            {
                if (!onSelected)
                {
                    onSelected = true;
                    GameObject clickObj = hit.transform.gameObject;
                    var objData = new object();
                    string uri = "http://192.168.0.2:8180/building/" + clickObj.name;
                    Debug.Log(uri);
                    var result = GetRequest(uri);
                    
                    while (result.MoveNext())
                    {
                        if (result.Current.GetType().Equals(_binfo.GetType()))
                        {
                            objData = result.Current;
                            break;
                        }
                        else
                        {
                            objData = null;
                        }
                    }
                    GameObject popup = Instantiate(buildingPopup, canvas.transform, false);//prefab 적용 시
                    popup.AddComponent<UIPopup>();
                    UIManager.Instance.OpenPopup(popup.GetComponentInChildren<UIPopup>(), objData);
                }
            }
            else
            {
                if (onSelected)
                {
                    onSelected = false;
                    Debug.Log("x");
                }
            }
        }
    }
    IEnumerator GetRequest(string uri)
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
}
