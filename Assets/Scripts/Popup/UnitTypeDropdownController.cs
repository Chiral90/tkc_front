using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitTypeDropdownController : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Indexes that should be ignored. Indexes are 0 based.")]
    public List<int> indexesToDisable = new List<int>();

    TMP_Dropdown _unitTypeDropdown;

    // Start is called before the first frame update
    void Start()
    {
        _unitTypeDropdown = GetComponent<TMP_Dropdown>();
        _unitTypeDropdown.options.Clear();
        SetOptions();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetOptions()
    {
        _unitTypeDropdown.options.Clear();
        // { "보병", "기병", "창병", "전차병", "궁병" }
        for (int i = 0; i < CurrentInfo.currentChampion.unit_types.Length; i++)
        {
            TMP_Dropdown.OptionData _o = new TMP_Dropdown.OptionData();
            _o.text = CurrentInfo.currentChampion.unit_types[i];

            // 0: 지, 1: 체, 2: 덕, 3: 첩보, 4: 보급
            if (CurrentInfo.currentChampion.champ_type == 0)
            {

            }
            else if (CurrentInfo.currentChampion.champ_type == 3)
            {
                _unitTypeDropdown.options.Add(_o);
                if (_o.text.Equals("기병") || _o.text.Equals("전차병"))
                {
                    EnableOption(i, false);
                }
            }
            else if (CurrentInfo.currentChampion.champ_type == 4)
            {
                _unitTypeDropdown.options.Add(_o);
                if (_o.text.Equals("창병") || _o.text.Equals("기병") || _o.text.Equals("전차병"))
                {
                    EnableOption(i, false);
                }
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var dropDownList = GetComponentInChildren<Canvas>();
        if (!dropDownList)
        {
            Debug.Log("dropDownList is null");
            return;
        }

        // If the dropdown was opened find the options toggles
        var toggles = dropDownList.GetComponentsInChildren<Toggle>(true);

        for (var i = 0; i < toggles.Length; i++)
        {
            toggles[i].interactable = !indexesToDisable.Contains(i - 1);
        }
    }

    // Anytime change a value by index
    public void EnableOption(int index, bool enable)
    {
        if (enable)
        {
            // remove index from disabled list
            if (indexesToDisable.Contains(index))
            {
                indexesToDisable.Remove(index);
            }
        }
        else
        {
            // add index to disabled list
            if (!indexesToDisable.Contains(index))
            {
                indexesToDisable.Add(index);
            }
        }

        var dropDownList = GetComponentInChildren<Canvas>();

        // If this returns null than the Dropdown was closed
        if (!dropDownList) return;

        // If the dropdown was opened find the options toggles
        var toggles = dropDownList.GetComponentsInChildren<Toggle>(true);
        toggles[index].interactable = enable;
    }

    // Anytime change a value by string label
    public void EnableOption(string label, bool enable)
    {
        var index = _unitTypeDropdown.options.FindIndex(o => string.Equals(o.text, label));

        // We need a 1-based index
        EnableOption(index + 1, enable);
    }
}