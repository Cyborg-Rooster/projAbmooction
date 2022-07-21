using System.Collections;
using UnityEngine;

public class DialogBoxBuilderController : MonoBehaviour
{
    [SerializeField] GameObject DialogBox;

    public ButtonPressed LastButtonState;
    // Start is called before the first frame update

    public IEnumerator Show(string label, string content, bool type)
    {
        GameObject d = Instantiate(DialogBox, transform);
        DialogBoxController c = d.GetComponent<DialogBoxController>();
        c.SetType(label, content, type);

        yield return new WaitUntil(() => c.button != ButtonPressed.Null);
        LastButtonState = c.button;
        Destroy(d);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
