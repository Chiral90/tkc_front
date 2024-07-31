using UnityEngine;
using System.Linq;

public class SampleWorldMapScene : MonoBehaviour
{
    private Camera _uiCamera;
    private PolygonCollider2D _collider;

    private bool _isMouseOver;
    private bool _isMousePushed;
    GameObject _target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // var ray = _uiCamera.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        // Physics.Raycast(ray, out hit);
        // Debug.Log(hit.transform.gameObject.name);
        // if (hit.collider != null && hit.collider == _collider && !_isMouseOver)
        // {
        //     Debug.Log(hit.transform.gameObject.name);
        // }
        
        // if (hit.collider != null && hit.collider == _collider && !_isMouseOver)
        // {
        //     _isMouseOver = true;
        //     OnMouseEnter();
        // }else if (hit.collider != _collider && _isMouseOver)
        // {
        //     _isMouseOver = false;
        //     OnMouseExit();
        // }

        // if (_isMouseOver) OnMouseOver();
        // if (_isMouseOver && Input.GetMouseButtonDown(0))
        // {
        //     _isMousePushed = true;
        //     OnMouseDown();
        // }
        // if (_isMousePushed && Input.GetMouseButton(0)) OnMouseDrag();
        // if (_isMousePushed && Input.GetMouseButtonUp(0))
        // {
        //     _isMousePushed = false;
        //     OnMouseUp();
        //     if(_isMouseOver) OnMouseUpAsButton();
        // }
    }
    private void OnMouseDown()
    {
        // Debug.Log("OnMouseDown");
    }
    private void OnMouseUp()
    {
        // Debug.Log("OnMouseUp");
    }
    // private void OnMouseExit()
    // {
    //     Debug.Log("OnMouseExit");
    // }
    private void OnMouseDrag()
    {
        // Debug.Log("OnMouseDrag");
    }
    private void OnMouseUpAsButton()
    {
        // Debug.Log("OnMouseUpAsButton");
    }
    // private void OnMouseEnter()
    // {
    //     Debug.Log("OnMouseEnter");
    // }
    public void OnMouseExit()
    {
        // Debug.Log("Mouse Exit");
    }
    private void OnMouseOver()
    {
        // Debug.Log("OnMouseOver at Sample Scene");
    }
    public void OnMouseEnter()
    {
        // Debug.Log("Mouse Enter at Sample Scene");
    }
    public void SelectLocation()
    {
        // 전체 맵과 전체 성 리스트, 상세 성 정보 보여주고
        // 선택
    }
    public void InitialScene()
    {
        // 테스트 용
        if (CurrentInfo.currentChampion == null)
        {


        }
        // 빌딩에 EventManager Component추가
        GameObject buildings = GameObject.Find("Map").transform.Find("Buildings").gameObject;
        int buildingCnt = buildings.transform.childCount;
        for (int i = 0; i < buildingCnt; i++)
        {
            buildings.transform.GetChild(i).gameObject.AddComponent<BuildingEventManager>();
        }
        GameObject _userCavnas = GameObject.Find("GameManager").transform.Find("userCanvas").gameObject;
        _userCavnas.transform.Find("userPanel").gameObject.SetActive(true);
        // 현재 위치 없으면
        if (CurrentInfo.currentChampion.location == "")
        {
            // 시작 위치 선택하라는 알람 팝업 띄우고

            // 선택 가능한 건물을 하이라이트 해주고 선택

            // 선택 확인
        }
    }
}
