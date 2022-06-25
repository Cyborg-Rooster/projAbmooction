using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    PlayableDirector PlayableDirector;

    [Header("Objects")]
    [SerializeField] CowController CowController;
    [SerializeField] ButtonController PauseButton;
    [SerializeField] SpawnerController ObstacleSpawner;

    [Header("Timelines")]
    [SerializeField] PlayableAsset TimelineEndGame;

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

    public void Finish()
    {
        StartCoroutine(FinishGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(4.1f);
        GameData.Phase = GamePhase.OnGame;
        PauseButton.SetButtonState(true);
        CowController.StartPhase();
        ObstacleSpawner.SpawnNextTemplate();
    }

    IEnumerator FinishGame()
    {
        GameData.Phase = GamePhase.OnFinish;
        CowController.SetStarted(false);
        PlayableDirector.playableAsset = TimelineEndGame;
        PlayableDirector.Play();
        yield return null;
    }
}
