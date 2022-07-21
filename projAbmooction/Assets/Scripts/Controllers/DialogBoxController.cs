using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBoxController : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] GameObject Content;
    [SerializeField] GameObject Label;
    [SerializeField] GameObject TextYesButton;
    [SerializeField] GameObject TextNoButton;
    [SerializeField] GameObject TextOkButton;

    [Header("Types")]
    [SerializeField] GameObject YesNo;
    [SerializeField] GameObject Ok;

    public ButtonPressed button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetType(string label, string content, bool type)
    {
        UIManager.SetText(Label, label);
        UIManager.SetText(Content, content);

        if (type)
        {
            UIManager.SetText(TextYesButton, Strings.yes);
            UIManager.SetText(TextNoButton, Strings.no);
            YesNo.SetActive(true);
        }
        else
        {
            UIManager.SetText(TextOkButton, Strings.ok);
            Ok.SetActive(true);
        }
    }

    public void SetButton(bool yes)
    {
        if (yes) button = ButtonPressed.Yes;
        else button = ButtonPressed.No;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
