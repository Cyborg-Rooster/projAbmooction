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

    [Header("Contents")]
    [SerializeField] List<SkinController> Skins;
    [SerializeField] List<ScenarioController> Scenarios;

    [Header("Controllers")]
    [SerializeField] HighlightController SkinHighlightController;
    [SerializeField] HighlightController ScenarioHighlightController;
    [SerializeField] ShakeObjectController Camera;

    List<int> SkinsBought = new List<int>();
    List<int> ScenariosBought = new List<int>();

    void Start()
    {
        GetBoughtList();
        UIManager.SetText(SkinConteinerLabel, Strings.lblSkins);
        UIManager.SetText(ScenarioConteinerLabel, Strings.lblScenarios);

        for (int i = 0; i < Skins.Count; i++) Skins[i].SetNameAndPrice(Strings.skins[i], SkinsBought.Contains(i), i);
        for (int i = 0; i < Scenarios.Count; i++)
            Scenarios[i].SetNameAndMaterial(Strings.scenarios[i], ScenariosBought.Contains(i), i, this);

        SkinHighlightController.SetHighlight(Skins[GameData.Skin].transform);
        ScenarioHighlightController.SetHighlight(Scenarios[GameData.Scenario].transform);
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

    public void ShakeCamera()
    {

        Debug.Log("teste");
        Camera.ShakeObject();
    }
}
