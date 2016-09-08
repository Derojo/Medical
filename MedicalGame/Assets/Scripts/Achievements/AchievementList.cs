using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AchievementList : MonoBehaviour {
	public List<string> progressionNames;
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
			if(progressionNames.Contains(AchievementManager.I.achievementList[i].Name)) {
				setProgessInformation(achievement.transform.GetChild(6).gameObject, AchievementManager.I.achievementList[i].Name);
				achievement.transform.GetChild(6).gameObject.SetActive(true);
			}
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
	
	private void setProgessInformation(GameObject progression, string name) {
		float fillAmount = 0;
		string progressionText = "";
		if(name == "Old School") {
			fillAmount = getFillAmount(PlayerManager.I.player.oldieAnswers, 10);
			progressionText = PlayerManager.I.player.oldieAnswers+"/10";
		} else if(name == "Verslavingsoverwinnaar") {
			fillAmount = getFillAmount(PlayerManager.I.player.verslavingsAnswers, 10);
			progressionText = PlayerManager.I.player.verslavingsAnswers+"/10";
		} else if(name == "Arts") {
			fillAmount = getFillAmount(PlayerManager.I.player.artsAnswers, 10);
			progressionText = PlayerManager.I.player.artsAnswers+"/10";
		} else if(name == "Allrounder") {
			fillAmount = getFillAmount(PlayerManager.I.player.algemeenAnswers, 10);
			progressionText = PlayerManager.I.player.algemeenAnswers+"/10";
		} else if(name == "Ziekenhuis specialist") {
			fillAmount = getFillAmount(PlayerManager.I.player.ziekenhuisAnswers, 10);
			progressionText = PlayerManager.I.player.ziekenhuisAnswers+"/10";
		} else if(name == "Gehandicapten begeleider") {
			fillAmount = getFillAmount(PlayerManager.I.player.gehandicaptenAnswers, 10);
			progressionText = PlayerManager.I.player.gehandicaptenAnswers+"/10";
		} else if(name == "Professor") {
			fillAmount = getFillAmount(PlayerManager.I.player.rightAnswersTotal, 100);
			progressionText = PlayerManager.I.player.rightAnswersTotal+"/100";
		} else if(name == "Stylist") {
			fillAmount = getFillAmount(PlayerManager.I.player.avatarChanges, 10);
			progressionText = PlayerManager.I.player.avatarChanges+"/10";
		} else if(name == "Populair") {
			fillAmount = getFillAmount(PlayerManager.I.player.acceptedMatches, 5);
			progressionText = PlayerManager.I.player.acceptedMatches+"/5";
		} else if(name == "Afmaker") {
			fillAmount = getFillAmount(PlayerManager.I.player.playedMatches, 5);
			progressionText = PlayerManager.I.player.playedMatches+"/5";
		} else if(name == "Onverslaanbaar") {
			fillAmount = getFillAmount(PlayerManager.I.player.wonMatchesRow, 10);
			progressionText = PlayerManager.I.player.wonMatchesRow+"/10";
		}  else if(name == "Op dreef") {
			fillAmount = getFillAmount(PlayerManager.I.player.wonMatches, 10);
			progressionText = PlayerManager.I.player.wonMatches+"/10";
		}
		progression.transform.GetChild(0).GetComponent<Image>().fillAmount = fillAmount;
		progression.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = progressionText;
	}
	
	private float getFillAmount(float value, float total) {
		return (value / total);
	}
}
