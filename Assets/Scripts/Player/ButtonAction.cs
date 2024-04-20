using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void createChampion()
    {
        GameObject.Find("Canvas").transform.Find("Champion Data").gameObject.SetActive(true);
    }

    public void confirmCreateChampion()
    {
        //string _id = "dozos";//get datas from cookies
        var _idObj = GameObject.Find("CurrentUser").transform.Find("CurrentID").GetComponent<TMP_Text>();
        string _id = _idObj.text;

        var _nameObj = GameObject.Find("ChampionName").GetComponent<TMP_InputField>();
        var _typeObj = GameObject.Find("ChampionType").GetComponent<TMP_Dropdown>();
        string _name = _nameObj.text;
        int _type = _typeObj.value;
        //validate name, type
        ChampionInfo _champ = new ChampionInfo();
        _champ.createNewChampion(_name, _type);
        string _champStr = JsonUtility.ToJson(_champ);
        Debug.Log(_champStr);
        WWWForm _champWWW = _champ.createWWWForm();
        Debug.Log(_champWWW);
        var result = FindObjectOfType<WebRequest>().PostRequest(CurrentInfo.serverURI + "/champ/create", _champWWW);
        while (result.MoveNext())
        {
            Debug.Log(result.Current.ToString());
        }
        //string _data = "createChampion=" + _id + ":" + _champStr;
        //FindObjectOfType<Node>().ws_SendMessage(_data);
    }

    public void cancelCreateChampion()
    {
        GameObject.Find("Canvas").transform.Find("Champion Data").gameObject.SetActive(false);
    }
}
