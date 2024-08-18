using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPopup : MonoBehaviour, IDragHandler, IScrollHandler
{
    BuildingInfo _bData;
    Action<WaitForUserResponse> callback;

    public BuildingInfo BData { get { return _bData; } set { _bData = value; } }
    [SerializeField] private GameObject popupCanvas; // 팝업 창의 캔버스
    [SerializeField] private Animator popupAnimator; // 팝업 창의 애니메이터

    // 애니메이션 이벤트에서 호출할 메서드
    public void OnCloseAnimationFinished()
    {
        // 애니메이션 완료 후 팝업 캔버스를 비활성화합니다.
        UIUtilities.SetUIActive(popupCanvas, false);
    }
    public void Start()
    {
        //UIUtilities.SetUIActive(popupCanvas, false);
    }
    // 팝업이 열릴 때 호출됩니다.
    public void Open()
    {
        if (popupCanvas != null)
        {
            // 팝업 캔버스를 활성화합니다.
            UIUtilities.SetUIActive(popupCanvas, true);

            // 팝업 애니메이션 재생 (애니메이션 이벤트로 OnCloseAnimationFinished 호출)
            if (popupAnimator != null)
            {
                popupAnimator.SetTrigger("Open");
            }
        }
    }

    // 팝업이 닫힐 때 호출됩니다.
    public void Close()
    {
        if (popupCanvas != null)
        {
            // Debug.Log("UIPopup Close Method...");
            // 팝업 애니메이션 재생 (애니메이션 이벤트로 OnCloseAnimationFinished 호출)
            if (popupAnimator != null)
            {
                popupAnimator.SetTrigger("Close");
            }
            else
            {
                // 애니메이션을 사용하지 않는 경우 즉시 팝업 캔버스를 비활성화합니다.
                UIUtilities.SetUIActive(popupCanvas, false);
            }
            Destroy(this.gameObject);
        }
    }

    // 팝업 내에서 어떤 동작을 수행할 때 호출될 메서드들을 추가할 수 있습니다.
    public void OnButtonClicked()
    {
        // 팝업 내 버튼이 클릭되었을 때 실행할 동작을 여기에 작성합니다.
        // Debug.Log("Inside popup...");
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("OnDrag");
        // Debug.Log(string.Format("transform: {0}, eventData: {1}", transform.position, eventData.position));
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
  
        if (Physics.Raycast(ray, out hit))
        {
            // Debug.Log(hit.transform.name);
        }
        transform.position = eventData.position;
    }

    public void OnScroll(PointerEventData eventData)
    {
        // throw new NotImplementedException();
        // Debug.Log("On Scroll");
    }

    public void SelectOKButton()
    {
        WaitForUserResponse response;
        response = new WaitForUserResponse(true);
        callback(response);
    }

    public void SelectCancelButton()
    {

    }
}

public static class UIUtilities
{
    // 게임 오브젝트의 활성화 상태를 설정하는 유틸리티 함수
    public static void SetUIActive(GameObject uiObject, bool isActive)
    {
        if (uiObject != null)
        {
            uiObject.SetActive(isActive);
        }
    }
}

public class WaitForUserResponse : ProcessResponse
{
    public bool userResponse;

    public WaitForUserResponse(bool response)
    {
        userResponse = response;
    }

    public bool response
    {
        get
        {
            return userResponse;
        }
        set
        {
            userResponse = value;
        }
    }
}

public interface ProcessResponse
{
    bool response
    {
        get;
        set;
    }
}