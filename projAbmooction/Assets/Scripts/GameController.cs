using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] CowController CowController;
    [SerializeField] SpawnerController ObstacleSpawner;
    [SerializeField] ShakeObjectController Camera;
    [SerializeField] MovementController Sky;
    [SerializeField] GameObject Planet;
    [SerializeField] GameObject Stars;

    [Header("UI")]
    [SerializeField] Animator Fade;
    [SerializeField] ButtonController PlayButton;
    [SerializeField] ButtonController PauseButton;
    [SerializeField] GameObject TxtCoins;
    [SerializeField] GameObject TxtMeter;

    [Header("Timelines")]
    [SerializeField] PlayableAsset TimelineEndGame;

    [SerializeField] int Coins;
    [SerializeField] float SpeedRate;
    [SerializeField] float MaximumSpeedRange;
    [SerializeField] float RepeatingTimeToIncreaseObstacleSpeed;

    PlayableDirector PlayableDirector;

    public float actualSpeedrange;

    float NextRepeatingTime;
    float NextMeterUpTime;
    float Meters;
    bool inEarth = true;

    static bool restartMode = false;

    private void Start()
    {
        GameData.RestartAttributes();

        PlayableDirector = GetComponent<PlayableDirector>();

        Fade.speed = 0;
        StartCoroutine(StartFade(false));

        if (restartMode) OnButtonPlayClicked();
    }

    #region "Buttons methods"
    public void OnButtonPlayClicked()
    {
        if (GameData.Phase == GamePhase.OnMain)
        {
            PlayableDirector.Play();
            StartCoroutine(StartGame());
        }
        else if (GameData.Phase == GamePhase.OnGame) OnButtonPlayPauseClicked();
    }

    public void OnButtonPlayPauseClicked()
    {
        StartCoroutine(Pause());
    }

    public void OnButtonRestartGameClicked(bool restart)
    {
        restartMode = restart;
        StartCoroutine(RestartGame());
    }
    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.2f);
        if (GameData.OnPause)
        {
            PauseButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);

            GameData.SpeedRange = GameData.PauseLastRange;
            CowController.SetPause(true);
        }
        else
        {
            GameData.PauseLastRange = GameData.SpeedRange;
            GameData.SpeedRange = 0f;

            CowController.SetPause(false);
            PauseButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
        }
        GameData.OnPause = !GameData.OnPause;
    }
    #endregion
    private void Update()
    {
        if(Time.time > NextRepeatingTime)
        {
            NextRepeatingTime = Time.time + RepeatingTimeToIncreaseObstacleSpeed;
            if(GameData.Phase == GamePhase.OnGame && !GameData.OnPause) AdjustSpeedRange();
        }

        if(Time.time > NextMeterUpTime)
        {
            NextMeterUpTime = Time.time + 1 - (GameData.SpeedRange / 10);
            if (GameData.Phase == GamePhase.OnGame) AddMeters();
        }

        if (Meters > 100 && inEarth) StartCoroutine(GetOutOfEarth());
    }

    private void AdjustSpeedRange()
    {
        if(GameData.SpeedRange < MaximumSpeedRange) GameData.SpeedRange += SpeedRate;
        actualSpeedrange = GameData.SpeedRange;
    }

    public void ShakeCamera()
    {
        Camera.ShakeObject();
    }

    public void Finish()
    {
        StartCoroutine(FinishGame());
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
        UIManager.SetText(TxtCoins, Coins);
    }

    private void AddMeters()
    {
        Meters++;
        UIManager.SetText(TxtMeter, $"{Meters}m");
    }

    IEnumerator StartFade(bool fadeIn)
    {
        if (fadeIn) Fade.Play("fadeIn");
        else
        {
            Fade.Play("fadeOut");
            yield return new WaitForSeconds(1f);
        }

        Fade.speed = 1;
        yield return new WaitForSeconds(.5f);
        Fade.speed = 0;
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

    IEnumerator RestartGame()
    {
        yield return StartFade(true);
        SceneManager.RestartScene();
    }

    IEnumerator GetOutOfEarth()
    {
        inEarth = false;
        Sky.SetIsMoving(true);
        yield return Sky.WaitForComeToTargetPosition();
        Planet.SetActive(true);
        Stars.SetActive(true);
    }
}
