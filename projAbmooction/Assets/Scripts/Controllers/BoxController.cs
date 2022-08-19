using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class BoxController : MonoBehaviour
{
    [SerializeField] GameObject BoxImage;
    [SerializeField] GameObject BoxLabel;
    [SerializeField] Sprite[] BoxSprites;

    Box Box;
    int ID;

    public void SetBox(Box box, int id)
    {
        ID = id;
        if (box == null) UpdateBox(BoxSprites[3], Strings.Empty, null);
    }


    private void UpdateBox(Sprite sprite, string label, Box box)
    {
        UIManager.SetImage(BoxImage, sprite);
        UIManager.SetText(BoxLabel, label);
        Box = box;
    }

    private void SetTypedBox(Box box)
    {
        string label;
        //if (box.A) { }
    }
}
