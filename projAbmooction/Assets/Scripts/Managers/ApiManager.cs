using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using UnityEngine.Networking;
using UnityEngine;

class ApiManager
{
    public static IEnumerator GetCurrentTime(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            GameData.NetworkState = NetworkStates.Null;
            Debug.Log("Connecting");
            //webRequest.timeout = 1;
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.Success:
                    WorldTime api = JsonUtility.FromJson<WorldTime>(webRequest.downloadHandler.text);

                    GameData.DateTimeNow = DateTime.Parse(api.dateTime);
                    GameData.NetworkState = NetworkStates.Online;
                    Debug.Log("API get successful.");
                    break;
                default:
                    Debug.LogError("API get an error: " + webRequest.error);
                    GameData.NetworkState = NetworkStates.Offline;
                    break;
            }
        }
    }

    public static async void GetAPI(string uri)
    {
        GameData.NetworkState = NetworkStates.Null;
        Debug.Log("Connecting");

        try
        {
            WebRequest request = WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            WorldTime api = JsonUtility.FromJson<WorldTime>(responseFromServer);

            GameData.DateTimeNow = DateTime.Parse(api.dateTime);
            GameData.NetworkState = NetworkStates.Online;
            Debug.Log("API get successful.");

            reader.Close();
            dataStream.Close();
            response.Close();
        }
        catch (Exception e)
        {
            Debug.LogError($"API get an error: {e}");
            GameData.NetworkState = NetworkStates.Offline;
        }
    }
}
