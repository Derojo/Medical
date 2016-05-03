using UnityEngine;
using System.Collections;
using System;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndScreenScript : MonoBehaviour {



    public GameObject lostMatch;
    public GameObject wonMatch;
    public GameObject charAnim;
    public Text matchScore;
	private float percent;
	
    void Awake()
    {
        PlayerManager.I.player.rightAnswersRow = 0;
        PlayerManager.I.CheckLevelUp();
    }

	void Start() {

        // Checking winner
        if(MatchManager.I.winningMatch)
        {
            wonMatch.SetActive(true);
            PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 100;
            charAnim.GetComponent<Animation>().Play("Win_Anim");
            //showBrainCoinTween(4, 100);
        }

        if(!MatchManager.I.winningMatch)
        {
            lostMatch.SetActive(true);
            charAnim.GetComponent<Animation>().Play("Lose_Anim");
        }

        //set match score
        matchScore.text = "Score: " + MatchManager.I.getMatchScore(MatchManager.I.currentMatchID, MatchManager.I.GetOppenentId(null, MatchManager.I.currentMatchID));
	}

	
		

}