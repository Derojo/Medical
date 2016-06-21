﻿using UnityEngine;
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

	public GameObject AttributeInfo;
	public GameObject AttributeTitle;
	public GameObject AttributeValue;
    public Animator animControl;
    public GameObject continueButton;

	private string defaultInfoText;


	void Awake()
	{
		PlayerManager.I.player.rightAnswersRow = 0;
		PlayerManager.I.CheckLevelUp();
	}

	void Start() {
		defaultInfoText = AttributeInfo.GetComponent<Text>().text;
		//		string oppId = MatchManager.I.GetOppenentId (null, MatchManager.I.currentMatchID);
		string oppId = PlayerManager.I.currentOpponentInfo["_id"].ToString();
		Debug.Log (oppId);

		//set match score
		matchScore.text = "Score: " + MatchManager.I.getMatchScore(MatchManager.I.currentMatchID, oppId);


		// Checking winner
		if(MatchManager.I.winningMatch)
		{
			wonMatch.SetActive(true);
			PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 100;
            animControl.SetBool("IsWinning", true);
            if (MatchManager.I.lastAttributeKey != -1) {
				StartCoroutine(showContinueButtonOverTime (2f));
				AttributeInfo.GetComponent<Text> ().text = defaultInfoText + " " + PlayerManager.I.currentOpponentInfo ["name"];
				AttributeTitle.GetComponent<Text> ().text = PlayerManager.I.GetAttributeTitleByKey (MatchManager.I.lastAttributeKey);
				AttributeValue.GetComponent<Text> ().text = PlayerManager.I.GetPlayerAttribute (MatchManager.I.lastAttributeKey);

			}
            else
            {
                AchievementManager.I.UltimateMate();
				continueButton.GetComponent<Image> ().DOFade (1, 1f);
				AttributeInfo.SetActive (false);
				AttributeTitle.SetActive (false);
				AttributeValue.SetActive (false);
			}
		}
        else
        {
			lostMatch.SetActive(true);
            continueButton.GetComponent<Image>().DOFade(1, 1f);
            animControl.SetBool("IsLosing", true);
            AttributeInfo.SetActive(false);
            AttributeTitle.GetComponent<Text>().text = "Jammer!";
            AttributeValue.GetComponent<Text>().text = "Helaas, je hebt niet gewonnen en dus geen gegeven vrijgespeeld";

        }

	}

	private IEnumerator showContinueButtonOverTime(float time) {
		yield return new WaitForSeconds (time);
		continueButton.GetComponent<Image> ().DOFade (1, 1f);
	}




}