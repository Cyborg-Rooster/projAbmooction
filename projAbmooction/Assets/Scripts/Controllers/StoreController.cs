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
    [SerializeField] GameObject ScenarioConteinerLabel;
    [SerializeField] GameObject ImprovementsConteinerLabel;
    [SerializeField] GameObject BoxConteinerLabel;

    [Header("Contents")]
    [SerializeField] List<SkinController> Skins;
    [SerializeField] List<ScenarioController> Scenarios;
    [SerializeField] List<ImprovementsController> Improvements;
    [SerializeField] List<BoxController> Boxes;

    [Header("Controllers")]
    [SerializeField] HighlightController SkinHighlightController;
    [SerializeField] HighlightController ScenarioHighlightController;
    [SerializeField] FadeController FadeAnimator;
    [SerializeField] GameObject OfflineButton;

    [Header("Network")]
    [SerializeField] AdvertisementController AdvertisementController;
    [SerializeField] AdvertisementInitializerController AdvertisementInitializerController;

    List<int> SkinsBought = new List<int>();
    List<int> ScenariosBought = new List<int>();

    void Start()
    {
        GetBoughtList();
        UIManager.SetText(SkinConteinerLabel, Strings.lblSkins);
        UIManager.SetText(ScenarioConteinerLabel, Strings.lblScenarios);
        UIManager.SetText(ImprovementsConteinerLabel, Strings.lblImprovements);
        UIManager.SetText(BoxConteinerLabel, Strings.lblSlots);

        //instance boxes
        InstanceBoxes();

        FadeAnimator.StartCoroutine(FadeAnimator.StartFade(false));

        //instance skins
        for (int i = 0; i < Skins.Count; i++) Skins[i].SetNameAndPrice(Strings.skins[i], SkinsBought.Contains(i), i);

        //instance scenarios
        for (int i = 0; i < Scenarios.Count; i++)
            Scenarios[i].SetNameAndMaterial(Strings.scenarios[i], ScenariosBought.Contains(i), i);

        //instance itens improvements
        UpdateItens();

        SkinHighlightController.SetHighlight(Skins[GameData.Skin].transform);
        ScenarioHighlightController.SetHighlight(Scenarios[GameData.Scenario].transform);
    }

    public void UpdateItens()
    {
        GameData.SetItems();
        foreach (var i in Improvements) 
        {
            if (i.ItemType == ImprovementsItem.Doubled) i.SetAttributes(GameData.Doubled, this);
            else if (i.ItemType == ImprovementsItem.Magnetic) i.SetAttributes(GameData.Magnetic, this);
            else if (i.ItemType == ImprovementsItem.Shield) i.SetAttributes(GameData.Shield, this);
            else i.SetAttributes(GameData.SlowMotion, this);
        }
    }

    public void TryReconnect()
    {
        StartCoroutine(Reconnect());
    }

    void Update()
    {
        UIManager.SetText(MoneysLabel, GameData.Coins);
    }

    void GetBoughtList()
    {
        string[] skins = SQLiteManager.ReturnValueAsString(CommonQuery.Select("SKIN_ID", "SKINS")).Split(' ');
        string[] scenarios = SQLiteManager.ReturnValueAsString(CommonQuery.Select("SKIN_ID", "SKINS")).Split(' ');

        //for (int i = 0; i < skins.Length; i++) Debug.Log(skins[i]);
        SkinsBought = skins.Select(int.Parse).ToList();
        ScenariosBought = scenarios.Select(int.Parse).ToList();
    }

    private void OnApplicationQuit()
    {
        SQLiteManager.SetDatabaseActive(false); 
    }

    IEnumerator Reconnect()
    {
        yield return NetworkManager.ConnectAndLoad(AdvertisementInitializerController, AdvertisementController);
        InstanceBoxes();
    }

    private void InstanceBoxes()
    {
        if (GameData.networkState == NetworkStates.Online)
        {
            for (int i = 0; i < Boxes.Count; i++)
            {
                Boxes[i].gameObject.SetActive(true);
                Boxes[i].SetBox(GameData.Boxes[i], i);
            }

            OfflineButton.SetActive(false);
        }
        else
        {
            for (int i = 0; i < Boxes.Count; i++)
                Boxes[i].gameObject.SetActive(false);
            OfflineButton.SetActive(true);
        }
    }
}
