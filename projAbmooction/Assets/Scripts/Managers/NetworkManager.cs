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

        if (GameData.NetworkState == NetworkStates.Online)
        {
            FirebaseManager.Init();
            FirebaseManager.LoadBox();

            yield return new WaitUntil(() => FirebaseManager.BoxLoaded);

            initializer.Initialize();

            yield return new WaitUntil(() => initializer.ReturnIfAdsInitializes());
        }
        else FirebaseManager.BoxLoaded = true;
    }
}
