using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{

    //Loading Scenes
    public void LoadLevelPage(string changeScene)
    {
        EditorSceneManager.LoadScene(changeScene);
    }
}
