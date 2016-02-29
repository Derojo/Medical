using UnityEngine;
using System.Collections;

public class AchievementManager : MonoBehaviour {

    public GameObject achievementPrefab;
    private float fadeTime = 1.5f;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {

    }
    //creating achviement popup
    public void OnClick()
    {
        //Create visual achievement
        CreateAchievement("General");
    }
 
    //Creat achievement prefab
    public void CreateAchievement(string category)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);
        SetAchievementInfo(category, achievement);
        StartCoroutine(FadeAchievement(achievement));

    }
    //Destroy achievement afeter 3 seconds
    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }


    //Set information about achievement
    public void SetAchievementInfo(string category, GameObject achievement)
    {
        achievement.transform.SetParent(GameObject.Find(category).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);
        achievement.transform.localPosition = new Vector3(0,0, 0);
       
    }

    //fade in/out
    private IEnumerator FadeAchievement(GameObject achievement)
    {
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();
        float rate = 1.0f / fadeTime;

        int startAlpha = 0;
        int endAlpha = 1;

        
        

        for (int i = 0; i < 2; i++)
        {
            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                progress += rate * Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(2);
            startAlpha = 1;
            endAlpha = 0;
        }

        Destroy(achievement);
        
    }
        
}
