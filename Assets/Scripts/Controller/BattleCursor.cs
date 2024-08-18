using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.CanvasScaler;

public class BattleCursor : MonoBehaviour
{
    [SerializeField]
    Camera _selectionArea;
    public Camera SelectionArea { set { _selectionArea = value; } }
    [SerializeField]
    RectTransform _selectionBox;
    public RectTransform SelectionBox { set { _selectionBox = value; } }
    private Vector2 _startPos;
    Dictionary<string, float> _lastSelectionBox = new Dictionary<string, float>() { { "w", 0 }, { "h", 0 }, { "x", 0 }, { "y", 0 }, { "z", 0 } };
    List<GameObject> selectedUnits = new();

    [SerializeField]
    bool _unpositionedUnit;
    public bool UnPositionedUnit { set { _unpositionedUnit = value; } }
    private static BattleCursor _instance;

    public static BattleCursor Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BattleCursor>();
                if (_instance == null)
                {
                    Debug.LogError("BattleCursor가 씬에 존재하지 않습니다.");
                }
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Battle"))
        {
            if (_unpositionedUnit)
            {
                ActiveUnpositionedUnit(
                    GameObject.Find("worldCanvas").transform.Find("Units").transform.Find("UnPositionedUnit").gameObject
                    );

                if (Input.GetMouseButtonDown(0))
                {
                    SetUnitPosition();
                }
                if (Input.GetKey(KeyCode.Escape))
                {
                    SwitchSelectMode(false);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                // mouse up
                if (Input.GetMouseButtonUp(0))
                {
                    ReleaseSelectionBox();
                }
                // mouse held down
                if (Input.GetMouseButton(0))
                {
                    UpdateSelectionBox(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
                if (Input.GetKey(KeyCode.Escape))
                {
                    ResetSelectedUnit();
                }
                // debug: draw ray
                //if (_lastSelectionBox["w"] > 0 || _lastSelectionBox["h"] > 0)
                //{
                //    DrawRay(_lastSelectionBox);
                //}
            }
        }
    }
    // called when we release the selection box
    void ReleaseSelectionBox()
    {
        SetLastSelectionBox(_selectionBox);
        _selectionBox.gameObject.SetActive(false);
        Vector2 min = _selectionBox.anchoredPosition - (_selectionBox.sizeDelta / 2);
        Vector2 max = _selectionBox.anchoredPosition + (_selectionBox.sizeDelta / 2);

        RaycastHit2D[] hits = RaycastEverything(
            new Vector3(_lastSelectionBox["x"], _lastSelectionBox["y"], _lastSelectionBox["z"])
            , new Vector3(0, 0, -10));
        hits = hits.Distinct().ToArray();
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            PolygonCollider2D pc = hit.transform.GetComponent<PolygonCollider2D>();

            if (pc)
            {
                // SetTrueSelection(pc.gameObject);
            }
        }
        ResetSelectionBox();
    }
    RaycastHit2D[] RaycastEverything(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        direction = direction.normalized;
        float originalMaxDistance = -maxDistance;

        float width = _lastSelectionBox["w"];
        float height = _lastSelectionBox["h"];
        float x = _lastSelectionBox["x"];
        float y = _lastSelectionBox["y"];
        float z = _lastSelectionBox["z"];

        float w = 0;
        float h = 0;

        List<RaycastHit2D> hits = new();
        while (w < width)
        {
            while (h < height)
            {
                RaycastHit2D[] hit;
                hit = Physics2D.RaycastAll(
                    new Vector3(x, y, z)
                    + new Vector3(w, h, 0)
                    - new Vector3(width / 2, height / 2)
                    , direction, originalMaxDistance, layerMask
                    //, new Vector3(0, 0, originalMaxDistance)
                );
                h += 0.5f;
                for (int i = 0; i < hit.Length; i++)
                {
                    if (!selectedUnits.Find(x => x.Equals(hit[i].collider.gameObject)))
                    {
                        hits.Add(hit[i]);
                        selectedUnits.Add(hit[i].collider.gameObject);
                        SetTrueSelection(hit[i].collider.gameObject);
                    }
                }
            }
            w += 0.5f;
            h = 0;
        }
        return hits.ToArray();
    }
    // called when we are creating a selection box
    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (_lastSelectionBox["w"] > 0  || _lastSelectionBox["h"] > 0)
        {
            ResetLastSelectionBox();
        }
        if (!_selectionBox.gameObject.activeInHierarchy)
        {
            _selectionBox.gameObject.SetActive(true);
        }
        float width = curMousePos.x - _startPos.x;
        float height = curMousePos.y - _startPos.y;
        _selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        _selectionBox.anchoredPosition = _startPos + new Vector2(width / 2, height / 2);
    }
    public Vector3 GetWorldPointUnderCursor()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    void ResetSelectionBox()
    {
        _selectionBox.sizeDelta = new Vector2(0, 0);
    }
    void ResetLastSelectionBox()
    {
        _lastSelectionBox["w"] = 0;
        _lastSelectionBox["h"] = 0;
        _lastSelectionBox["x"] = 0;
        _lastSelectionBox["y"] = 0;
        _lastSelectionBox["z"] = 0;
    }
    void SetLastSelectionBox(RectTransform r)
    {
        _lastSelectionBox["w"] = r.rect.width;
        _lastSelectionBox["h"] = r.rect.height;
        _lastSelectionBox["x"] = r.position.x;
        _lastSelectionBox["y"] = r.position.y;
        _lastSelectionBox["z"] = r.position.z;
    }
    void DrawRay(Dictionary<string, float> r)
    {
        float width = r["w"];
        float height = r["h"];
        float x = r["x"];
        float y = r["y"];
        float z = r["z"];

        float w = 0;
        float h = 0;
        while (w < width)
        {
            while (h < height)
            {
                Debug.DrawRay(new Vector3(x, y, z)
                    + new Vector3(w, h, 0)
                    - new Vector3(width / 2, height / 2)
                    , new Vector3(0, 0, -100), Color.green);
                h += 0.5f;
            }
            w += 0.5f;
            h = 0;
        }
    }
    void ResetSelectedUnit()
    {
        if (selectedUnits != null)
        {
            foreach (GameObject unit in selectedUnits)
            {
                SetFalseSelection(unit);
            }
            selectedUnits.Clear();
        }
    }
    void SetTrueSelection(GameObject unit)
    {
        if (unit == null) return;
        if (!unit.GetComponent<BattleUnit>().Selected)
        {
            unit.GetComponent<BattleUnit>().Selected = true;
        }
    }
    void SetFalseSelection(GameObject unit)
    {
        if (unit == null) return;
        if (unit.GetComponent<BattleUnit>().Selected)
        {
            unit.GetComponent<BattleUnit>().Selected = false;
        }
    }
    public void AddSelectedUnit(GameObject unit)
    {
        if (!selectedUnits.Find(x => x.gameObject == unit))
        {
            selectedUnits.Add(unit);
            unit.GetComponent<BattleUnit>().Selected = true;
        }
    }
    public void RemoveSelectedUnit(GameObject unit)
    {
        if (selectedUnits.Find(x => x.gameObject == unit))
        {
            selectedUnits.Remove(unit);
            unit.GetComponent<BattleUnit>().Selected = false;
        }
    }
    // Click Position Button
    public void PreviewUnitPosition()
    {
        // Update switch
        SwitchSelectMode(true);

        // Get UnitInfo Data
        GameObject _button = EventSystem.current.currentSelectedGameObject;
        // Next Step Parameter (Units-UnPositionedUnit)

        // Set UnitInfo
        SetUnitInfoVariables(
            GameObject.Find("worldCanvas").transform.Find("Units").transform.Find("UnPositionedUnit").gameObject
            , _button.GetComponent<BattleUnit>().Unit);
    }
    void ActiveUnpositionedUnit(GameObject upu)
    {
        //UnitInfo _v = Variables.Object(upu).Get<UnitInfo>("UnitInfo");
        //Debug.Log(_v.troops_quantity);
        // Follow Unit Image
        upu.transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f));
    }
    void SetUnitInfoVariables(GameObject o, UnitInfo u)
    {
        Variables.Object(o).Set("UnitInfo", u);
        Debug.Log("Unit Troops Qty: " + Variables.Object(o).Get<UnitInfo>("UnitInfo").troops_quantity);
    }
    void SetUnitPosition()
    {
        // Follow Unit Image
        Vector3 p = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f));
        GameObject unit = Resources.Load<GameObject>("Prefabs/BattleUnit");
        GameObject battleUnit = Instantiate(unit, GameObject.Find("worldCanvas").transform.Find("Units"), false);//prefab 적용 시
        try
        {
            UnitInfo u = GetUnitInfo();
            battleUnit.name = "BattleUnit_" + u.unit_index;
            u.unit_location = CurrentInfo.currentBattleLocationInfo.building_name;
            u.unit_location_x = p.x;
            u.unit_location_y = p.y;
            u.unit_location_z = p.z;
            u.unit_status = 1;
            UnitInfo _unit = CurrentInfo.currentChampion.units.Find(x => x.unit_index == u.unit_index);
            _unit = u;
            battleUnit.GetComponent<BattleUnit>().Unit = u;
            battleUnit.transform.position = p;
            //Insert battleUnit data into db

            //Update Unit List
            Debug.Log(u.unit_nickname + " / " + u.troops_quantity + " : unit is positioned");
            SwitchSelectMode(false);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    UnitInfo GetUnitInfo()
    {
        GameObject o = GameObject.Find("worldCanvas").transform.Find("Units").transform.Find("UnPositionedUnit").gameObject;
        UnitInfo u;
        UnitInfo _v = Variables.Object(o).Get<UnitInfo>("UnitInfo");
        u = _v;
        return u;
    }
    void SwitchSelectMode(bool selectMode)
    {
        // Switch Trigger
        BattleCursor.Instance.UnPositionedUnit = selectMode;
        // Set UnitInfo Data
        GameObject.Find("worldCanvas").transform.Find("Units").transform.Find("UnPositionedUnit").gameObject.SetActive(selectMode);

        // Switch Popup
        if (selectMode)
        {
            UIManager.Instance.HidePopup(GameObject.Find("popupCanvas").transform.Find("Unit Formation Window").gameObject.GetComponent<UIPopup>());
        }
        else
        {
            UIManager.Instance.ShowPopup(GameObject.Find("popupCanvas").transform.Find("Unit Formation Window").gameObject.GetComponent<UIPopup>());
        }
    }

    void AttackUnit(GameObject target)
    {
        BattleUnit _b = target.GetComponent<BattleUnit>();
        UnitInfo _u = _b.Unit;
        if (0 == 0)
        {

        }    
    }
}
