using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    Debug.LogError("UIManager가 씬에 존재하지 않습니다.");
                }
            }
            return _instance;
        }
    }

    private Stack<UIPopup> openPopups = new Stack<UIPopup>();
    private Queue<UIPopup> pendingPopups = new Queue<UIPopup>(); // 예약된 팝업을 위한 큐

    private void Update()
    {
        // 뒤로가기 키를 누르면 가장 최근에 열린 팝업을 닫습니다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseLastOpenedPopup();
        }
    }

    // 팝업을 엽니다.
    public void OpenBuildingPopup(UIPopup popup, BuildingInfo data = null)
    {
        if (popup != null)
        {
            if (!data.IsUnityNull())
            {
                popup.bData = data;
                Debug.Log(string.Format("current champ_name : {0} / castellan : {1} / equal? : {2}", CurrentInfo.currentChampion.champ_name, data.castellan, data.castellan.Equals(CurrentInfo.currentChampion.champ_name)));
                Debug.Log(string.Format("current team : {0} / building team : {1} / equal? : {2}", CurrentInfo.currentChampion.team, data.team, CurrentInfo.currentChampion.team == data.team));
                // set building datas
                var components = popup.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (var c in components)
                {
                    if (c.gameObject.name == "Building Name") { c.text = data.building_name; }
                    if (c.gameObject.name == "Building Population") { c.text = data.population.ToString(); }
                    if (c.gameObject.name == "Building Castellan") { c.text = data.castellan; }
                }
                // Debug.Log(string.Join(',', data.stationed));
                GameObject _button = popup.gameObject.transform.Find("Enter Building").gameObject;

                if (data.status == 1)
                {
                    _button.GetComponentInChildren<TextMeshProUGUI>().text = "전투";
                }
                else if (CurrentInfo.currentChampion.team == data.team)
                {
                    foreach (ChampionInfo c in data.stationed)
                    {
                        if (c.champ_name.Equals(CurrentInfo.currentChampion.champ_name))
                        {
                            Debug.Log("stationed...");
                            _button.SetActive(false);
                            break;
                        }
                        else
                        {
                            Debug.Log("not stationed...");
                            _button.GetComponentInChildren<TextMeshProUGUI>().text = "진입";
                            _button.SetActive(true);
                        }
                    }
                    popup.gameObject.transform.Find("Building Panel Options").gameObject.SetActive(true);
                }
                else if (CurrentInfo.currentChampion.location.Equals(""))
                {
                    Debug.Log("no location...");
                    _button.GetComponentInChildren<TextMeshProUGUI>().text = "선택";
                    _button.SetActive(true);
                }
                else
                {
                    if (data.team == 0)
                    {
                        _button.GetComponentInChildren<TextMeshProUGUI>().text = "점령";
                    }
                    else
                    {
                        if (CurrentInfo.currentChampion.champ_type == 3)
                        {
                            _button.GetComponentInChildren<TextMeshProUGUI>().text = "잠입";
                        }
                        else
                        {
                            _button.GetComponentInChildren<TextMeshProUGUI>().text = "침입";
                        }
                    }
                    _button.SetActive(true);
                }
            }
            else
            {
                Debug.Log("data is null");
            }
            // 새로운 팝업을 엽니다.
            UIUtilities.SetUIActive(popup.gameObject, true);
            openPopups.Push(popup);
        }
    }
    public void OpenUnitFormationPopup(UIPopup popup)
    {
        // 새로운 팝업을 엽니다.
        UIUtilities.SetUIActive(popup.gameObject, true);
        openPopups.Push(popup);
    }

    // 팝업을 닫습니다.
    public void ClosePopup(UIPopup popup)
    {
        if (popup != null && openPopups.Contains(popup))
        {
            // 팝업을 닫습니다.
            UIUtilities.SetUIActive(popup.gameObject, false);
            // 팝업 제거
            Destroy(openPopups.Pop().gameObject);

            // 팝업이 닫힌 후 예약된 팝업이 있다면 엽니다.
            if (pendingPopups.Count > 0)
            {
                OpenBuildingPopup(pendingPopups.Dequeue());
            }
        }
    }

    // 가장 최근에 열린 팝업을 닫습니다.
    public void CloseLastOpenedPopup()
    {
        if (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Peek());
        }
    }

    // 모든 열린 팝업을 닫습니다.
    public void CloseAllOpenPopups()
    {
        while (openPopups.Count > 0)
        {
            ClosePopup(openPopups.Peek());
        }
    }

    // 예약된 팝업을 추가합니다.
    public void ReservePopup(UIPopup popup)
    {
        if (popup != null)
        {
            pendingPopups.Enqueue(popup);
        }
    }

    public void HidePopup(UIPopup popup)
    {
        // 팝업을 닫습니다.
        UIUtilities.SetUIActive(popup.gameObject, false);
    }
    public void ShowPopup(UIPopup popup)
    {
        // 팝업을 닫습니다.
        UIUtilities.SetUIActive(popup.gameObject, true);
    }
}
