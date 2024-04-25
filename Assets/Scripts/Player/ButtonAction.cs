using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour
{
    Api api;
    public void startGame()
    {
        Debug.Log("Start Game : " + CurrentInfo.currentChampion.champ_name);
        SceneManager.LoadScene("SampleScene");
    }

    public void createChampion()
    {
        GameObject.Find("Canvas").transform.Find("Champion Data").gameObject.SetActive(true);
    }

    public void confirmCreateChampion()
    {
        //string _id = "dozos";//get datas from cookies
        string _id = CurrentInfo.currentID;

        var _nameObj = GameObject.Find("ChampionName").GetComponent<TMP_InputField>();
        var _typeObj = GameObject.Find("ChampionType").GetComponent<TMP_Dropdown>();
        var _teamObj = GameObject.Find("ChampionType").GetComponent<TMP_Dropdown>();
        string _name = _nameObj.text;
        int _type = _typeObj.value;
        int _team = _teamObj.value;
        //validate name, type
        ChampionInfo _champ = new ChampionInfo();
        _champ.createNewChampion(_name, _type, _team);
        string _champStr = JsonUtility.ToJson(_champ);
        Debug.Log(_champStr);
        WWWForm _champWWW = _champ.createWWWForm();
        StartCoroutine(api.PostRequest(_champWWW, onSuccess: (result) =>
        {
            Debug.Log(result);
        }, onFailure: (error) =>
        {
            Debug.Log(error);
        }));
    }

    public void cancelCreateChampion()
    {
        GameObject.Find("Canvas").transform.Find("Champion Data").gameObject.SetActive(false);
    }

    public void openChamplist()
    {
        GameObject _button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject _subPanel = _button.transform.parent.gameObject.transform.parent.Find("Building Panel Sub").gameObject;
        Debug.Log(string.Format("subPanel: {0}", _subPanel.activeSelf));
        if (!_subPanel.activeSelf)
        {
            _subPanel.SetActive(true);
        }
        _subPanel.transform.Find("Building Detail Panel").gameObject.SetActive(false);
        _subPanel.transform.Find("Building Champ List Panel").gameObject.SetActive(true);
    }
    public void openDetail()
    {
        GameObject _button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        GameObject _subPanel = _button.transform.parent.gameObject.transform.parent.Find("Building Panel Sub").gameObject;
        Debug.Log(string.Format("subPanel: {0}", _subPanel.activeSelf));
        if (!_subPanel.activeSelf)
        {
            _subPanel.SetActive(true);
        }
        _subPanel.transform.Find("Building Champ List Panel").gameObject.SetActive(false);
        _subPanel.transform.Find("Building Detail Panel").gameObject.SetActive(true);
    }
}
