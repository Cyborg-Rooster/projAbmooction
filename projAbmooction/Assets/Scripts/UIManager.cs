using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
class UIManager
{
    public static void SetRandomScale(GameObject gameObject)
    {
        System.Random r = new System.Random();
        if (r.Next(0, 2) == 1) gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }

    public static void SetText(GameObject output, string text)
    {
        SetText(output.GetComponent<Text>(), text);
    }

    public static void SetText(GameObject output, int text)
    {
        SetText(output.GetComponent<Text>(), text.ToString());
    }

    public static void SetImage(GameObject output, Sprite sprite)
    {
        output.GetComponent<Image>().sprite = sprite;
    }

    public static void SetButtonState(GameObject output, bool active)
    {
        output.GetComponent<Button>().interactable = active;
    }

    static void SetText(Text output, string text)
    {
        output.text = text.ToString();
    }
}
