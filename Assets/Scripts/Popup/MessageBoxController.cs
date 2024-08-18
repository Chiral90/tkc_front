using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBoxController : MonoBehaviour
{
    TextMeshProUGUI _msg;
    UIPopup _popup;
    // Start is called before the first frame update
    void Start()
    {
        _msg = this.transform.Find("Content").GetComponent<TextMeshProUGUI>();
        _popup = this.GetComponent<UIPopup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMessage(string msg)
    {
        _msg.text = msg;
    }

    public void SelectOK()
    {
        _popup.Close();
    }

    public void SelectCancel()
    {
        _popup.Close();
    }
}
