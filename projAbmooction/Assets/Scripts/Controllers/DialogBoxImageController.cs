using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBoxImageController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] GameObject Content;
    [SerializeField] GameObject Label;
    [SerializeField] GameObject Image;
    [SerializeField] GameObject TextYesButton;
    [SerializeField] GameObject TextNoButton;


    public ButtonPressed button;

    public void SetDialogBox(string label, string content, Sprite image, string yes, string no)
    {
        UIManager.SetText(Label, label);
        UIManager.SetText(Content, content);
        UIManager.SetImage(Image, image);
        UIManager.SetText(TextYesButton, yes);
        UIManager.SetText(TextNoButton, no);
    }

    public void SetButton(bool yes)
    {
        if (yes) button = ButtonPressed.Yes;
        else button = ButtonPressed.No;
    }
}
