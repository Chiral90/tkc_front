using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SampleBattleScene : MonoBehaviour
{
    Vector3 _unpositionedLastPos;
    BattleCursor _battleCursor;
    //BuildingInfo _b;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitialScene()
    {
        bool _areThereStandbyUnit = CheckStandbyUnit();

        if (_areThereStandbyUnit)
        {
            // Set Unit Location
            Debug.Log("Set Unit Position");
        }
        SetBattleCursor();
        PositioningUnits();
        //_b = CurrentInfo.currentBattleLocationInfo;
    }
    void SetBattleCursor()
    {
        Debug.Log("Set Battle Cursor");
        _battleCursor = GameObject.Find("GameManager").transform.Find("BattleCursor").GetComponent<BattleCursor>();
        _battleCursor.SelectionArea = GameObject.Find("worldCanvas").transform.Find("SelectionBox").Find("SelectionCamera").GetComponent<Camera>();
        _battleCursor.SelectionBox = GameObject.Find("worldCanvas").transform.Find("SelectionBox").GetComponent<RectTransform>();
    }
    bool CheckStandbyUnit()
    {
        bool result = false;
        try
        {
            if (CurrentInfo.currentChampion.units == null) return false;

            foreach (UnitInfo u in CurrentInfo.currentChampion.units)
            {
                if (u.unit_status < 1)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
            return false;
        }
    }
    public void SetUnitPosition(Canvas c)
    {
        Canvas _uiCanvas = GameObject.Find("GameManager").transform.Find("userCanvas").gameObject.GetComponent<Canvas>();
        GameObject unitList = Resources.Load<GameObject>("Prefabs/UnitFormationList");

        GameObject popup = Instantiate(unitList, _uiCanvas.transform, false);//prefab Àû¿ë ½Ã
        popup.name = "Unit List";
        UIManager.Instance.OpenUnitFormationPopup(popup.GetComponentInChildren<UIPopup>());
        try
        {
            foreach (UnitInfo u in CurrentInfo.currentChampion.units)
            {
                if (u.unit_status < 1)
                {

                }
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
        }
    }
    void PositioningUnits()
    {
        if (CurrentInfo.currentChampion == null) return;
        GameObject _units = GameObject.Find("worldCanvas").transform.Find("Units").gameObject;
        // other users units

        // current user units
        foreach (UnitInfo u in CurrentInfo.currentChampion.units)
        {
            if (u.unit_status > 0)
            {
                GameObject prefab = Resources.Load<GameObject>("Prefabs/BattleUnit");
                GameObject unit = Instantiate(prefab, _units.transform, false);
                unit.GetComponent<BattleUnit>().Unit = u;
                unit.transform.position = new Vector3(u.unit_location_x, u.unit_location_y, u.unit_location_z);
            }
        }
    }
    //save state

    //reserve action
    public void ReserveAction()
    {
        GameObject _units = GameObject.Find("worldCanvas").transform.Find("Units").gameObject;

        for (int i = 1; i < _units.transform.childCount; i++)
        {
            if (_units.transform.GetChild(i))
            {
                BattleUnit _b = _units.transform.GetChild(i).GetComponent<BattleUnit>();
                BattleUnitEventManager _e = _b.gameObject.GetComponent<BattleUnitEventManager>();
                
            }
        }
    }
}
