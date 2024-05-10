using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class SampleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 테스트 용
        if (CurrentInfo.currentChampion == null)
        {
            CurrentInfo.currentID = "dozos";
            CurrentInfo.currentChampion = new ChampionInfo();
            CurrentInfo.currentChampion.createNewChampion("chiral", 0, 1);
            CurrentInfo.currentChampion.own_castles.Add("castle1");
            CurrentInfo.currentChampion.location = "castle1";
        }
        // 현재 위치 없으면
        if (CurrentInfo.currentChampion.location == "")
        {
            // 시작 위치 선택하라는 알람 팝업 띄우고

            // 선택 가능한 건물을 하이라이트 해주고 선택

            // 선택 확인
        }
        // 빌딩에 EventManager Component추가
        GameObject buildings = GameObject.Find("Map").transform.Find("Buildings").gameObject;
        int buildingCnt = buildings.transform.childCount;
        for (int i = 0; i < buildingCnt; i++)
        {
            buildings.transform.GetChild(i).gameObject.AddComponent<BuildingEventManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void selectLocation()
    {
        // 전체 맵과 전체 성 리스트, 상세 성 정보 보여주고
        // 선택
    }
}
