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

    public RewardAdState RewardAdState = RewardAdState.Null;
    public RewardAdState RewardLoadState = RewardAdState.Null;

    #region "Load Ad"

    public void LoadInterstitial()
    {
        string AdUnitIdToLoad = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? IosInterstitialAdUnitId
            : AndroidInterstitialAdUnitId;

        LoadAd(AdUnitIdToLoad);
    }

    public void LoadRewarded()
    {
        RewardLoadState = RewardAdState.Null;
        RewardAdState = RewardAdState.Null;

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
        if (placementId == IosRewardedAdUnitId || placementId == AndroidRewardedAdUnitId) RewardLoadState = RewardAdState.Finish;

        Debug.Log(RewardLoadState);
        Debug.Log(placementId + " loaded successful. ");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        if (placementId == IosRewardedAdUnitId || placementId == AndroidRewardedAdUnitId) RewardLoadState = RewardAdState.Canceled;
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
        if (placementId == AndroidRewardedAdUnitId || placementId == IosRewardedAdUnitId)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED) RewardAdState = RewardAdState.Finish;
            else RewardAdState = RewardAdState.Canceled;
            //LoadRewarded();
        }
        else LoadInterstitial();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error: {error} - {message}");

        if (placementId == AndroidRewardedAdUnitId || placementId == IosRewardedAdUnitId)
        {
            RewardAdState = RewardAdState.Canceled;
            LoadRewarded();
        }
        else LoadInterstitial();
    }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowStart(string placementId) { }

   
    #endregion
}
