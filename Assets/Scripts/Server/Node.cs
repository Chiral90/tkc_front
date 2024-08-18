using System;
using System.Collections;
using System.Collections.Generic;
//using System.Net.WebSockets;
using UnityEditor.VersionControl;
using UnityEngine;
using WebSocketSharp;

public class Node
{
    WebSocketSharp.WebSocket ws;
    string result;
    public Node() {
        ws = new WebSocketSharp.WebSocket("ws://192.168.0.2:8182");// 127.0.0.1은 본인의 아이피 주소이다. 3333포트로 연결한다는 의미이다.
        ws.OnMessage += ws_OnMessage; //서버에서 유니티 쪽으로 메세지가 올 경우 실행할 함수를 등록한다.
        ws.OnOpen += ws_OnOpen;//서버가 연결된 경우 실행할 함수를 등록한다
        ws.OnClose += ws_OnClose;//서버가 닫힌 경우 실행할 함수를 등록한다.
        ws.Connect();//서버에 연결한다.
        ws.Send("hello");//서버에게 "hello"라는 메세지를 보낸다.
    }
    
    void ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);//받은 메세지를 디버그 콘솔에 출력한다.
        string data = e.Data.ToString();
    }
    void ws_OnOpen(object sender, System.EventArgs e)
    {
        Debug.Log("socket is opened"); //디버그 콘솔에 "open"이라고 찍는다.
    }
    void ws_OnClose(object sender, CloseEventArgs e)
    {
        Debug.Log("socket is closed"); //디버그 콘솔에 "close"이라고 찍는다.
    }
    void Call(object sender, MessageEventArgs e)
    {
        Debug.Log("주소 :  "+((WebSocket)sender).Url+", 데이터 : "+e.Data);
    }
    public void ws_SendGetData(string method)
    {
        string _method = method;
        ws.Send(_method);
    }
    public string ws_GetData(object sender, MessageEventArgs e)
    {
        string _data = e.Data.ToString();
        return _data;
    }

    public void ws_SendMessage(string data)
    {
        Debug.Log(data);
        ws.Send(data);
    }
}