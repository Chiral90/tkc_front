using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuantitySliderController : MonoBehaviour
{
    Slider _thisSlider;
    //Slider _unitSlider;
    Slider _troopsSlider;
    TMP_InputField _text;
    
    // Start is called before the first frame update
    void Start()
    {
        _thisSlider = GetComponent<Slider>();
        _text = this.transform.parent.Find(this.name.Replace(" Slider", "")).GetComponent<TMP_InputField>();
        _thisSlider.onValueChanged.AddListener(delegate { ValueChanged(); });
        if (this.name.Contains("Unit"))
        {
            _troopsSlider = this.transform.parent.parent.Find("Troops").Find("Troops Quantity Slider").GetComponent<Slider>();
            _thisSlider.maxValue = CurrentInfo.currentChampion.leadership / GameManager.Instance.UnitMinVal;
            _thisSlider.minValue = 1;
        }
        else if (this.name.Contains("Troops"))
        {
            //_unitSlider = this.transform.parent.parent.Find("Unit").Find("Unit Quantity Slider").GetComponent<Slider>();
            _thisSlider.maxValue = CurrentInfo.currentChampion.leadership;
            _thisSlider.minValue = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ValueChanged()
    {
        if (_text)
        {
            _text.text = _thisSlider.value.ToString();
            ChangeValueInteractively();
        }
    }

    public void ChangeValueInteractively()
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
        if (this.name.Contains("Unit"))
        {
            _troopsSlider.maxValue = CurrentInfo.currentChampion.leadership - _troopsAmount;
        }
        else if (this.name.Contains("Troops"))
        {
            //_unitSlider.maxValue = Convert.ToInt32(
            //    Math.Ceiling(CurrentInfo.currentChampion.leadership - _troopsAmount / (decimal)GameManager.Instance.UnitMinVal)
            //    );
        }
    }
}
