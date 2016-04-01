using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class AchievementList : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ShowAllAchievements();
    }
	

    public void ShowAllAchievements()
    {
        for (int i = 0; i < AchievementManager.I.achievementList.Count; i++)
        {
            GameObject achievement = (GameObject)Instantiate(AchievementManager.I.achievementPrefab);
            AchievementManager.I.SetAchievementInfo("AchievementHolder", achievement, AchievementManager.I.achievementList[i].Name);
            if (AchievementManager.I.achievementList[i].Unlocked)
            {
                achievement.transform.GetChild(4).GetComponent<Image>().sprite = AchievementManager.I.achievementList[i].ASprite;

            }
            else
            {
                achievement.transform.GetChild(4).GetComponent<Image>().sprite = AchievementManager.I.achievementList[i].BSprite;
            }
        }
    }

}
