using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class ImprovementsController : MonoBehaviour
{
    public ImprovementsItem ItemType;

    [SerializeField] GameObject label;
    [SerializeField] GameObject ItemImage; 
    [SerializeField] DialogBoxBuilderController Builder;

    Item Item;
    StoreController Store;

    private void Start()
    {
        //SetAttributes();
        //UIManager.SetText(label, $"{itemName} {percent}%");
    }

    public void SetAttributes(Item item, StoreController store)
    {
        Store = store;
        Item = item;
        UIManager.SetText(label, $"{item.Name} {item.Level}%");
        /*if (ID == 0) 
        {
            itemName = Strings.itemMagnetic;
            percent = GameData.improvementsMagnetic;
            time = Mechanics.MagneticTime;
        }
        else if (ID == 1) 
        {
            itemName = Strings.itemDouble;
            percent = GameData.improvementsDouble;
            time = Mechanics.DoubledTime;
        }
        else if (ID == 2) 
        {
            itemName = Strings.itemShield;
            percent = GameData.improvementsShield;
            time = Mechanics.ShieldTime;
        }
        else 
        {
            itemName = Strings.itemSlowMotion;
            percent = GameData.improvementsSlowMotion;
            time = Mechanics.SlowMotionTime;
        }*/
    }

    public void ChangeImprovements()
    {
        if (Item.Level < 100) StartCoroutine(AddChanges());
        else StartCoroutine(ShowFinishChanges());
    }

    IEnumerator ShowFinishChanges()
    {
        yield return Builder.ShowSlider
        (
            Strings.lblImprovements,
            Strings.ChangesFinish(Item.Name, Item.Time),
            false,
            UIManager.GetImage(ItemImage),
            ReturnDividedValue(Item.Level)
        );
    }

    IEnumerator AddChanges()
    {
        /* int price;
         if(per)
         yield return Builder.ShowSlider
         (
             Strings.lblImprovements, 
             Strings.AddChanges(name, time, 
         )
     }*/
        yield return Builder.ShowSlider
        (
            Strings.lblImprovements,
            Strings.AddChanges(Item.Name, Item.Time, Item.Price),
            true,
            UIManager.GetImage(ItemImage),
            //Item.Level / 100
            ReturnDividedValue(Item.Level)
        );

        if(Builder.LastButtonState == ButtonPressed.Yes)
        {
            if (GameData.Coins >= Item.Price)
            {
                GameData.Coins -= Item.Price;

                Item.Level += 20;
                Item.Price += 500;
                Item.Time += 2;

                SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"COINS = '{GameData.Coins}'", "COINS = COINS"));
                SQLiteManager.RunQuery(CommonQuery.Update("ITEMS", $"LEVEL = {Item.Level}, PRICE= {Item.Price}, TIME= {Item.Time}", $"ITEM_ID = {Item.ID}"));
                Store.UpdateItens();
                ChangeImprovements();
            }
        }
    }

    float ReturnDividedValue(float value)
    {
        Debug.Log(value / 100);
        return value / 100;
    }
    
}
