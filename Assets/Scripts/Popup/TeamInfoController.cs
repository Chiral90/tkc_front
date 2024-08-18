#define DEV_STATE

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TeamInfoController : MonoBehaviour
{
    TMP_Dropdown _teamTypeDropdown;
    GameObject _teamList;
    GameObject _teamData;
    bool _isPositioning;
    // Start is called before the first frame update
    void Start()
    {
        _teamTypeDropdown = this.transform.Find("Team Type").GetComponent<TMP_Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPositioning)
        {

        }
    }

    void SetTeamList()
    {
        GameObject _c = this.transform.Find("Team List")
            .Find("Scroll View").Find("Viewport").Find("Content").gameObject;
        GameObject _item = _c.transform.Find("Label").gameObject;
#if DEV_STATE
        for (int i = 0; i < 3; i++)
        {
            GameObject _i = Instantiate(_item, _c.transform, false);
            _i.name = "team_" + i;
            _i.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = i.ToString();
            _i.transform.Find("Type").GetComponent<TextMeshProUGUI>().text = i.ToString();
            _i.transform.Find("Leader").GetComponent<TextMeshProUGUI>().text = i == 0 ? "ÀÚÀ¯" : i == 1 ? "chiral" : "enantiomer";
            _i.transform.Find("Castle").GetComponent<TextMeshProUGUI>().text = 2.ToString();
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
    public void Apply()
    {
#if DEV_STATE
        if (CurrentInfo.currentChampion is null) return;
        CurrentInfo.currentChampion.team = _teamTypeDropdown.value;
#else
        var objData = new object();
        api.routePath = "/team";
        var result = StaticCoroutine.StartStaticCoroutine(api.PostRequest(onSuccess: (result) =>
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
    public void Cancel()
    {

    }
    void OnEnable()
    {
        SetTeamList();
    }
}
