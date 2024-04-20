using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GetObject : MonoBehaviour
{
    string scene;
    bool onSelected;
    Vector2 pos;
    RaycastHit2D hit;
    GameObject _uiCanvas;
    GameObject buildingPopup;

    WebRequest webRequest;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene().name;
        webRequest = FindObjectOfType<WebRequest>();
        if (name == "SampleScene") { _uiCanvas = GameObject.Find("popupCanvas"); Resources.Load <GameObject>("../../Prefabs/Building Panel"); }
    }

    // Update is called once per frame
    void Update()
    {
        if (name == "SampleScene")
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
                        string uri = CurrentInfo.serverURI + "building/" + clickObj.name;
                        Debug.Log(uri);
                        var result = webRequest.GetRequest(uri);
                        
                        while (result.MoveNext())
                        {
                            if (result.Current.GetType().Equals(typeof(BuildingInfo)))
                            {
                                objData = result.Current;
                                break;
                            }
                            else
                            {
                                objData = null;
                            }
                        }
                        GameObject popup = Instantiate(buildingPopup, _uiCanvas.transform, false);//prefab 적용 시
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
            else if (scene == "StartScene")
            {
                if (Input.GetMouseButton(0))
                {
                    
                }
            }
        }
    }
}
