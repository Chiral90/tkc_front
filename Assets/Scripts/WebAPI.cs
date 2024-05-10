using UnityEngine;
using System.Collections;

using System;
using UnityEngine.Networking;

public struct ApiError
{
    public long statusCode;
    public string message;

    public ApiError(long _statusCode, string _message)
    {
        statusCode = _statusCode;
        message = _message;
    }
}

public struct Api
{
    public string routePath;
    public IEnumerator GetRequest(Action<string> onSuccess, Action<ApiError> onFailure)
    {
        string url = CurrentInfo.serverURI + routePath;
        yield return Get(url, (request) =>
        {
            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {
                string jsonString = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                onSuccess(jsonString);
            }
            else
            {
                onFailure(new ApiError(request.responseCode, request.error));
            }
        });
    }
    public IEnumerator PostRequest(WWWForm data, Action<string> onSuccess, Action<ApiError> onFailure)
    {
        string url = CurrentInfo.serverURI + routePath;
        yield return Post(url, data, (request) =>
        {
            if (request.result == UnityWebRequest.Result.Success && request.responseCode == 200)
            {
                string jsonString = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                onSuccess(jsonString);
            }
            else
            {
                onFailure(new ApiError(request.responseCode, request.error));
            }
        });
    }

    IEnumerator Get(string url, Action<UnityWebRequest> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Cookie", string.Format("id={0}", CurrentInfo.currentID));
        yield return request.SendWebRequest();
        callback(request);
    }
    IEnumerator Post(string url, WWWForm obj, Action<UnityWebRequest> callback)
    {
        UnityWebRequest request = UnityWebRequest.Post(url, obj);
        request.SetRequestHeader("Cookie", string.Format("id={0}", CurrentInfo.currentID));
        yield return request.SendWebRequest();
        callback(request);
    }
}
// Usage Script
// StartCoroutine(api.GetRequest(onSuccess: (result) =>
// {
//     // do something
// }, onFailure: (error) =>
// {
//     // do something
// }));