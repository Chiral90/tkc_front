using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuantityTextController : MonoBehaviour
{
    Slider _slider;
    TMP_InputField _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_InputField>();
        _slider = this.transform.parent.Find(this.name + " Slider").GetComponent<Slider>();
        _text.onValueChanged.AddListener(textChanged);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // We need this signature in order to directly add and remove this method as 
    // a listener for the onValueChanged event
    public void textChanged(string newText)
    {
        // temporarily remove your callback
        _text.onValueChanged.RemoveListener(textChanged);

        // change the text without being notified about it
        _slider.value = Convert.ToInt32(_text.text);

        // again attach the callback
        _text.onValueChanged.AddListener(textChanged);
    }
}
