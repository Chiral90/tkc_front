using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    //0: start, 1: world map, 2: battle map
    int _sceneType;
    Object _scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSceneType()
    {
        string _s = SceneManager.GetActiveScene().name;
        if (_s.Contains("Battle"))
        {
            Debug.Log("Battle Scene");
            _scene = this.GetComponent<SampleBattleScene>();
        }
        else if (_s.Contains("Start"))
        {
            Debug.Log("Start Scene");
        }
        else if (_s.Contains("World"))
        {
            Debug.Log("World Scene");
            _scene = this.GetComponent<SampleWorldMapScene>();
        }
    }
    public void LoadWorldMap()
    {
        SceneManager.LoadScene("SampleWorldMapScene");
    }
    public void LoadBattleMap()
    {
        SceneManager.LoadScene("SampleBattleScene");
    }
    public void LoadSupplyMap()
    {
        SceneManager.LoadScene("SampleSupplyScene");
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        UIManager.Instance.CloseAllOpenPopups();
        // If scene is your game scene, change panel
        if (scene.Equals(SceneManager.GetSceneByName("SampleWorldMapScene")))
        {
            this.GetComponent<SampleWorldMapScene>().InitialScene();
        }
        else if (scene.Equals(SceneManager.GetSceneByName("SampleBattleScene")))
        {
            this.GetComponent<SampleBattleScene>().InitialScene();
        }
    }
    void OnActiveSceneChanged(UnityEngine.SceneManagement.Scene before, UnityEngine.SceneManagement.Scene after)
    {
        if (before.name.Contains("Battle"))
        {
            if (!(new TurnInfoManager().GetRemain().Equals("00:00")))
            {
                this.GetComponent<SampleBattleScene>().ReserveBattleUnitAction();
            }
        }
    }
}
