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

                achievement.GetComponent<Image>().sprite = AchievementManager.I.unlockedSprite;
            }
            else
            {
                achievement.GetComponent<Image>().sprite = AchievementManager.I.lockedSprite;
            }
        }
    }

}
