using System.Collections;
using UnityEngine;

public class DialogBoxBuilderController : MonoBehaviour
{
    [SerializeField] GameObject DialogBox;
    [SerializeField] GameObject DialogBoxImage;
    [SerializeField] GameObject DialogBoxSlider;

    public ButtonPressed LastButtonState;
    // Start is called before the first frame update

    public IEnumerator ShowTyped(string label, string content, bool type)
    {
        if (!CheckIfExistAnotherDialogInstance())
        {
            GameObject d = Instantiate(DialogBox, transform);
            DialogBoxController c = d.GetComponent<DialogBoxController>();
            c.SetType(label, content, type);

            yield return new WaitUntil(() => c.button != ButtonPressed.Null);
            LastButtonState = c.button;
            Destroy(d);
        }
    }

    public IEnumerator ShowImage(string label, string content, string yes, string no, Sprite image, Vector3 scaleImage)
    {
        if (!CheckIfExistAnotherDialogInstance())
        {
            GameObject d = Instantiate(DialogBoxImage, transform);
            DialogBoxImageController c = d.GetComponent<DialogBoxImageController>();
            c.SetDialogBox(label, content, image, yes, no, scaleImage);

            yield return new WaitUntil(() => c.button != ButtonPressed.Null);
            LastButtonState = c.button;
            Destroy(d);
        }
    }

    public IEnumerator ShowSlider(string label, string content, bool yesNo, Sprite image, float percent)
    {
        if (!CheckIfExistAnotherDialogInstance())
        {
            GameObject d = Instantiate(DialogBoxSlider, transform);
            DialogBoxSliderController c = d.GetComponent<DialogBoxSliderController>();
            c.SetDialogBox(label, content, image, yesNo, percent);

            yield return new WaitUntil(() => c.button != ButtonPressed.Null);
            LastButtonState = c.button;
            Destroy(d);
        }
    }

    bool CheckIfExistAnotherDialogInstance()
    {
        return transform.childCount > 0;
    }
}
