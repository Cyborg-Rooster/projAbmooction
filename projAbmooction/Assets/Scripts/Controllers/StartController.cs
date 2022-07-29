using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SQLiteManager.SetDatabase();
        GameData.Load();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartCoroutine(StartSplashscreen());
    }

    IEnumerator StartSplashscreen()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("sceGame");
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)) SceneManager.LoadScene("sceGame");
    }
}
