using UnityEngine;

public class TurnTimer : MonoBehaviour
{
    float _pointTime = 1.0f;
    float _nextTime = 0.0f;
    
    #region Singleton
    private static TurnTimer _instance;
    public static TurnTimer Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("TurnTimer is NULL");
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (Time.time > _nextTime)
        {
            _nextTime = Time.time + _pointTime;
        }
    }
}
