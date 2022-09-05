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
    [SerializeField] VerticalParallaxController VerticalParallax;

    [Header("UI")]
    [SerializeField] FadeController Fade;
    [SerializeField] ButtonController PlayButton;
    [SerializeField] ButtonController ShopButton;
    [SerializeField] ButtonController PauseButton;
    [SerializeField] ButtonController AdButton;
    [SerializeField] GameObject RestartButton;
    [SerializeField] GameObject MainMenuButton;
    [SerializeField] GameObject TxtCoins;
    [SerializeField] GameObject TxtMeter;

    [Header("Timelines")]
    [SerializeField] PlayableAsset TimelineEndGame;

    [Header("Controllers")]
    [SerializeField] AdvertisementController AdvertisementController;

    [SerializeField] float SpeedRate;
    [SerializeField] float MaximumSpeedRange;
    [SerializeField] float RepeatingTimeToIncreaseObstacleSpeed;

    PlayableDirector PlayableDirector;

    public float actualSpeedrange;

    float NextRepeatingTime;
    float NextMeterUpTime;
    bool inEarth = true;
    bool waitingSky = false;

    static bool restartMode = false;
    static bool rewarded = false;

    private void Start()
    {
        Mechanics.RestartAttributes(rewarded);
        UIManager.SetText(TxtCoins, GameData.Coins);
        UIManager.SetText(TxtMeter, $"{Mechanics.Meters}m");
        UIManager.SetText(RestartButton, Strings.restart);
        UIManager.SetText(AdButton.transform.GetChild(0).gameObject, Strings.seeAnAd);
        UIManager.SetText(MainMenuButton, Strings.backToMain);

        if (rewarded || GameData.NetworkState == NetworkStates.Offline) AdButton.SetButtonState(false);
        rewarded = false;

        PlayableDirector = GetComponent<PlayableDirector>();

        //Fade.SetSpeed(0);
        //Fade.StartCoroutine(Fade.StartFade(false));

        if (restartMode) OnButtonPlayClicked();

        //InvokeRepeating("ShowTime", 1f, 1f);
    }

    #region "Buttons methods"
    public void OnButtonPlayClicked()
    {
        if (Mechanics.Phase == GamePhase.OnMain) StartCoroutine(StartGame());
        else if (Mechanics.Phase == GamePhase.OnGame) OnButtonPlayPauseClicked();
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

    public void OnButtonSeeAnAdClicked()
    {
        StartCoroutine(SeeAnAd());
    }

    public void OnButtonFormClicked(bool shop)
    {
        if (shop) 
        { 
            FormsController.State = FormState.Store;
            GameData.InterstitialTime++;
        }
        else FormsController.State = FormState.Options;

        StartCoroutine(GoToForms());
    }

    IEnumerator Pause()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.2f);
        if (Mechanics.OnPause)
        {
            PauseButton.gameObject.SetActive(true);
            PlayButton.gameObject.SetActive(false);

            Mechanics.SpeedRange = Mechanics.PauseLastRange;

            Planet.GetComponent<MovementController>().SetIsMoving(true);
            VerticalParallax.GetComponent<MovementController>().SetIsMoving(true);

            if (waitingSky) Sky.SetIsMoving(true);
            CowController.SetPause(true);
        }
        else
        {
            Mechanics.PauseLastRange = Mechanics.SpeedRange;
            Mechanics.SpeedRange = 0f;

            Planet.GetComponent<MovementController>().SetIsMoving(false);
            VerticalParallax.GetComponent<MovementController>().SetIsMoving(false);

            if (waitingSky) Sky.SetIsMoving(false);

            CowController.SetPause(false);
            PauseButton.gameObject.SetActive(false);
            PlayButton.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        Mechanics.OnPause = !Mechanics.OnPause;
    }

    IEnumerator GoToForms()
    {
        yield return Fade.StartFade(true);

        if(GameData.InterstitialTime == 3)
        {
            GameData.InterstitialTime = 0;
            AdvertisementController.ShowInterstitial();
        }

        SceneManager.LoadScene("sceForms");
    }
    #endregion

    private void Update()
    {
        if(Time.time > NextRepeatingTime)
        {
            NextRepeatingTime = Time.time + RepeatingTimeToIncreaseObstacleSpeed;
            if(Mechanics.Phase == GamePhase.OnGame && !Mechanics.OnPause) AdjustSpeedRange();
        }

        if(Time.time > NextMeterUpTime)
        {
            NextMeterUpTime = Time.time + 1 - (Mechanics.SpeedRange / 10);
            if (Mechanics.Phase == GamePhase.OnGame) AddMeters();
        }

        if (Mechanics.Meters > 100 && inEarth) StartCoroutine(GetOutOfEarth());
    }

    /*void ShowTime()
    {
        Debug.Log(GameData.DateTimeNow);
    }*/

    private void OnApplicationQuit()
    {
        SQLiteManager.SetDatabaseActive(false);
    }

    private void AdjustSpeedRange()
    {
        if(Mechanics.SpeedRange < MaximumSpeedRange) Mechanics.SpeedRange += SpeedRate;
        actualSpeedrange = Mechanics.SpeedRange;
    }

    public void ShakeCamera()
    {
        Camera.ShakeObject();
    }

    public void Finish()
    {
        GameData.Save();
        Mechanics.Phase = GamePhase.OnFinish;

        if(Mechanics.BoxCatched != null)
        {
            GameData.Boxes[Mechanics.BoxCatched.ID] = Mechanics.BoxCatched;
            FirebaseManager.SaveBox(Mechanics.BoxCatched);
        }

        Planet.GetComponent<MovementController>().SetIsMoving(false);
        VerticalParallax.GetComponent<MovementController>().SetIsMoving(false);

        if (waitingSky) Sky.SetIsMoving(false);

        PlayableDirector.playableAsset = TimelineEndGame;
        //TimelineBindingController.SetGenericBinding(PlayableDirector, CowController.GetComponent<Animator>());
        PlayableDirector.Play();
    }

    public void AddCoins(int coins)
    {
        GameData.Coins += coins;
        UIManager.SetText(TxtCoins, GameData.Coins);
    }

    private void AddMeters()
    {
        Mechanics.Meters++;
        UIManager.SetText(TxtMeter, $"{Mechanics.Meters}m");
    }

    IEnumerator StartGame()
    {
        UIManager.SetButtonState(PlayButton.gameObject, false);
        UIManager.SetButtonState(ShopButton.gameObject, false);

        //PlayableDirector.enabled = true;
        //TimelineBindingController.SetGenericBinding(PlayableDirector, CowController.GetComponent<Animator>());
        PlayableDirector.Play();

        yield return new WaitForSeconds(4f);
        Mechanics.Phase = GamePhase.OnGame;
        PauseButton.SetButtonState(true);
        CowController.StartPhase();
        ObstacleSpawner.SpawnNextTemplate();
        PlayButton.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        UIManager.SetButtonState(PlayButton.gameObject, true);
        VerticalParallax.gameObject.SetActive(true);
    }

    IEnumerator RestartGame()
    {
        yield return Fade.StartFade(true);
        SceneManager.RestartScene();
    }

    IEnumerator SeeAnAd()
    {
        yield return Fade.StartFade(true);
        AdvertisementController.ShowRewarded();
        yield return new WaitUntil(() => AdvertisementController.RewardAdState != RewardAdState.Null);

        if(AdvertisementController.RewardAdState == RewardAdState.Finish)
        {
            AdvertisementController.LoadRewarded();
            rewarded = true;
            restartMode = true;
            SceneManager.RestartScene();
        }
        else
        {
            AdvertisementController.LoadRewarded();
            yield return Fade.StartFade(false);
        }
    }

    IEnumerator GetOutOfEarth()
    {
        inEarth = false;

        yield return VerticalParallax.WaitForComeToTargetLocation();

        Sky.SetIsMoving(true);
        waitingSky = true;
        yield return Sky.WaitForComeToTargetPosition();
        waitingSky = false;

        Planet.SetActive(true);
        VerticalParallax.TurnStars();
        //VerticalParallax.SetActive(true);
    }
}
