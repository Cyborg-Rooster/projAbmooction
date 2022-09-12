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
    public static AdState RewardAdLoadState = AdState.Null;
    public static AdState InterstitialAdLoadState = AdState.Null;
    //Show
    public static AdState RewardAdShowState = AdState.Null;
    public static AdState InterstitialAdShowState = AdState.Null;

    #region "Load Ad"

    public void LoadInterstitial()
    {
        InterstitialAdLoadState = AdState.Null;
        string AdUnitIdToLoad = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? IosInterstitialAdUnitId
            : AndroidInterstitialAdUnitId;

        LoadAd(AdUnitIdToLoad);
    }

    public void LoadRewarded()
    {
        RewardAdLoadState = AdState.Null;
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
            InterstitialAdLoadState = AdState.Yes;
        else
            RewardAdLoadState = AdState.Yes;
        Debug.Log(placementId + " loaded successful. ");
        GameData.NetworkState = NetworkStates.Online;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (placementId == AndroidInterstitialAdUnitId || placementId == IosInterstitialAdUnitId)
            InterstitialAdLoadState = AdState.No;
        else
            RewardAdLoadState = AdState.No;
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
        if (placementId == AndroidInterstitialAdUnitId || placementId == IosInterstitialAdUnitId) InterstitialAdShowState = AdState.Yes;
        else RewardAdShowState = AdState.Yes;

        Debug.Log(placementId + " showed successful.");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        if (placementId == AndroidInterstitialAdUnitId || placementId == IosInterstitialAdUnitId) InterstitialAdShowState = AdState.No;
        else RewardAdLoadState = AdState.No;

        Debug.Log($"an error occurred while displaying the ad - {error}: {message}");
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowStart(string placementId) { }
    #endregion
}
