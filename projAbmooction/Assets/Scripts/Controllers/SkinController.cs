using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    [SerializeField] SkinPrice Price;
    [SerializeField] GameObject PriceLabel;

    /*int[] prices = new int[6]
    {
        300,
        500,
        700,
        1000,
        2000,
        10000
    };*/


    // Start is called before the first frame update
    void Start()
    {
        /*if (Price == SkinPrice.ThreeHundred) UIManager.SetText(PriceLabel, prices[0]);
        else if (Price == SkinPrice.FiveHundred) UIManager.SetText(PriceLabel, prices[1]);
        else if (Price == SkinPrice.SevenHundred) UIManager.SetText(PriceLabel, prices[2]);
        else if (Price == SkinPrice.Thousand) UIManager.SetText(PriceLabel, prices[3]);
        else if (Price == SkinPrice.TwoThousand) UIManager.SetText(PriceLabel, prices[4]);
        else if (Price == SkinPrice.TenThousand) UIManager.SetText(PriceLabel, prices[5]);
        else UIManager.SetText(PriceLabel, Strings.lblBought);*/

        if(Price == SkinPrice.Free) UIManager.SetText(PriceLabel, Strings.lblBought);
        else UIManager.SetText(PriceLabel, $"{(int)Price} {Strings.coins}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
