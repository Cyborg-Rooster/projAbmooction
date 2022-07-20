using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreController : MonoBehaviour
{
/*#if UNITY_EDITOR
    private void OnValidate()
    {
        Transform t = transform.GetChild(0).GetChild(1).GetChild(0);

        for (int i = 0; i < t.childCount; i++) SkinNameLabels.Add(t.GetChild(i).GetChild(1).GetChild(0).gameObject);
    }
#endif*/
    // Start is called before the first frame update
    [Header("Labels")]
    [SerializeField] GameObject SkinConteinerLabel;
    [SerializeField] GameObject MoneysLabel;
    [SerializeField] List<GameObject> SkinNameLabels;
    void Start()
    {
        UIManager.SetText(SkinConteinerLabel, Strings.lblSkins);
        UIManager.SetText(MoneysLabel, GameData.Coins);
        for (int i = 0; i < SkinNameLabels.Count; i++) UIManager.SetText(SkinNameLabels[i], Strings.Skins[i]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
