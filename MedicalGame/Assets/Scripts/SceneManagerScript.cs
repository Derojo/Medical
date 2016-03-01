using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
   
    void Awake()
    {
        DontDestroyOnLoad(this);
    }


    //Loading Scenes
    public void LoadLevelPage(string changeScene)
    {
        SceneManager.LoadScene(changeScene);
    }


}
