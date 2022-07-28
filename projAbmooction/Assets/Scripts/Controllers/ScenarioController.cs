using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class ScenarioController : MonoBehaviour
{
    [SerializeField] ScenarioCards CardsNeeded;
    [SerializeField] GameObject NameLabel;
    [SerializeField] GameObject ImageBackground;
    [SerializeField] HighlightController HighlightController;
    [SerializeField] Sprite BackgroundImage;

    [SerializeField] bool Bought;
    [SerializeField] int ID;

    [SerializeField] DialogBoxBuilderController Builder;

    int actualCards;

    public void SetNameAndMaterial(string name, bool bought, int id)
    {
        Bought = bought;
        ID = id;

        UIManager.SetText(NameLabel, name);
        actualCards = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("QUANTITY", "SCENARIOS", $"SCENARIO_ID = {ID}"));

        if (actualCards >= (int)CardsNeeded) Bought = true;

        //if (!Bought) UIManager.SetMaterialOnImage(ImageBackground, BlackMaterial);
    }

    public void SetScenario()
    {
        if (Bought) StartCoroutine(PurchasedScenarioSelector());
        else StartCoroutine(NotPurchasedScenarioSelector());
    }

    IEnumerator PurchasedScenarioSelector()
    {
        yield return Builder.ShowImage
        (
            Strings.lblScenarios,
            Strings.SelectScenario(Strings.scenarios[ID]),
            Strings.confirm, Strings.cancel,
            BackgroundImage
        );

        if (Builder.LastButtonState == ButtonPressed.Yes)
        {
            GameData.Scenario = ID;
            HighlightController.SetHighlight(transform);
            SQLiteManager.RunQuery(CommonQuery.Update("GAME_DATA", $"SCENARIO = {GameData.Scenario}", "SCENARIO = SCENARIO"));
        }
    }

    IEnumerator NotPurchasedScenarioSelector()
    {
        yield return Builder.ShowImage
        (
            Strings.lblScenarios,
            Strings.selectScenarioError,
            $"{actualCards}/{(int)CardsNeeded}", Strings.cancel,
            BackgroundImage
        );
    }
}
