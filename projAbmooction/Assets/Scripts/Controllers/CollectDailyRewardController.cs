using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CollectDailyRewardController : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] GameObject TextQuantity;
    [SerializeField] GameObject TextDay;

    [Header("Adquired")]
    [SerializeField] GameObject TextAcquired;
    [SerializeField] GameObject SpriteHighlight;
    [SerializeField] bool IsAdquired;

    [Header("Item Type")]
    [SerializeField] GameObject SpriteItem;
    [SerializeField] Sprite FinalRewardSprite;
    [SerializeField] bool IsFinalReward;

    int Day;
    int Quantity;
    bool CanBeRewarded;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDailyReward(int day, int quantity, bool acquired)
    {
        IsAdquired = acquired;
        Day = day;
        Quantity = quantity;

        if(IsFinalReward)
        {
            CanBeRewarded = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("CAN_BE_REWARDED", "DAILY_REWARD")) == 1;
            if(CanBeRewarded)
            {
                UIManager.SetSprite(SpriteItem, FinalRewardSprite);
                TextQuantity.SetActive(false);
                SpriteItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else UIManager.SetText(TextQuantity, $"{Quantity}X");
        }
        else UIManager.SetText(TextQuantity, $"{Quantity}X");

        UIManager.SetText(TextDay, $"{Strings.Day} {Day}");

        if(IsAdquired)
        {
            TextAcquired.SetActive(true);
            SpriteHighlight.SetActive(true);
        }
    }

    public void OnDailyRewardClicked()
    {
        if (!IsAdquired)
        {
            string day = SQLiteManager.ReturnValueAsString(CommonQuery.Select("NEXT_DAY", "DAILY_REWARD"));
            if (DateTime.Parse(day).Date < GameData.DateTimeNow.Date && Day == 1 || DateTime.Parse(day).Date == GameData.DateTimeNow.Date)
            {
                if (Day == 30)
                {
                    if (CanBeRewarded)
                    {

                    }
                    else GetCoinsReward();

                    SQLiteManager.RunQuery
                    (
                        CommonQuery.Update
                        (
                            "DAILY_REWARD",
                            $"LAST_DAY = -1, NEXT_DAY = '{GameData.DateTimeNow.Date.AddDays(1)}', CAN_BE_REWARDED = 0",
                            "LAST_DAY = LAST_DAY"
                        )
                    );
                }
                else
                {
                    GetCoinsReward();

                    SQLiteManager.RunQuery
                    (
                        CommonQuery.Update
                        (
                            "DAILY_REWARD",
                            $"LAST_DAY = {Day - 1}, NEXT_DAY = '{GameData.DateTimeNow.Date.AddDays(1)}'",
                            "LAST_DAY = LAST_DAY"
                        )
                    );
                }

                TextAcquired.SetActive(true);
                SpriteHighlight.SetActive(true);

                IsAdquired = true;
            }
        }
    }

    void GetCoinsReward()
    {
        GameData.Coins += Quantity;
        SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = {GameData.Coins}", "COINS = COINS"));
    }
}
