using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using TMPro;
using System.Threading.Tasks;
using System;

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

        InitialSetting();
    }
    void InitialSetting()
    {
        if (CurrentInfo.currentChampion.team == 0)
        {
            LoadMessageBox("시작 세력을 선택하세요.");
            SelectStartTeam();
        }
        if (CurrentInfo.currentChampion.location == "")
        {
            LoadMessageBox("시작 위치를 선택하세요.");
            SelectStartLocation();
        }
    }
    void SelectStartLocation()
    {
        Debug.Log("Select Start Location");
        // 선택 방식?
        // 선택 가능한 건물을 하이라이트 해주고 선택
        if (GameObject.Find("popupCanvas").transform.Find("MessageBox").gameObject != null)
        {
            //UIManager.Instance.ReservePopup(popup.GetComponentInChildren<UIPopup>());
        }
    }
    void SelectStartTeam()
    {
        Debug.Log("Select Start Team");
        Canvas _popupCanvas = GameObject.Find("popupCanvas").GetComponent<Canvas>();
        GameObject _p = Resources.Load<GameObject>("Prefabs/Team Info Window");
        GameObject popup = Instantiate(_p, _popupCanvas.transform, false);//prefab 적용 시
        popup.name = "Team Info Window";

        if (GameObject.Find("popupCanvas").transform.Find("MessageBox").gameObject != null)
        {
            UIManager.Instance.ReservePopup(popup.GetComponentInChildren<UIPopup>());
        }
    }
    void LoadMessageBox(string msg)
    {
        Debug.Log("Load Message Box");
        Canvas _popupCanvas = GameObject.Find("popupCanvas").gameObject.GetComponent<Canvas>();
        GameObject _mb = Resources.Load<GameObject>("Prefabs/MessageBox");
        _mb.transform.Find("Content").GetComponent<TextMeshProUGUI>().text = msg;
        GameObject popup = Instantiate(_mb, _popupCanvas.transform, false);//prefab 적용 시
        popup.name = "MessageBox";
        if (GameObject.Find("popupCanvas").transform.Find("MessageBox").gameObject != null)
        {
            popup.transform.position =
                new Vector3(GameObject.Find("popupCanvas").transform.Find("MessageBox").gameObject.transform.position.x - 50
                , GameObject.Find("popupCanvas").transform.Find("MessageBox").gameObject.transform.position.y - 50
                , GameObject.Find("popupCanvas").transform.Find("MessageBox").gameObject.transform.position.z);
            UIManager.Instance.OpenMessageBox(popup.GetComponent<UIPopup>());
        }
    }
}
