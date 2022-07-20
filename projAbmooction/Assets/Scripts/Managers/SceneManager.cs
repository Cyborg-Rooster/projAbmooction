using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
class SceneManager
{
    public static void RestartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene
        (
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public static void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
