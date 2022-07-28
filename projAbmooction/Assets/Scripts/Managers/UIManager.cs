using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
class UIManager
{
    #region "Text"
    public static void SetText(GameObject output, string text)
    {
        SetText(output.GetComponent<Text>(), text);
    }

    public static void SetText(GameObject output, int text)
    {
        SetText(output.GetComponent<Text>(), text.ToString());
    }
    static void SetText(Text output, string text)
    {
        output.text = text.ToString();
    }
    #endregion

    #region "Image"
    public static void SetImage(GameObject output, Sprite sprite)
    {
        output.GetComponent<Image>().sprite = sprite;
    }

    public static Sprite GetImage(GameObject output)
    {
        return output.GetComponent<Image>().sprite;
    }

    public static void SetRandomScale(GameObject gameObject)
    {
        System.Random r = new System.Random();
        if (r.Next(0, 2) == 1) gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }

    public static void SetMaterialOnImage(GameObject output, Material material)
    {
        output.GetComponent<Image>().material = material;
    }
    public static Material ReturnImageMaterial(GameObject output)
    {
        return output.GetComponent<Image>().material;
    }

    #endregion

    #region "General UI Components"
    public static void SetButtonState(GameObject output, bool active)
    {
        output.GetComponent<Button>().interactable = active;
    }

    public static float ChangeSlider(GameObject slider, bool decrease, float value)
    {
        Slider s = slider.GetComponent<Slider>();
        if (decrease) s.value -= value;
        else s.value += value;
        return s.value;
    }

    public static void SetSliderValue(GameObject slider, float value)
    {
        slider.GetComponent<Slider>().value = value;
    }
    #endregion
}
