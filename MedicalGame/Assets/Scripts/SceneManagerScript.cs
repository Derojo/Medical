using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
 

    //Loading Scenes
    public void LoadLevelPage(string changeScene)
    {
        SceneManager.LoadScene(changeScene);
    }


}
