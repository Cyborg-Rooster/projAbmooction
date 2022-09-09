using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;

class ApiManager
{
    public static IEnumerator GetCurrentTime(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            Debug.Log(GameData.NetworkState);
            GameData.NetworkState = NetworkStates.Null;
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    WorldTime api = JsonUtility.FromJson<WorldTime>(webRequest.downloadHandler.text);

                    GameData.DateTimeNow = DateTime.Parse(api.dateTime);
                    GameData.NetworkState = NetworkStates.Online;
                    break;
                default:
                    Debug.LogError("API get an error: " + webRequest.error);
                    GameData.NetworkState = NetworkStates.Offline;
                    break;
            }
        }
    }
}
