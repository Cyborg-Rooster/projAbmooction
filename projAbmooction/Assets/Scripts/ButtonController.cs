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

    public bool clicked;
    public bool multipleClicks;

    public void OnClick()
    {
        StartCoroutine(Click());
    }

    public void SetButtonState(bool active)
    {
        if (active) UIManager.SetImage(gameObject, defaultSprite);
        else UIManager.SetImage(gameObject, clickedSprite);
        UIManager.SetButtonState(gameObject, true);
    }

    IEnumerator Click()
    {
        yield return new WaitForSeconds(0.1f);
        UIManager.SetImage(gameObject, clickedSprite);

        yield return new WaitForSeconds(0.1f);
        if (multipleClicks) UIManager.SetImage(gameObject, defaultSprite);
        else UIManager.SetButtonState(gameObject, false);
    }

}
