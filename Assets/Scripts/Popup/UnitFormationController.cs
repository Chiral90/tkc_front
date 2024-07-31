using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UnitFormationController : MonoBehaviour
{
    TMP_InputField _unitQuantity;
    TMP_Dropdown _unitTypeDropdown;
    UnityEngine.UI.Slider _troopsSlider;
    GameObject _unitsFormationList;
    GameObject _unitData;
    bool _isPositioning;
    // Start is called before the first frame update
    void Start()
    {
        // Set Unit Qunatity
        _unitQuantity = this.transform.Find("Unit").Find("Unit Quantity").GetComponent<TMP_InputField>();
        _unitQuantity.text = CurrentInfo.currentChampion.units == null ? "0" : CurrentInfo.currentChampion.units.Count.ToString();
        //_unitSlider = this.transform.Find("Unit").Find("Unit Quantity Slider").GetComponent<UnityEngine.UI.Slider>();
        _unitTypeDropdown = this.transform.Find("Unit").Find("Unit Type").GetComponent<TMP_Dropdown>();
        _troopsSlider = this.transform.Find("Troops").Find("Troops Quantity Slider").GetComponent<UnityEngine.UI.Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPositioning)
        {

        }
    }

    public void ApplyUnitFormation()
    {
        if (CurrentInfo.currentChampion.champ_type == 0) return;

        UnitInfo _u = new UnitInfo();
        try
        {
            _u.unit_index = CurrentInfo.currentChampion.units.Count;
        }
        catch (NullReferenceException e)
        {
            _u.unit_index = 0;
        }
        _u.unit_nickname =
            EventSystem.current.currentSelectedGameObject.transform.parent
            .Find("Unit").Find("Unit Nickname").GetComponent<TMP_InputField>().text;
        _u.unit_type = _unitTypeDropdown.value;
        _u.unit_defence = _troopsSlider.value;
        _u.unit_attack = _troopsSlider.value;
        _u.unit_status = 0;
        _u.troops_quantity = Convert.ToInt32(_troopsSlider.value);
        _u.unit_morale = 0;
        if (CurrentInfo.currentChampion.units != null)
        {
            CurrentInfo.currentChampion.units.Add(_u);
        }
        else
        {
            CurrentInfo.currentChampion.units = new();
            CurrentInfo.currentChampion.units.Add(_u);
        }
        SetUnitsFormationList();
        ChangeValueInteractively();
        _unitQuantity.text = CurrentInfo.currentChampion.units.Count.ToString();
    }
    public void RevertUnitFormation()
    {
        //_unitSlider.value = _unitSlider.minValue;
        _troopsSlider.value = _troopsSlider.minValue;
        _unitTypeDropdown.value = 0;
    }
    void ChangeValueInteractively()
    {
        List<UnitInfo> _u = CurrentInfo.currentChampion.units;
        int _troopsAmount = 0;
        if (_u != null && _u.Count > 0)
        {
            foreach (UnitInfo u in _u)
            {
                _troopsAmount += u.troops_quantity;
            }
        }
        //_unitSlider.maxValue = Convert.ToInt32(
        //        Math.Ceiling(CurrentInfo.currentChampion.leadership - _troopsAmount / (decimal)GameManager.Instance.UnitMinVal)
        //        );
        _troopsSlider.maxValue = CurrentInfo.currentChampion.leadership - _troopsAmount;
    }
    void SetUnitsFormationList()
    {
        Debug.Log("Set Unit Formation");
        GameObject _pgo = this.transform.Find("UnitFormationList").Find("Scroll View").Find("Viewport").Find("Content").gameObject;
        
        if (CurrentInfo.currentChampion.units == null || CurrentInfo.currentChampion.units.Count == 0) return;
        Debug.Log("Unit Count: " + CurrentInfo.currentChampion.units.Count);
        foreach (UnitInfo c in CurrentInfo.currentChampion.units)
        {
            GameObject _data;
            if (_pgo.transform.Find("Unit_" + c.unit_index))
            {
                _data = _pgo.transform.Find("Unit_" + c.unit_index).gameObject;
            }
            else
            {
                _data = Instantiate(Resources.Load<GameObject>("Prefabs/UnitListItem"), _pgo.transform);
                _data.name = "Unit_" + c.unit_index;
            }
            SetUnitData(_data, c);
        }
    }

    void SetUnitData(GameObject d, UnitInfo c)
    {
        d.transform.Find("Name").gameObject.SetActive(true);
        d.transform.Find("Type").gameObject.SetActive(true);
        d.transform.Find("Troops").gameObject.SetActive(true);
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Battle"))
        {
            d.transform.Find("Position").gameObject.SetActive(true);
        }
        d.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = c.unit_nickname;
        d.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = c.UnitType;
        d.transform.Find("Troops").GetComponent<TextMeshProUGUI>().text = c.troops_quantity.ToString();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Battle"))
        {
            if (c.unit_status > 0)
            {
                d.transform.Find("Position").GetComponent<UnityEngine.UI.Button>().enabled = false;
                d.transform.Find("Position").GetComponent<UnityEngine.UI.Button>()
                    .transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = c.unit_location;
            }
            else
            {
                d.transform.Find("Position").GetComponent<UnityEngine.UI.Button>().enabled = true;
            }
        }
        d.transform.Find("Position").GetComponent<BattleUnit>().Unit = c;
    }
    void OnEnable()
    {
        SetUnitsFormationList();
    }
}
