using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCoroutine : MonoBehaviour
{
    private static StaticCoroutine instance = null;

#if UNITY_EDITOR
    [Serializable]
    private class CoroutineRecord
    {
        public string name;
    }

    [SerializeField] private List<CoroutineRecord> coroutineRecords = new List<CoroutineRecord>();
#endif

    /// <summary>
    /// Initialize StaticCoroutine after the scene has loaded.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnLoad()
    {
        if (instance == null)
        {
            GameObject go = new GameObject("StaticCoroutine");
            instance = go.AddComponent<StaticCoroutine>();
            DontDestroyOnLoad(go);
        }
    }

    /// <summary>
    /// Start static coroutine.
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public static Coroutine StartStaticCoroutine(IEnumerator enumerator)
    {
        return instance.StartProcess(enumerator);
    }

    /// <summary>
    /// Start enumerator.
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    private Coroutine StartProcess(IEnumerator enumerator)
    {
        return StartCoroutine(enumerator);
    }
}