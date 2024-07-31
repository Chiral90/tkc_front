#define DEV_STATE

using System;
using UnityEngine;

public class BuildingEventManager : MonoBehaviour
{
    BuildingInfo _b;
    public BuildingInfo BuildingData {  get { return _b; } }
    Api api;
    private Camera _uiCamera;
    private PolygonCollider2D _collider;

    private bool _isMouseOver;
    private bool _isMousePushed;
    GameObject _target;

    [SerializeField]
    int _state = 0;
    public int State
    {
        get { return _state; }
        set { _state = value; }
    }
    
    private void Awake()
    {
        _uiCamera = FindObjectOfType<Camera>(); // 해당 오브젝트를 비추고 있는 카메라
        _collider = GetComponent<PolygonCollider2D>(); // 오브젝트의 Collider

        _target = _collider.gameObject;
    }

    private void Update()
    {
        var ray = _uiCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        
        if (hit.collider != null && hit.collider == _collider && !_isMouseOver)
        {
            _isMouseOver = true;
            OnMouseEnter();
        }else if (hit.collider != _collider && _isMouseOver)
        {
            _isMouseOver = false;
            OnMouseExit();
        }

        if (_isMouseOver) OnMouseOver();
        if (_isMouseOver && Input.GetMouseButtonDown(0))
        {
            _isMousePushed = true;
            OnMouseDown();
        }
        if (_isMousePushed && Input.GetMouseButton(0)) OnMouseDrag();
        if (_isMousePushed && Input.GetMouseButtonUp(0))
        {
            _isMousePushed = false;
            OnMouseUp();
            if(_isMouseOver) OnMouseUpAsButton();
        }
    }
    private void OnMouseDown()
    {
        // Debug.Log("OnMouseDown");
    }
    private void OnMouseUp()
    {
        // Debug.Log("OnMouseUp");
        OpenPopup(_target);
    }
    // private void OnMouseExit()
    // {
    //     Debug.Log("OnMouseExit");
    // }
    private void OnMouseOver()
    {
        // Debug.Log("OnMouseOver");
    }
    private void OnMouseDrag()
    {
        // Debug.Log("OnMouseDrag");
    }
    private void OnMouseUpAsButton()
    {
        // Debug.Log("OnMouseUpAsButton");
    }
    // private void OnMouseEnter()
    // {
    //     Debug.Log("OnMouseEnter");
    // }
    public void OnMouseEnter()
    {
        // Debug.Log("Mouse Enter");
        PolygonCollider2D pc = _target.GetComponent<PolygonCollider2D>();
        HighlightAroundCollider(_target, pc, Color.yellow, Color.red, 0.1f);
    }
    public void OnMouseExit()
    {
        // Debug.Log("Mouse Exit");
        DeleteHighlight(_target);
    }
    void HighlightAroundCollider(GameObject target, Component cpType, Color beginColor, Color endColor, float highlightSize = 0.3f)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = target.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = cpType.gameObject.AddComponent<LineRenderer>();
        }

        //2. Assign Material to the new Line Renderer
        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

        float zPos = 10f;//Since this is 2D. Make sure it is in the front

        if (cpType is PolygonCollider2D)
        {
            //3. Get the points from the PolygonCollider2D
            Vector2[] pColiderPos = (cpType as PolygonCollider2D).points;

            //Set color and width
            lineRenderer.SetColors(beginColor, endColor);
            lineRenderer.SetWidth(highlightSize, highlightSize);

            //4. Convert local to world points
            for (int i = 0; i < pColiderPos.Length; i++)
            {
                pColiderPos[i] = cpType.transform.TransformPoint(pColiderPos[i]);
            }

            //5. Set the SetVertexCount of the LineRenderer to the Length of the points
            lineRenderer.SetVertexCount(pColiderPos.Length + 1);
            for (int i = 0; i < pColiderPos.Length; i++)
            {
                //6. Draw the  line
                Vector3 finalLine = pColiderPos[i];
                finalLine.z = zPos;
                lineRenderer.SetPosition(i, finalLine);

                //7. Check if this is the last loop. Now Close the Line drawn
                if (i == (pColiderPos.Length - 1))
                {
                    finalLine = pColiderPos[0];
                    finalLine.z = zPos;
                    lineRenderer.SetPosition(pColiderPos.Length, finalLine);
                }
            }
        }

         //Not Implemented. You can do this yourself
        else if (cpType is BoxCollider2D)
        {

        }
    }
    void DeleteHighlight(GameObject target)
    {
        //1. Create new Line Renderer
        LineRenderer lineRenderer = target.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            
        }
        Destroy(target.GetComponent<LineRenderer>());
    }

    void OpenPopup(GameObject target)
    {
        BuildingInfo _bInfo = null;
        // clickObj.name의 팝업이 없으면 팝업 생성
        GameObject targetPopup;
        try
        {
            targetPopup = GameObject.Find("popupCanvas").transform.Find(target.name).gameObject;
            if (targetPopup != null)
            {
                return;
            }
        }
        catch (NullReferenceException e)
        {
#if DEV_STATE
            _bInfo = new BuildingInfo();
            if (target.name == "middlecastle1")
            {
                _bInfo.castellan = "chiral";
                _bInfo.population = 5000;
                _bInfo.morale = 100;
                _bInfo.status = 1;
                _bInfo.team = 1;
                _bInfo.stationed = new ChampionInfo[] { };
                _bInfo.building_name = "middlecastle1";
            }
            else if (target.name == "castle1")
            {
                _bInfo.castellan = "chiral";
                _bInfo.population = 1000;
                _bInfo.morale = 10;
                _bInfo.status = 0;
                _bInfo.team = 1;
                ChampionInfo _c1 = new ChampionInfo();
                _c1.champ_name = "chiral";
                _c1.createNewChampion("chiral", 3, 1);
                _c1.location = "castle1";
                _bInfo.stationed = new ChampionInfo[] { _c1 };
                _bInfo.building_name = "castle1";
            }
            else if (target.name == "castle2")
            {
                _bInfo.castellan = "enantiomer";
                _bInfo.population = 50000;
                _bInfo.morale = 10;
                _bInfo.status = 0;
                _bInfo.team = 2;
                ChampionInfo _c1 = new ChampionInfo();
                _c1.champ_name = "enantiomer";
                _c1.createNewChampion("enantiomer", 3, 2);
                _c1.location = "castle2";
                _bInfo.stationed = new ChampionInfo[] { _c1 };
                _bInfo.building_name = "castle2";
            }
            else if (target.name == "middlecastle2")
            {
                _bInfo.castellan = "";
                _bInfo.population = 1000;
                _bInfo.morale = 50;
                _bInfo.status = 0;
                _bInfo.team = 0;
                _bInfo.stationed = new ChampionInfo[] { };
                _bInfo.building_name = "middlecastle2";
            }
            Canvas _uiCanvas = GameObject.Find("popupCanvas").gameObject.GetComponent<Canvas>();
            GameObject buildingPopup = Resources.Load<GameObject>("Prefabs/Building Panel");

            GameObject popup = Instantiate(buildingPopup, _uiCanvas.transform, false);//prefab 적용 시
            popup.name = target.name;
            UIManager.Instance.OpenBuildingPopup(popup.GetComponentInChildren<UIPopup>(), _bInfo);
#else
            var objData = new object();
            api.routePath = "/building/" + target.name;
            var result = StaticCoroutine.StartStaticCoroutine(api.GetRequest(onSuccess: (result) =>
            {
                Debug.Log(result);
                _bInfo = JsonUtility.FromJson<BuildingInfo>(result);
                Debug.Log(_bInfo.stationed);
                Canvas _uiCanvas = GameObject.Find("popupCanvas").gameObject.GetComponent<Canvas>();
                GameObject buildingPopup = Resources.Load<GameObject>("Prefabs/Building Panel");
                
                GameObject popup = Instantiate(buildingPopup, _uiCanvas.transform, false);//prefab 적용 시
                popup.name = target.name;
                UIManager.Instance.OpenBuildingPopup(popup.GetComponentInChildren<UIPopup>(), _bInfo);
                // do something
            }, onFailure: (error) =>
            {
                Debug.Log(error);
                // do something
            }));
#endif
            _b = _bInfo;
        }
    }
}