using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleUnit : MonoBehaviour
{
    UnitInfo _unit;
    public UnitInfo Unit
    {
        set { _unit = value; }
        get { return _unit; }
    }
    Canvas _flagCanvas;

    public float moveSpeed;
    public Vector3 _moveDirection;
    public Vector3 MoveDirection
    {
        get { return _moveDirection; }
        set { _moveDirection = value; }
    }
    [SerializeField]
    bool _selected = false;
    public bool Selected
    {
        get { return _selected; }
        set { _selected = value; }
    }
    bool _finishHighlight = true;
    public bool FinishHighlight
    {
        get { return _finishHighlight; }
        set { _finishHighlight = value; }
    }

    BattleUnitEventManager _eventManager;
    
    // Start is called before the first frame update
    void Start()
    {
        CalcDefaultMoveSpeed();
        if (SceneManager.GetActiveScene().name.Contains("Battle"))
        {
            if (!this.transform.name.Equals("Position") && !(this.transform.parent.name.Equals("UnPositionedUnit")))
            {
                _flagCanvas = this.transform.Find("UnitFlag80x80").Find("FlagCanvas").GetComponent<Canvas>();
                _flagCanvas.renderMode = RenderMode.WorldSpace;
                _flagCanvas.overrideSorting = true;
                _flagCanvas.worldCamera = Camera.main;
                _eventManager = this.gameObject.GetComponent<BattleUnitEventManager>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.transform.name.Equals("Position") && !(this.transform.parent.name.Equals("UnPositionedUnit")))
        {
            if (_selected)
            {
                if (_finishHighlight)
                {
                    AbleToHighlightSelectedUnit();
                }
            }
            if (!_selected)
            {
                if (!_finishHighlight)
                {
                    UnableToHighlightSelectedUnit();
                }
            }
            if (this.transform.Find("Path").GetComponent<LineRenderer>() != null)
            {
                if (!_eventManager.IsMoving)
                {
                    _eventManager.IsMoving = true;
                }
                else
                {
                    if (_eventManager.Path == null || _eventManager.Path.Count == 0)
                    {
                        _eventManager.IsMoving = false;
                    }
                }
            }
        }
    }
    void AbleToHighlightSelectedUnit()
    {
        if (_selected)
        {
            _eventManager.AbleToHighlightSelectedUnit();
            _finishHighlight = false;
        }
    }
    void UnableToHighlightSelectedUnit()
    {
        if (!_selected)
        {
            //result = SwitchUnitSelected();
            _eventManager.UnableToHighlightSelectedUnit();
            _finishHighlight = true;
        }
    }
    bool SwitchUnitSelected()
    {
        _selected = !_selected;
        return _selected;
    }
    void MoveUnit()
    {

    }
    void CalcDefaultMoveSpeed()
    {
        float s = 0.01f;
        // unit type
        if (_unit.unit_type == 0)
        {
            s *= 1f;
        }
        else if (_unit.unit_type == 1)
        {
            s *= 1f;
        }
        else if (_unit.unit_type == 2)
        {
            s *= 2f;
        }
        else if (_unit.unit_type == 3)
        {
            s *= 1.8f;
        }
        else if (_unit.unit_type == 4)
        {
            s *= 0.8f;
        }
        moveSpeed = s;
    }
}
