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
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    WorldTime api = JsonUtility.FromJson<WorldTime>(webRequest.downloadHandler.text);

                    GameData.DateTimeNow = DateTime.Parse(api.dateTime);
                    Debug.Log(GameData.DateTimeNow.ToString());

                    break;
                case UnityWebRequest.Result.InProgress :
                default:
                    Debug.LogError("API get an error: " + webRequest.error);
                    GameData.IsOnline = false;
                    break;
            }
        }
    }
}
