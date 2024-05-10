using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

// 예시
// 새롭게 추가된 데이터 클래스
public class CustomData
{
    bool objectOnTile;
    int objectCount;
    List<string> destinations;
    // ... 
}
// 타일의 정보를 관리하는 클래스
public class TileManager : MonoBehaviour
{
    public Dictionary<Vector3Int, CustomData> dataOnTiles; // 타일의 데이터
    public Tilemap tilemap; // 타일맵 컴포넌트
    public Grid buildings;
    
    private void Start()
    {
        dataOnTiles = new Dictionary<Vector3Int, CustomData>();
        // 한 칸 이내 위치
        Vector3Int pos1 = new Vector3Int(1, 0, 0);
        Vector3Int pos2 = new Vector3Int(-1, 0, 0);
        Vector3Int pos3 = new Vector3Int(0, 1, 0);
        Vector3Int pos4 = new Vector3Int(0, -1, 0);
        Vector3Int[] near = {pos1, pos2, pos3, pos4};
        // 타일맵 내의 셀 좌표들에 대해서 타일이 있다면 딕셔너리에 초기 정보를 추가한다.
        foreach(Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            int count = 0;
            // 해당 좌표에 타일이 없으면 넘어간다.
            if(!tilemap.HasTile(pos)) continue;
            if (tilemap.tag != "Road") continue;
            // 해당 좌표의 타일을 얻는다.
            var tile = tilemap.GetTile<TileBase>(pos);
            // 1칸 이내 근접한 타일 확인
            foreach (Vector3Int p in near)
            {
                Vector3Int tmp = pos + p;
                var tmpTile = tilemap.GetTile<TileBase>(tmp);
                // 길이면
                if (tmpTile != null)
                {
                    count += 1;
                }
            }
            // 갈림길
            if (count == 3)
            {
                // 정보 초기화
                dataOnTiles[pos] = new CustomData();
                // Debug.Log("crossroads");
                // Debug.Log(pos);
            }
            // 최종 카운트가 1이면 막다른 길
            if (count == 1)
            {
                dataOnTiles[pos] = new CustomData();
                // Debug.Log("dead end");
                // Debug.Log(pos);
                foreach (Vector3Int p in near)
                {
                    Vector3Int tmp = pos + p;
                    Tilemap _b = buildings.transform.Find("castle1").gameObject.GetComponent<Tilemap>();
                    Debug.Log(string.Format("{0}, {1}, {2}", _b.name, _b.cellBounds.position, pos));
                    
                    var tmpTile = _b.GetTile<TileBase>(_b.LocalToCell(tmp));
                    if (tmpTile == null) continue;
                    Debug.Log(tmpTile.name);
                    if (_b.HasTile(tmp))
                    {
                        Debug.Log("castle1");
                    }
                }
                break;
            }
        }
    }
}
