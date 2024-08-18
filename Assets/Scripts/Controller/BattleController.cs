using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BattleController : MonoBehaviour
{
    BuildingInfo _thisBuilding;
    Dictionary<int, List<string>> _users;
    // Start is called before the first frame update
    void Start()
    {
        // This Building Data
        _thisBuilding = this.GetComponent<BuildingEventManager>().BuildingData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set Joined Champ List
    void SetJoinedChampList()
    {
        Api api;
        var objData = new object();
        api.routePath = "";
        var result = StaticCoroutine.StartStaticCoroutine(api.GetRequest(onSuccess: (result) =>
        {
            Debug.Log(result);
            // do something
        }, onFailure: (error) =>
        {
            Debug.Log(error);
            // do something
        }));
    }
}
