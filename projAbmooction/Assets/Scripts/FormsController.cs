using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormsController : MonoBehaviour
{
    public static FormState State = FormState.Store;

    [Header("UI")]
    [SerializeField] FadeController Fade;
    [SerializeField] GameObject Label;

    [Header("Forms")]
    [SerializeField] GameObject FormsStore;

    // Start is called before the first frame update
    void Start()
    {
        SetFormState();
    }

    #region "Buttons methods"
    public void ReturnToGame()
    {
        StartCoroutine(BackToGame());
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetFormState()
    {
        if (State == FormState.Store) SpawnStore();
    }

    void SpawnStore()
    {
        UIManager.SetText(Label, Strings.lblStore);
        FormsStore.SetActive(true);
    }

    IEnumerator BackToGame()
    {
        yield return Fade.StartFade(true);
        SceneManager.LoadScene("sceGame");
    }
}
