using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
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
        FirebaseManager.Init();
        yield return ApiManager.GetCurrentTime("https://timeapi.io/api/Time/current/zone?timeZone=America/Sao_Paulo");
        FirebaseManager.LoadBox();
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
