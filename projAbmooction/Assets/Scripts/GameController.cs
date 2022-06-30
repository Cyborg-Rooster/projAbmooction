using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    PlayableDirector PlayableDirector;

    [Header("Objects")]
    [SerializeField] CowController CowController;
    [SerializeField] SpawnerController ObstacleSpawner;
    [SerializeField] ShakeObjectController Camera;

    [Header("UI")]
    [SerializeField] ButtonController PlayButton;
    [SerializeField] ButtonController PauseButton;

    [Header("Timelines")]
    [SerializeField] PlayableAsset TimelineEndGame;

    private void Start()
    {
        PlayableDirector = GetComponent<PlayableDirector>();
    }

    #region "Buttons methods"
    public void OnButtonPlayClicked()
    {
        if (GameData.Phase == GamePhase.OnMain)
        {
            PlayableDirector.Play();
            StartCoroutine(StartGame());
        }
        else if (GameData.Phase == GamePhase.OnGame) OnButtonPlayPause();
    }

    public void OnButtonPlayPause()
    {
        StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.2f);
        if (GameData.OnPause)
        {
            PauseButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);

            GameData.SpeedRange = GameData.LastRange;
            CowController.SetPause(true);
        }
        else
        {
            GameData.LastRange = GameData.SpeedRange;
            GameData.SpeedRange = 0f;

            CowController.SetPause(false);
            PauseButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
        }
        GameData.OnPause = !GameData.OnPause;
    }
    #endregion

    public void ShakeCamera()
    {
        Camera.ShakeObject();
    }

    public void Finish()
    {
        StartCoroutine(FinishGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(4f);
        GameData.Phase = GamePhase.OnGame;
        PauseButton.SetButtonState(true);
        CowController.StartPhase();
        ObstacleSpawner.SpawnNextTemplate();
        PlayButton.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    IEnumerator FinishGame()
    {
        GameData.Phase = GamePhase.OnFinish;
        //Debug.Break();
        PlayableDirector.playableAsset = TimelineEndGame;
        PlayableDirector.Play();
        yield return null;
    }
}
