//#define DEV_STATE

using System.Timers;
using System;
using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    int _unitMinVal = 5;
    public int UnitMinVal {  get { return _unitMinVal; } }
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is NULL");
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        // GameObject userCanvasObj = Instantiate(Resources.Load<GameObject>("Prefabs/userCanvas"), GameObject.Find("Main Camera").transform.parent, false);//prefab 적용 시
        GameObject userCanvasObj = this.transform.Find("userCanvas").gameObject;
        userCanvasObj.transform.Find("userPanel").gameObject.SetActive(false);
        Debug.Log("GameManager Start");
        this.gameObject.SetActive(true);
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
