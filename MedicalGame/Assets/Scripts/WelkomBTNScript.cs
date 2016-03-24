using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class WelkomBTNScript : MonoBehaviour {

    public string achievementName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void OnButtonClick()
    {
        AchievementManager.I.EarnAchievement(achievementName);
    }
}
