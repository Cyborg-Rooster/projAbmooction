using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ScenarioChangeController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] SpriteRenderer Background;
    [SerializeField] SpriteRenderer Foreground;
    [SerializeField] SpriteRenderer Cloud1, Cloud2;

    [Header("Farm")]
    [SerializeField] Sprite farmBackground;
    [SerializeField] Sprite farmForeground;
    [SerializeField] Sprite farmCloud;
    [SerializeField] bool farmIsForeground;

    [Header("City")]
    [SerializeField] Sprite cityBackground;
    [SerializeField] Sprite cityForeground;
    [SerializeField] Sprite cityCloud;
    [SerializeField] bool cityIsForeground;

    private void Start()
    {
        if (GameData.Scenario == 0) SetAttributes(farmBackground, farmForeground, farmCloud, true);
        else if (GameData.Scenario == 1) SetAttributes(cityBackground, cityForeground, cityCloud, false);
    }

    private void SetAttributes(Sprite background, Sprite foreground, Sprite cloud, bool isForeground)
    {
        Background.sprite = background;
        Foreground.sprite = foreground;
        Cloud1.sprite = cloud; 
        Cloud2.sprite = cloud;

        if (isForeground) Foreground.sortingLayerName = "Foreground";
        else Foreground.sortingLayerName = "Default";
    }
}
