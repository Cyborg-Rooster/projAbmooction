using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoreController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Labels")]
    [SerializeField] GameObject SkinConteinerLabel;
    [SerializeField] GameObject MoneysLabel;
    [SerializeField] List<SkinController> Skins;
    [SerializeField] HighlightController HighlightController;

    List<int> SkinsBought = new List<int>();

    void Start()
    {
        GetSkinBoughtList();
        UIManager.SetText(SkinConteinerLabel, Strings.lblSkins);

        for (int i = 0; i < Skins.Count; i++) Skins[i].SetNameAndPrice(Strings.skins[i], SkinsBought.Contains(i), i);

        HighlightController.SetHighlight(Skins[GameData.Skin].transform);
    }

    void Update()
    {
        UIManager.SetText(MoneysLabel, GameData.Coins);
    }

    public void GetSkinBoughtList()
    {
        string[] s = SQLiteManager.ReturnValueAsString(CommonQuery.Select("SKIN_ID", "SKINS")).Split(' ');
        for (int i = 0; i < s.Length; i++) Debug.Log(s[i]);
        SkinsBought = s.Select(int.Parse).ToList();
    }
}
