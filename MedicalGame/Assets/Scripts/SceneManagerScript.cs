using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript management;

    void Awake()
    {
        if(management == null)
        {
            DontDestroyOnLoad(gameObject);
            management = this;
        }
        else if (management != this)
        {
            Destroy(gameObject);
        }
    }

    //Loading Scenes
    public void LoadLevelPage(string changeScene)
    {
		Loader.I.enableLoader();
		Loader.I.LoadScene(changeScene);
    }

    public void ExitApp()
    {
        Application.Quit();
    }


}
