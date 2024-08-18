#define DEV_STATE

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TeamTypeDropdownController : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Indexes that should be ignored. Indexes are 0 based.")]
    public List<int> indexesToDisable = new List<int>();

    TMP_Dropdown _teamTypeDropdown;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Team Type Dropdown Start...");
        _teamTypeDropdown = GetComponent<TMP_Dropdown>();
        _teamTypeDropdown.options.Clear();
        SetOptions();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetOptions()
    {
        _teamTypeDropdown.options.Clear();
#if DEV_STATE
        for (int i = 0; i < 3; i++)
        {
            TMP_Dropdown.OptionData _o = new TMP_Dropdown.OptionData();
            _o.text = i.ToString();
            
            _teamTypeDropdown.options.Add(_o);
        }
#else
        var objData = new object();
        api.routePath = "/team";
        var result = StaticCoroutine.StartStaticCoroutine(api.GetRequest(onSuccess: (result) =>
        {
            Debug.Log(result);
            // do something
        }, onFailure: (error) =>
        {
            Debug.Log(error);
            // do something
        }));
#endif
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
        var index = _teamTypeDropdown.options.FindIndex(o => string.Equals(o.text, label));

        // We need a 1-based index
        EnableOption(index + 1, enable);
    }
}