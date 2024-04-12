using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    public void OpenPopup(UIPopup popup, object? data = null)
    {
        if (popup != null)
        {
            if (!data.IsUnityNull())
            {
                Debug.Log(popup.gameObject.name);
                var components = popup.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                Debug.Log(components.Count());
                foreach (var c in components)
                {
                    Debug.Log(c.gameObject.name);
                    if (c.gameObject.name == "Building Name") { c.text = ((BuildingInfo)data).buildingName; }
                    if (c.gameObject.name == "Building Population") { c.text = ((BuildingInfo)data).population.ToString(); }
                    if (c.gameObject.name == "Building Castellan") { c.text = ((BuildingInfo)data).castellanName; }
                }
            }
            // 새로운 팝업을 엽니다.
            UIUtilities.SetUIActive(popup.gameObject, true);
            openPopups.Push(popup);
        }
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
                OpenPopup(pendingPopups.Dequeue());
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
}
