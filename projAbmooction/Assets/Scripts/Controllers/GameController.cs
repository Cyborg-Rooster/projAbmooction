using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
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
    [SerializeField] GameObject TxtBestScore;
    [SerializeField] GameObject TxtNew;
    [SerializeField] GameObject txtFinalBestScore;

    [Header("Timelines")]
    [SerializeField] PlayableAsset TimelineStartGame;
    [SerializeField] PlayableAsset TimelineEndGame;
    [SerializeField] PlayableAsset TimelineCollectDailyRewards;
    [SerializeField] PlayableAsset TimelineCloseDailyRewards;

    [Header("Controllers")]
    [SerializeField] AdvertisementController AdvertisementController;
    [SerializeField] DialogBoxBuilderController Builder;

    [Header("Daily Reward")]
    [SerializeField] List<CollectDailyRewardController> CollectDailyRewardControllers;
    [SerializeField] GameObject TextDailyLabel;
    [SerializeField] CanvasGroup CanvasBelow;

    [Header("Variables")]
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
        UIManager.SetText(TextDailyLabel, Strings.DailyReward);

        SetDailyRewards();

        Mechanics.RestartAttributes(rewarded);
        UIManager.SetText(TxtCoins, GameData.Coins);
        UIManager.SetText(TxtMeter, $"{Mechanics.Meters}m");
        UIManager.SetText(RestartButton, Strings.restart);
        UIManager.SetText(AdButton.transform.GetChild(0).gameObject, Strings.seeAnAd);
        UIManager.SetText(MainMenuButton, Strings.backToMain);
        UIManager.SetText(TxtBestScore, Strings.BestScore);
        UIManager.SetText(TxtNew, Strings.New);

        if (rewarded || GameData.NetworkState == NetworkStates.Offline) AdButton.SetButtonState(false);
        rewarded = false;

        PlayableDirector = GetComponent<PlayableDirector>();

        if (restartMode) OnButtonPlayClicked();
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

    public void OnButtonCollectDailyRewardClicked()
    { 
        CanvasBelow.interactable = false;
        PlayableDirector.playableAsset = TimelineCollectDailyRewards;
        PlayableDirector.Play();
    }

    public void OnButtonCloseCollectDailyRewardClicked()
    {
        PlayableDirector.playableAsset = TimelineCloseDailyRewards;
        PlayableDirector.Play();

        CanvasBelow.interactable = true;
    }

    public void OnButtonLogOnFacebookClicked()
    {
        //StartCoroutine(LogOnFacebookManager.LogOnFacebook(Builder));
    }


    IEnumerator Pause()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(0.2f);
        if (Mechanics.OnPause)
        {
            PlayButton.gameObject.SetActive(false);
            PauseButton.gameObject.SetActive(true);

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
        if(Mechanics.Meters > GameData.BestScore)
        {
            TxtNew.SetActive(true);
            GameData.BestScore = Mechanics.Meters;

            //if (FacebookManager.IsLogged) FirebaseManager.SaveData(OnlineData.ReturnOnlineData());
        }

        UIManager.SetText(txtFinalBestScore, $"{GameData.BestScore} m");
        GameData.Save();
        Mechanics.Phase = GamePhase.OnFinish;

        if(Mechanics.BoxCatched != null)
        {
            GameData.Boxes[Mechanics.BoxCatched.ID] = Mechanics.BoxCatched;
            //FirebaseManager.SaveBox(Mechanics.BoxCatched);
        }

        Planet.GetComponent<MovementController>().SetIsMoving(false);
        VerticalParallax.GetComponent<MovementController>().SetIsMoving(false);

        if (waitingSky) Sky.SetIsMoving(false);

        PlayableDirector.playableAsset = TimelineEndGame;
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

        PlayableDirector.playableAsset = TimelineStartGame;
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
        AdvertisementController.LoadRewarded();
        yield return new WaitUntil(() => AdvertisementController.RewardAdLoadState != DefaultState.Null);

        if (AdvertisementController.RewardAdLoadState == DefaultState.Yes)
        {
            yield return Fade.StartFade(true);
            AdvertisementController.ShowRewarded();
            yield return new WaitUntil(() => AdvertisementController.RewardAdShowState != DefaultState.Null);

            if (AdvertisementController.RewardAdShowState == DefaultState.Yes)
            {
                rewarded = true;
                restartMode = true;
                SceneManager.RestartScene();
            }
            else yield return Fade.StartFade(false);
        }
        else yield return Builder.ShowTyped(Strings.titleError, Strings.contentError, false);
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
    }

    void SetDailyRewards()
    {
        int lastRewardGetted = SQLiteManager.ReturnValueAsInt(CommonQuery.Select("LAST_DAY", "DAILY_REWARD"));

        Debug.Log
        (
            $"saved: {DateTime.Parse(SQLiteManager.ReturnValueAsString(CommonQuery.Select("NEXT_DAY", "DAILY_REWARD")))}, " +
            $"actual: {GameData.DateTimeNow.Date}, " +
            $"bool: {DateTime.Parse(SQLiteManager.ReturnValueAsString(CommonQuery.Select("NEXT_DAY", "DAILY_REWARD"))) < GameData.DateTimeNow.Date}"
        );

        if(DateTime.Parse(SQLiteManager.ReturnValueAsString(CommonQuery.Select("NEXT_DAY", "DAILY_REWARD"))) < GameData.DateTimeNow.Date)
                lastRewardGetted = -1;

        for (int i = 0; i < CollectDailyRewardControllers.Count; i++)
        {
            int quantity;
            if (i >= 0 && i <= 11) quantity = 200;
            else if (i >= 12 && i <= 23) quantity = 500;
            else if (i == 29) quantity = 5000;
            else quantity = 1000;

            CollectDailyRewardControllers[i].SetDailyReward(i + 1, quantity, i <= lastRewardGetted);
        }
    }
}
