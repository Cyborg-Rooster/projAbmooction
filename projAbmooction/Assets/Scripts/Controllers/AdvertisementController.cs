using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;
class AdvertisementController : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [Header("Interstitial ID")]
    [SerializeField] string AndroidInterstitialAdUnitId;
    [SerializeField] string IosInterstitialAdUnitId;

    [Header("Rewarded ID")]
    [SerializeField] string AndroidRewardedAdUnitId;
    [SerializeField] string IosRewardedAdUnitId;

    //Load
    public static DefaultState RewardAdLoadState = DefaultState.Null;
    public static DefaultState InterstitialAdLoadState = DefaultState.Null;
    //Show
    public static DefaultState RewardAdShowState = DefaultState.Null;
    public static DefaultState InterstitialAdShowState = DefaultState.Null;

    #region "Load Ad"

    public void LoadInterstitial()
    {
        InterstitialAdLoadState = DefaultState.Null;
        string AdUnitIdToLoad = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? IosInterstitialAdUnitId
            : AndroidInterstitialAdUnitId;

        LoadAd(AdUnitIdToLoad);
    }

    public void LoadRewarded()
    {
        RewardAdLoadState = DefaultState.Null;
        string AdUnitIdToLoad = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? IosRewardedAdUnitId
            : AndroidRewardedAdUnitId;

        LoadAd(AdUnitIdToLoad);
    }

    private void LoadAd(string AdUnitIdToLoad)
    {
        Debug.Log("Loading Ad: " + AdUnitIdToLoad);
        Advertisement.Load(AdUnitIdToLoad, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == AndroidInterstitialAdUnitId || placementId == IosInterstitialAdUnitId)
            InterstitialAdLoadState = DefaultState.Yes;
        else
            RewardAdLoadState = DefaultState.Yes;
        Debug.Log(placementId + " loaded successful. ");
        GameData.NetworkState = NetworkStates.Online;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (placementId == AndroidInterstitialAdUnitId || placementId == IosInterstitialAdUnitId)
            InterstitialAdLoadState = DefaultState.No;
        else
            RewardAdLoadState = DefaultState.No;
        Debug.Log($"Error: {error} - {message}");
        GameData.NetworkState = NetworkStates.Offline;
    }
    #endregion

    #region "Show Ad"
    public void ShowInterstitial()
    {
        string AdUnitIdToLoad = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? IosInterstitialAdUnitId
            : AndroidInterstitialAdUnitId;

        ShowAd(AdUnitIdToLoad);
    }

    public void ShowRewarded()
    {
        string AdUnitIdToLoad = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? IosRewardedAdUnitId
            : AndroidRewardedAdUnitId;

        ShowAd(AdUnitIdToLoad);
    }

    private void ShowAd(string AdUnitIdToLoad)
    {
        Debug.Log("Showing Ad: " + AdUnitIdToLoad);
        Advertisement.Show(AdUnitIdToLoad, this);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == AndroidInterstitialAdUnitId || placementId == IosInterstitialAdUnitId) InterstitialAdShowState = DefaultState.Yes;
        else RewardAdShowState = DefaultState.Yes;

        Debug.Log(placementId + " showed successful.");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (placementId == AndroidInterstitialAdUnitId || placementId == IosInterstitialAdUnitId) InterstitialAdShowState = DefaultState.No;
        else RewardAdLoadState = DefaultState.No;

        Debug.Log($"an error occurred while displaying the ad - {error}: {message}");
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowStart(string placementId) { }
    #endregion
}
