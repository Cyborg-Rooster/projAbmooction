using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BoxRewardController : MonoBehaviour
{
    [Header("Labels")]
    [SerializeField] GameObject AdquiredLabel;
    [SerializeField] GameObject ItemLabel;
    [SerializeField] GameObject CongratulationsLabel;
    [SerializeField] GameObject ButtonLabel;

    [Header("Images")]
    [SerializeField] Animator Box;
    [SerializeField] SpriteRenderer ItemImage;

    [Header("Box Animators")]
    [SerializeField] RuntimeAnimatorController[] BoxTypes;

    [Header("Sprites")]
    [SerializeField] Sprite[] Cards;
    [SerializeField] Sprite Coins;

    [Header("Playables")]
    [SerializeField] PlayableDirector Director;
    [SerializeField] PlayableAsset GetRewardAsset;
    [SerializeField] PlayableAsset CloseRewardAsset;

    [SerializeField] CanvasGroup CanvasBelow;

    public void GetReward(Box box, BoxController boxController)
    {
        CanvasBelow.interactable = false;
        if (CheckIfChooseMoney())
        {
            int money = ReturnMoneyQuantity(box);
            GameData.Coins += money;

            SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = {GameData.Coins}", "COINS = COINS"));

            SetComponets
            (
                Strings.CollectCoins(money),
                Coins,
                BoxTypes[box.Type - 1],
                box,
                boxController
            );
        }
        else
        {
            int id = Random.Range(0, Cards.Length);
            while (!CheckIfCardIsValid(id)) id = Random.Range(0, Cards.Length);

            int quantityToAdd = ReturnScenarioQuantity(box);
            int actualQuantity = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("QUANTITY", "SCENARIOS", $"SCENARIO_ID = {id}"));

            SQLiteManager.RunQuery(CommonQuery.Update("SCENARIOS", $"QUANTITY = {actualQuantity + quantityToAdd}", $"SCENARIO_ID = {id}"));

            SetComponets
            (
                Strings.CollectCards(actualQuantity + quantityToAdd, id),
                Cards[id],
                BoxTypes[box.Type - 1],
                box,
                boxController
            );
        }
    }

    bool CheckIfCardIsValid(int id)
    {
        return SQLiteManager.ReturnValueAsInt
            (
                CommonQuery.Select("QUANTITY", "SCENARIOS", $"SCENARIO_ID = {id}")
            ) < (int)ScenarioChoosed(id);
    }

    public void CloseReward()
    {
        Director.playableAsset = CloseRewardAsset;
        Director.Play();

        CanvasBelow.interactable = true;
    }

    bool CheckIfChooseMoney()
    {
        return Random.Range(0, 2) == 1 ? true : false;
    }

    int ReturnMoneyQuantity(Box box)
    {
        if (box.Type == 1) return Random.Range(10, 101);
        else if (box.Type == 2) return Random.Range(100, 501);
        else return Random.Range(500, 1001);
    }

    int ReturnScenarioQuantity(Box box)
    {
        if (box.Type == 1) return Random.Range(1, 26);
        else if (box.Type == 2) return Random.Range(25, 51);
        else return Random.Range(50, 101);
    }

    void SetComponets(string reward, Sprite itemType, RuntimeAnimatorController boxType, Box box, BoxController boxController)
    {
        FirebaseManager.RemoveBox(box);
        GameData.Boxes[box.ID] = null;
        boxController.SetBox(null, box.ID);

        UIManager.SetText(CongratulationsLabel, Strings.Congratulations);
        UIManager.SetText(AdquiredLabel, Strings.YouAcquired);
        UIManager.SetText(ItemLabel, reward);
        UIManager.SetText(ButtonLabel, Strings.Collect);

        ItemImage.sprite = itemType;
        Box.runtimeAnimatorController = boxType;

        Director.playableAsset = GetRewardAsset;
        Director.Play();
    }

    ScenarioCards ScenarioChoosed(int id)
    {
        switch(id)
        {
            case 0: return ScenarioCards.Farm;
            case 1: return ScenarioCards.City;
            case 2: return ScenarioCards.Mountain;
            case 3: return ScenarioCards.Western;
            case 4: return ScenarioCards.Medieval;
            default: return ScenarioCards.Alien;
        }
    }
}
