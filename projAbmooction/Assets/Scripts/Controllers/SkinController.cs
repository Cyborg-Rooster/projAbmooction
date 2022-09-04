using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinController : MonoBehaviour
{
    [SerializeField] SkinPrice Price;
    [SerializeField] GameObject NameLabel;
    [SerializeField] GameObject PriceLabel;
    [SerializeField] HighlightController HighlightController;

    [SerializeField] bool Bought;
    [SerializeField] int ID;

    [SerializeField] DialogBoxBuilderController Builder;
    [SerializeField] AdvertisementController AdvertisementController;

    public void SetNameAndPrice(string name, bool bought, int id)
    {
        Bought = bought;
        ID = id;
        UIManager.SetText(NameLabel, name);

        if(Price == SkinPrice.Free || Bought) UIManager.SetText(PriceLabel, Strings.lblBought);
        else UIManager.SetText(PriceLabel, $"{(int)Price} {Strings.coins}");
    }

    public void SetSkin()
    {
        if (Bought)
        {
            GameData.Skin = ID;
            HighlightController.SetHighlight(transform);
            SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"SKIN = {GameData.Skin}", "SKIN = SKIN"));
        }
        else StartCoroutine(BuySkin());

    }

    IEnumerator BuySkin()
    {
        yield return Builder.ShowTyped
        (
            Strings.lblSkins,
            Strings.ContentBuySkin(Strings.skin, (int)Price),
            true
        );

        if(Builder.LastButtonState == ButtonPressed.Yes)
        {
            if (GameData.Coins > (int)Price) BuySuccesful();
            else yield return BuyFailed();
        }
    }

    private void BuySuccesful()
    {
        GameData.Coins -= (int)Price;

        string lastBought = SQLiteManager.ReturnValueAsString
        (
            CommonQuery.Select("SKIN_ID", "SKINS")
        ) + $" {ID}";

        SQLiteManager.RunQuery(CommonQuery.Update("SKINS", $"SKIN_ID = '{lastBought}'", "SKIN_ID = SKIN_ID"));
        SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = '{GameData.Coins}'", "COINS = COINS"));
        Bought = true;
        UIManager.SetText(PriceLabel, Strings.lblBought);
        SetSkin();
    }

    private IEnumerator BuyFailed()
    {
        yield return Builder.ShowTyped
        (
            Strings.lblSkins,
            $"{Strings.noMoneyEnough} {Strings.SeeAnADAndGetCoins}",
            true
        );

        if (Builder.LastButtonState == ButtonPressed.Yes)
        {
            AdvertisementController.ShowRewarded();
            yield return new WaitUntil(() => AdvertisementController.RewardAdState != RewardAdState.Null);

            if (AdvertisementController.RewardAdState == RewardAdState.Finish)
            {
                AdvertisementController.LoadRewarded();
                GameData.Coins += 500;
                SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = {GameData.Coins}", "COINS = COINS"));
            }
            else AdvertisementController.LoadRewarded();
        }
    }

            // Update is called once per frame
    void Update()
    {
        
    }
}
