using System.Collections;
using UnityEngine;

public class DialogBoxBuilderController : MonoBehaviour
{
    [SerializeField] GameObject DialogBox;
    [SerializeField] GameObject DialogBoxImage;
    [SerializeField] GameObject DialogBoxSlider;
    [SerializeField] GameObject DialogBoxWaiting;

    public ButtonPressed LastButtonState;
    // Start is called before the first frame update

    public IEnumerator ShowTyped(string label, string content, bool type)
    {
        if (CheckIfExistAnotherDialogInstance()) Destroy(transform.GetChild(0).gameObject);

        GameObject d = Instantiate(DialogBox, transform);
        DialogBoxController c = d.GetComponent<DialogBoxController>();
        c.SetType(label, content, type);

        yield return new WaitUntil(() => c.button != ButtonPressed.Null);
        Destroy(d);
        LastButtonState = c.button;
    }

    public IEnumerator ShowImage(string label, string content, string yes, string no, Sprite image, Vector3 scaleImage)
    {
        if (CheckIfExistAnotherDialogInstance()) Destroy(transform.GetChild(0).gameObject);

        GameObject d = Instantiate(DialogBoxImage, transform);
        DialogBoxImageController c = d.GetComponent<DialogBoxImageController>();
        c.SetDialogBox(label, content, image, yes, no, scaleImage);

        yield return new WaitUntil(() => c.button != ButtonPressed.Null);
        Destroy(d);
        LastButtonState = c.button;
    }

    public IEnumerator ShowSlider(string label, string content, bool yesNo, Sprite image, float percent)
    {
        if (CheckIfExistAnotherDialogInstance()) Destroy(transform.GetChild(0).gameObject);

        GameObject d = Instantiate(DialogBoxSlider, transform);
        DialogBoxSliderController c = d.GetComponent<DialogBoxSliderController>();
        c.SetDialogBox(label, content, image, yesNo, percent);

        yield return new WaitUntil(() => c.button != ButtonPressed.Null);
        Destroy(d);
        LastButtonState = c.button;
    }

    public GameObject ShowWaiting()
    {
        if (CheckIfExistAnotherDialogInstance()) Destroy(transform.GetChild(0).gameObject);
        GameObject d = Instantiate(DialogBoxWaiting, transform);
        DialogBoxWaitingController c = d.GetComponent<DialogBoxWaitingController>();
        c.SetWaiting();

        //yield return new WaitUntil(() => FirebaseManager.BoxLoaded);
        //yield return new WaitUntil(() => GameData.NetworkState != NetworkStates.Null);

        //if (GameData.NetworkState == NetworkStates.Offline) 
        //yield return ShowTyped(Strings.titleError, Strings.contentError, false);

        return d;
    }

    public void CloseWaiting(GameObject waiting)
    {
        Destroy(waiting);
    }

    bool CheckIfExistAnotherDialogInstance()
    {
        return transform.childCount > 0;
    }
}
