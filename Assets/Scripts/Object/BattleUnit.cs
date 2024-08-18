#define DEV_STATE

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
    ProgressBarPro _hp;
    float _troopsMaxQuantity;

    Vector2 _moveDirection;
    public Vector2 MoveDirection
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
        if (SceneManager.GetActiveScene().name.Contains("Battle"))
        {
            if (!this.transform.name.Equals("Position") && !(this.transform.parent.name.Equals("UnPositionedUnit")))
            {
                _flagCanvas = this.transform.Find("UnitFlag80x80").Find("FlagCanvas").GetComponent<Canvas>();
                _flagCanvas.renderMode = RenderMode.WorldSpace;
                _flagCanvas.overrideSorting = true;
                _flagCanvas.worldCamera = Camera.main;
                _eventManager = this.gameObject.GetComponent<BattleUnitEventManager>();
                if (!this.name.Equals("Position"))
                {
                    _hp = this.transform.Find("State").transform.Find("HP").Find("HorizontalBoxGradient").GetComponent<ProgressBarPro>();
                    SetUnitState();
                    _troopsMaxQuantity = _unit.troops_quantity;
                }
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
        float s = 1f;
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
    void CalcDefaultAttak()
    {
        float a = _unit.troops_quantity;
        // unit type
        if (_unit.unit_type == 0)
        {
            a *= 0;
        }
        else if (_unit.unit_type == 1)
        {
            a *= 0.1f;
        }
        else if (_unit.unit_type == 2)
        {
            a *= 0.2f;
        }
        else if (_unit.unit_type == 3)
        {
            a *= 1.8f;
        }
        else if (_unit.unit_type == 4)
        {
            a *= 0.5f;
        }
        _unit.unit_attack = a;
    }

    void SetUnitState()
    {
        _hp.SetValue(_unit.troops_quantity, _unit.troops_quantity, false);
        CalcDefaultMoveSpeed();
        CalcDefaultAttak();
    }

    public void Attack(Collider2D col)
    {
        Debug.Log("Attack: " + col.name + " / damage: " + (int)_unit.unit_attack);
#if DEV_STATE
        col.gameObject.GetComponent<BattleUnit>().Attacked((int)_unit.unit_attack);
#else
        // Send damage battle socket server
#endif
    }
    public void Attacked(int damage)
    {
        Debug.Log(this.name + " is Attacked");
        _unit.troops_quantity -= damage;
        UpdateUnitState();
        if (_unit.troops_quantity <= 0)
        {
            Eliminated();
        }
    }
    void Eliminated()
    {
        DestroyImmediate(gameObject);
    }
    void UpdateUnitState()
    {
        UpdateUnitHP();
    }
    void UpdateUnitHP()
    {
        _hp.SetValue(_unit.troops_quantity / _troopsMaxQuantity);
    }
    void GetExp()
    {

    }
}
