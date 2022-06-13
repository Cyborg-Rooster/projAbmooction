using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    public GamePhase Phase = GamePhase.OnMain;

    PlayableDirector PlayableDirector;

    [SerializeField] CowController CowController;

    private void Start()
    {
        PlayableDirector = GetComponent<PlayableDirector>();
    }

    #region "Buttons methods"
    public void OnButtonPlayClicked()
    {
        PlayableDirector.Play();
        StartCoroutine(StartGame());
    }
    #endregion

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(4f);
        Phase = GamePhase.OnGame;
        CowController.StartPhase();
    }
}
