using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;

class NetworkManager
{
    public static IEnumerator ConnectAndLoad(AdvertisementInitializerController initializer)
    {
        yield return ApiManager.GetCurrentTime("https://timeapi.io/api/Time/current/zone?timeZone=America/Sao_Paulo");

        FirebaseManager.Init();
        FirebaseManager.LoadBox();
        FacebookManager.Init();

        //yield return new WaitUntil(() => FirebaseManager.BoxLoaded);
        yield return new WaitUntil(() => FacebookManager.IsInitialized);
        yield return new WaitUntil(() => GameData.NetworkState != NetworkStates.Null);
        if (GameData.NetworkState == NetworkStates.Online)
        {
            initializer.Initialize();

            yield return new WaitUntil(() => initializer.ReturnIfAdsInitializes());
        }
    }
}
