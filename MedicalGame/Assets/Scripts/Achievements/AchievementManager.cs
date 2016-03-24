using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[Prefab("AchievementManager", true, "")]
public class AchievementManager : Singleton<AchievementManager> {

    public GameObject achievementPrefab;

    //public Sprite[] sprites;
    private float fadeTime = 1.5f;
    public GameObject visualAchievement;
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();
    public List<Achievement> achievementList = new List<Achievement>();     

    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    /*  private static AchievementManager instance;

      public static AchievementManager Instance
      {
          get
          {
              if (instance == null) //If the instance isn't instantiated we need to find it
              {
                  instance = GameObject.FindObjectOfType<AchievementManager>();
              }
              return AchievementManager.instance;
          }
      }*/
    public bool Load() { return true; }


    void OnEnable() {

            achievements = new Dictionary<string, Achievement>();
            CreateAllAchievements();
        
   
    }
    // Use this for initialization
    void Start ()
    {

        if (achievementList == null) {
            achievementList = new List<Achievement>();
        }
        //Deleting playerprefs for testing REMEMBER TO REMOVE
        //PlayerPrefs.DeleteAll();
        //Creates the general achievments

    }
	
	// Update is called once per frame
	void Update ()
    {

         //creating achviement popup
         if(Input.GetMouseButtonUp(1))
         {
            //Create visual achievement
            EarnAchievement("Achievement 1");
         }

         if(Input.GetKeyDown(KeyCode.S))
        {
            EarnAchievement("Achievement 2");
        }

    }//end of update

    public void EarnAchievement(string title)
    {
        
        if( achievements[title].EarnAchievement())
        {
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
            SetAchievementInfo("EarnCanvas", achievement, title);
            StartCoroutine(FadeAchievement(achievement));
        }
    }

    public void CreateAllAchievements() {
        for (int i = 0; i < achievementList.Count; i++) {

            achievementList[i].LoadAchievement();
            // Achievement newAchievement = new Achievement(achievementList[i].Name, achievementList[i].Description, achievementList[i].Points, achievementList[i].ASprite);
            achievements.Add(achievementList[i].Name, achievementList[i]);
        }
    }
    
    //Creat achievement prefab
    public void CreateAchievement(string parent, string title, string description, int points, int spriteIndex)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);
        //Achievement newAchievement = new Achievement(title, description, points, spriteIndex, achievement);
        //saving new achievements to dictionairy
       // achievements.Add(title, newAchievement);
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
        achievement.transform.GetChild(3).GetComponent<Image>().sprite = achievements[title].ASprite;

        Debug.Log(achievements[title].Unlocked);

        
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


    public void checkAllAchievements() {
        checkAchievement1();
        checkAchievement2();

    }

    private void checkAchievement1() {

    }

    private void checkAchievement2()
    {

    }

}
