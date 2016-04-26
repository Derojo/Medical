using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class HomeStartUp : MonoBehaviour {

    public Image rankingImg;
    public Text currentRanktxt;
    public Text currentLVL;

    // Use this for initialization
    void Start ()
    {
        //Earning first achievement
        AchievementManager.I.checkAchievementConnect();
        //Getting rank image
        rankingImg.sprite = PlayerManager.I.GetRankSprite();
        //Getting rank name
        currentRanktxt.text = PlayerManager.I.player.playerRank;
        currentLVL.text = "Je bent nu level " + PlayerManager.I.player.playerLvl.ToString() + " van de rank ";
    }
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
