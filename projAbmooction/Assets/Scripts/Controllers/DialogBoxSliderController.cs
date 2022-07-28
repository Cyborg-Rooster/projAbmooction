using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBoxSliderController : MonoBehaviour
{
    [Header("Label")]
    [SerializeField] GameObject Content;
    [SerializeField] GameObject Label;
    [SerializeField] GameObject TextYesButton;
    [SerializeField] GameObject TextNoButton;
    [SerializeField] GameObject TextOkButton;

    [Header("Components")]
    [SerializeField] GameObject Image;
    [SerializeField] GameObject Slider;
    [SerializeField] GameObject YesNoButton;
    [SerializeField] GameObject OkButton;




    public ButtonPressed button;

    public void SetDialogBox(string label, string content, Sprite image, bool yesNo, float percent)
    {
        UIManager.SetText(Label, label);
        UIManager.SetText(Content, content);
        UIManager.SetImage(Image, image);
        UIManager.SetSliderValue(Slider, percent);

        if (yesNo)
        {
            YesNoButton.SetActive(true);
            UIManager.SetText(TextYesButton, Strings.yes);
            UIManager.SetText(TextNoButton, Strings.no);
        }
        else
        {
            OkButton.SetActive(true);
            UIManager.SetText(TextNoButton, Strings.ok);
        }
    }

    public void SetButton(bool yes)
    {
        if (yes) button = ButtonPressed.Yes;
        else button = ButtonPressed.No;
    }
}
