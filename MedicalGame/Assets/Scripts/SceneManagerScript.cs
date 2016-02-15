using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    //Loading Home Scene
    public void LoadLevelPage(string changeScene)
    {
        EditorSceneManager.LoadScene(changeScene);
    }
}
