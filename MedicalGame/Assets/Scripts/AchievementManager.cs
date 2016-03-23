using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour {

    public GameObject achievementPrefab;

    public Sprite[] sprites;
    private float fadeTime = 1.5f;
    public GameObject visualAchievement;
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    public Sprite unlockedSprite;

    private static AchievementManager instance;
   /* public static AchievementManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<AchievementManager>;
            }
        }
        return AchievementManager.instance; 
    }*/

	// Use this for initialization
	void Start ()
    {
        //Creates the general achievments
        CreateAchievement("AchievementHolder", "Mouse", "You Clicked the mouse button", 10, 0);
        CreateAchievement("AchievementHolder", "Start", "Starten van je eerste spel", 5, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {

         //creating achviement popup
         if(Input.GetMouseButtonUp(1))
         {
            //Create visual achievement
            EarnAchievement("Mouse");
         }

    }//end of update

    public void EarnAchievement(string title)
    {
        if( achievements[title].EarnAchievement())
        {
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
            SetAchievementInfo("EarnCanvas", achievement, title);
            StartCoroutine(HideAchievement(achievement));
        }
    }


   
 
    //Creat achievement prefab
    public void CreateAchievement(string parent, string title, string description, int points, int spriteIndex)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);
        Achievement newAchievement = new Achievement(name, description, points, spriteIndex, achievement);
        //saving new achievements to dictionairy
        achievements.Add(title, newAchievement);
        SetAchievementInfo(parent, achievement, title);
        //StartCoroutine(FadeAchievement(achievement));

    }
    //Destroy achievement afeter 3 seconds
    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);

    }


    //Set information about achievement
    public void SetAchievementInfo(string parent, GameObject achievement, string title)
    {
        achievement.transform.SetParent(GameObject.Find(parent).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);
        achievement.transform.localPosition = new Vector3(0,0, 0);
        // Getting achievement childeren
        achievement.transform.GetChild(0).GetComponent<Text>().text = title;
        achievement.transform.GetChild(1).GetComponent<Text>().text = achievements[title].Description;
        achievement.transform.GetChild(2).GetComponent<Text>().text = achievements[title].Points.ToString();
        achievement.transform.GetChild(3).GetComponent<Image>().sprite = sprites [achievements[title].SpriteIndex];
    }

    //fade in/out
    /*private IEnumerator FadeAchievement(GameObject achievement)
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
        
    }*/
        
}
