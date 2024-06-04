using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    Canvas _flagCanvas;
    // Start is called before the first frame update
    void Start()
    {
        _flagCanvas = this.transform.Find("UnitFlag80x80").Find("Canvas").GetComponent<Canvas>();
        _flagCanvas.renderMode = RenderMode.WorldSpace;
        _flagCanvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
