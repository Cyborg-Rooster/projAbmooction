using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class ScenarioController : MonoBehaviour
{
    [SerializeField] GameObject NameLabel;
    [SerializeField] GameObject ImageBackground;
    [SerializeField] HighlightController HighlightController;
    [SerializeField] Material BlackMaterial;

    [SerializeField] bool Bought;
    [SerializeField] int ID;

    Material startMaterial;
    StoreController controller;

    public void SetNameAndMaterial(string name, bool bought, int id, StoreController storeController)
    {
        startMaterial = UIManager.ReturnImageMaterial(ImageBackground);
        controller = storeController;
        Bought = bought;
        ID = id;
        UIManager.SetText(NameLabel, name);

        //if (!Bought) UIManager.SetMaterialOnImage(ImageBackground, BlackMaterial);
    }

    public void SetScenario()
    {
        if (Bought)
        {
            GameData.Scenario = ID;
            HighlightController.SetHighlight(transform);
            SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"SCENARIO = {GameData.Skin}", "SCENARIO = SCENARIO"));
        }
        else controller.ShakeCamera();
    }
}
