using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class NetworkManager
{
    public static IEnumerator ConnectAndLoad(AdvertisementInitializerController initializer, AdvertisementController adController)
    {
        yield return ApiManager.GetCurrentTime("https://timeapi.io/api/Time/current/zone?timeZone=America/Sao_Paulo");

        Debug.Log(GameData.networkState);
        if (GameData.networkState == NetworkStates.Online)
        {
            FirebaseManager.Init();
            FirebaseManager.LoadBox();

            yield return new WaitUntil(() => FirebaseManager.BoxLoaded);

            initializer.Initialize();

            adController.LoadRewarded();
            adController.LoadInterstitial();
        }
        else FirebaseManager.BoxLoaded = true;
    }
}
