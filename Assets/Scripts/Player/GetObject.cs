using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GetObject : MonoBehaviour
{
    string scene;
    bool onSelected;
    Vector2 pos;
    RaycastHit2D hit;
    GameObject _uiCanvas;
    GameObject buildingPopup;

    WebRequest webRequest;

    BuildingInfo _binfo = new BuildingInfo();
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
                        string uri = webRequest.serverURI + "building/" + clickObj.name;
                        Debug.Log(uri);
                        var result = webRequest.GetRequest(uri);
                        
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
    public void clikcedButton()
    {
        string _clickedButton = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(_clickedButton);
        if (_clickedButton == "Start Game") { SceneManager.LoadScene("SampleScene"); }
        else if (_clickedButton == "Create Champion") {
            GameObject.Find("Canvas").transform.Find("Champion Data").gameObject.SetActive(true);
        }
    }
}
