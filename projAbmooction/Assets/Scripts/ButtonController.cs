using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class ButtonController : MonoBehaviour
{
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Sprite clickedSprite;
    //[SerializeField] SoundController audioSource;

    public bool Clicked;
    public bool MultipleClicks;
    //public bool StartInteractive;

    public void OnClick()
    {
        if(!MultipleClicks) UIManager.SetButtonState(gameObject, false);
        StartCoroutine(Click());

    }

    public void SetButtonState(bool active)
    {
        if (active) UIManager.SetImage(gameObject, defaultSprite);
        else UIManager.SetImage(gameObject, clickedSprite);
        UIManager.SetButtonState(gameObject, active);
    }

    IEnumerator Click()
    {
        UIManager.SetImage(gameObject, clickedSprite);
        yield return new WaitForSeconds(0.1f);
        if (MultipleClicks) UIManager.SetImage(gameObject, defaultSprite);
        else UIManager.SetButtonState(gameObject, false);
    }

}
