using UnityEngine;
using System.Collections;

public class CanvasLoader : MonoBehaviour {

    public static CanvasLoader KeepAchievementCanvas;

    void Awake()
    {
        if (KeepAchievementCanvas == null)
        {
            DontDestroyOnLoad(gameObject);
            KeepAchievementCanvas = this;
        }
        else if (KeepAchievementCanvas != this)
        {
            Destroy(gameObject);
        }
    }
}
