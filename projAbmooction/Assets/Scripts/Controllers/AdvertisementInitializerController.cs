using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Advertisements;
using UnityEngine;

class AdvertisementInitializerController : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string AndroidGameId;
    [SerializeField] string IOSGameId;
    [SerializeField] bool TestMode = true;
    private string GameId;

    public void Initialize()
    {
        GameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? IOSGameId
            : AndroidGameId;
        Advertisement.Initialize(GameId, TestMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public bool ReturnIfAdsInitializes()
    {
        return Advertisement.isInitialized;
    }
}
