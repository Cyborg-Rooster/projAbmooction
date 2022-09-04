using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    [SerializeField] AdvertisementInitializerController AdvertisementInitializerController;
    [SerializeReference] AdvertisementController AdvertisementController;

    bool splashscreenEnded = false;
    bool loaded = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator StartSplashscreen()
    {
        yield return new WaitForSeconds(2f);
        splashscreenEnded = true;
    }

    IEnumerator Load()
    {
        SQLiteManager.SetDatabase();
        GameData.Load();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        StartCoroutine(StartSplashscreen());

        yield return NetworkManager.ConnectAndLoad(AdvertisementInitializerController, AdvertisementController);

        loaded = true;
        
        yield return new WaitUntil(() => splashscreenEnded);
        SceneManager.LoadScene("sceGame");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && loaded) SceneManager.LoadScene("sceGame");
    }
}
