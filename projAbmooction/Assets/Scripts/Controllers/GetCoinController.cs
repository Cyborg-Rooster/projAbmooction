using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCoinController : MonoBehaviour
{
    [SerializeField] GameObject Text;
    [SerializeField] DialogBoxBuilderController Builder;
    [SerializeField] AdvertisementController AdvertisementController;

    StoreController StoreController;
    //
    int ID;
    bool Caught;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetGetCoin(int id, StoreController storeController)
    {
        ID = id;
        StoreController = storeController;

        UIManager.SetText(Text, Strings.getCoins[ID]);
        if (ID != 0)
            Caught = SQLiteManager.ReturnValueAsInt
            (
                CommonQuery.Select("CAUGHT", "GET_COINS", $"GET_COINS_ID = {ID}")
            ) == 1;
    }

    public void SeeAnAd()
    {
        StartCoroutine(SeeAd());
    }

    public void LikeOrFollowThePage(bool like)
    {
        if (like) 
        {
            if (Caught) StartCoroutine(AlreadyCaught(Strings.AlreadyLiked));
            else StartCoroutine(LikeOrFollow(Strings.LikePage, "https://www.facebook.com/enrisc0")); 
        }
        else 
        {
            if (Caught) StartCoroutine(AlreadyCaught(Strings.AlreadyFollow));
            else StartCoroutine(LikeOrFollow(Strings.FollowPage, "https://www.instagram.com/enrisco.mp4/"));
        }
    }

    public void ConnectOnFacebook()
    {

    }

    private IEnumerator SeeAd()
    {
        yield return Builder.ShowTyped
        (
            Strings.lblSkins,
            $"{Strings.SeeAnADAndGetCoins}",
            true
        );

        if (Builder.LastButtonState == ButtonPressed.Yes)
        {
            if (GameData.NetworkState == NetworkStates.Online)
            {
                AdvertisementController.LoadRewarded();
                yield return new WaitUntil(() => AdvertisementController.RewardAdLoadState != AdState.Null);

                if (GameData.NetworkState != NetworkStates.Online || AdvertisementController.RewardAdLoadState != AdState.Yes)
                {
                    StoreController.InstanceNetworkItens();
                    yield return Builder.ShowTyped(Strings.titleError, Strings.contentError, false);
                }
                else
                {
                    AdvertisementController.ShowRewarded();
                    yield return new WaitUntil(() => AdvertisementController.RewardAdShowState != AdState.Null);

                    if (AdvertisementController.RewardAdShowState == AdState.Yes)
                    {
                        GameData.Coins += 500;
                        SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = {GameData.Coins}", "COINS = COINS"));
                    }
                }
            }
            else
            {
                StoreController.InstanceNetworkItens();
                yield return Builder.ShowTyped(Strings.titleError, Strings.contentError, false);
            }
        }
    }

    private IEnumerator LikeOrFollow(string message, string url)
    {
        yield return Builder.ShowTyped
        (
            Strings.lblCoins,
            message,
            true
        );

        if (Builder.LastButtonState == ButtonPressed.Yes)
        {
            GameData.Coins += 1000;
            SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = {GameData.Coins}", "COINS = COINS"));
            SQLiteManager.RunQuery(CommonQuery.Update("GET_COINS", $"CAUGHT = 1", $"GET_COINS_ID = {ID}"));
            Caught = true;

            Application.OpenURL(url);
        }
    }

    private IEnumerator AlreadyCaught(string message)
    {
        yield return Builder.ShowTyped
        (
            Strings.lblCoins,
            message,
            false
        );
    }
}
