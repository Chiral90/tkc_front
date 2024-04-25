using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetObject : MonoBehaviour
{
    string scene;
    bool onSelected;
    Vector2 pos;
    RaycastHit2D hit;
    Api api;
    [SerializeField] GameObject _uiCanvas;
    [SerializeField] GameObject buildingPopup;

    WebRequest webRequest;
    // Start is called before the first frame update
    void Start()
    {
        // webRequest = FindObjectOfType<WebRequest>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetMouseButton(0))
            {
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

                if (hit.collider != null)
                {
                    BuildingInfo _bInfo = null;
                    if (!onSelected)
                    {
                        onSelected = true;
                        GameObject clickObj = hit.transform.gameObject;
                        Debug.Log(clickObj.name);
                        var objData = new object();
                        //string uri = CurrentInfo.serverURI + "/building/" + clickObj.name;
                        api.routePath = "/building/" + clickObj.name;
                        StartCoroutine(api.GetRequest(onSuccess: (result) =>
                        {
                            _bInfo = JsonUtility.FromJson<BuildingInfo>(result);
                            Debug.Log("get request success : " + _bInfo.buildingName + "/" + _bInfo.buildingType);
                            GameObject popup = Instantiate(buildingPopup, _uiCanvas.transform, false);//prefab 적용 시
                            popup.AddComponent<UIPopup>();
                            UIManager.Instance.OpenBuildingPopup(popup.GetComponentInChildren<UIPopup>(), _bInfo);
                            // do something
                        }, onFailure: (error) =>
                        {
                            Debug.Log(error);
                            // do something
                        }));
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
