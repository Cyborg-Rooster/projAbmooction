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

        if (GameData.NetworkState == NetworkStates.Online)
        {
            initializer.Initialize();

            yield return new WaitUntil(() => initializer.ReturnIfAdsInitializes());
        }
    }
}
