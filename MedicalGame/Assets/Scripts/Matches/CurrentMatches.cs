using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;


public class CurrentMatches : MonoBehaviour {

	public List<Match> yourTurn;
	public List<Match> hisTurn;
	public List<Match> finishedMatches;
	public GameObject currentGamesPanel;

	// Use this for initialization
	void Start () {
		updateMatches ();
	}

	public void LoadMatch(string id) {
		MatchManager.I.LoadCurrentMatch (id);
	}

	public void updateMatches() {
		showYourTurnGames ();
		showHisTurnGames ();
		showFinishedGames ();
		if (yourTurn.Count == 0 && hisTurn.Count == 0) {
			currentGamesPanel.SetActive (false);
		}
	}

	public void showYourTurnGames() {
		yourTurn = MatchManager.I.GetPlayingMatches(true);
		float delay = 0;
		/************** YOUR TURN MATCHES *******************/
		// Set turn bar above playing matches
		if (yourTurn.Count != 0) {
			GameObject yourTurnBar = Instantiate (Resources.Load ("Your_Turn_Bar")) as GameObject;
			yourTurnBar.transform.SetParent (this.transform, false);
		}

		// Iterate through playing matches
		for (int i = 0; i < yourTurn.Count; i++)
		{
			string matchId = yourTurn [i].m_ID;
			Match match = MatchManager.I.GetMatch (matchId);

			GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;

			// Housekeeping
			matchUI.name = matchId;
			matchUI.transform.SetParent (this.transform, false);
			// Set match information
			//			matchUI.GetC  = matchId;
			foreach(Transform child in matchUI.transform) {
				if (child.name == "playerName") {
					child.GetComponent<Text> ().text = matchId;
				}
				if (child.name == "Score") {
					child.GetComponent<Text> ().text = MatchManager.I.getMatchScore (matchId).ToString () + "-0";
				}
				if (child.name == "line") {
					if (yourTurn.Count == 1 || (yourTurn.Count-1) == i) {
						child.gameObject.SetActive (false);
					}
				}
			}

			matchUI.GetComponent<Button> ().onClick.AddListener (delegate {LoadMatch (matchId); });

			matchUI.GetComponent<RectTransform>().DOScale (1.1f, .5f).SetEase(Ease.InFlash).SetDelay(delay);
			matchUI.GetComponent<RectTransform>().DOScale (1, 1f).SetEase(Ease.OutExpo).SetDelay((.5f+delay));
			delay += .2f;
		}
	}

	public void showHisTurnGames() {
		hisTurn = MatchManager.I.GetPlayingMatches(false);
		float delay = 0;
		/************** HIS TURN MATCHES *******************/
		// Set turn bar above playing matches
		if (hisTurn.Count != 0) {
			GameObject hisTurnBar = Instantiate (Resources.Load ("His_Turn_Bar")) as GameObject;
			hisTurnBar.transform.SetParent (this.transform, false);
		}
		// Iterate through playing matches
		for (int i = 0; i < hisTurn.Count; i++)
		{
			string matchId = hisTurn[i].m_ID;
			Debug.Log (matchId);
			Match match = MatchManager.I.GetMatch (matchId);

			GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;

			// Housekeeping
			matchUI.name = matchId;
			matchUI.transform.SetParent (this.transform, false);
			// Set match information
			//			matchUI.GetC  = matchId;
			foreach(Transform child in matchUI.transform) {
				if (child.name == "playerName") {
					child.GetComponent<Text> ().text = matchId;
				}
				if (child.name == "Score") {
					child.GetComponent<Text> ().text = MatchManager.I.getMatchScore (matchId).ToString () + "-0";
				}
				if (child.name == "line") {
					if (hisTurn.Count == 1 || (hisTurn.Count-1) == i) {
						child.gameObject.SetActive (false);
					}
				}
			}

			matchUI.GetComponent<RectTransform>().DOScale (1.1f, .5f).SetEase(Ease.InFlash).SetDelay(delay);
			matchUI.GetComponent<RectTransform>().DOScale (1, 1f).SetEase(Ease.OutExpo).SetDelay((.5f+delay));
			delay += .2f;
		}
	}

	void showFinishedGames() {
		finishedMatches = MatchManager.I.GetFinishedMatches();
		float delay = 0;
		/************** FINISHED MATCHES *******************/
		// Set turn bar above finished matches
		if (finishedMatches.Count != 0) {
			GameObject finishedBar = Instantiate (Resources.Load ("Finished_Bar")) as GameObject;
			finishedBar.transform.SetParent (this.transform, false);
		}

		// Iterate through finished matches
		for (int i = 0; i < finishedMatches.Count; i++)
		{
			string matchId = finishedMatches [i].m_ID;
			Match match = MatchManager.I.GetMatch (matchId);
			GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;

			// Housekeeping
			matchUI.name = matchId;
			matchUI.transform.SetParent (this.transform, false);
			// Set match information
			//			matchUI.GetC  = matchId;
			foreach(Transform child in matchUI.transform) {
				if (child.name == "playerName") {
					child.GetComponent<Text> ().text = matchId;
				}
				if (child.name == "Score") {
					child.GetComponent<Text> ().text = MatchManager.I.getMatchScore (matchId).ToString () + "-0";
				}
				if (child.name == "line") {
					if (finishedMatches.Count == 1 || (finishedMatches.Count-1) == i) {
						child.gameObject.SetActive (false);
					}
				}
			}

			matchUI.GetComponent<RectTransform>().DOScale (1.15f, .5f).SetEase(Ease.InFlash).SetDelay(delay);
			matchUI.GetComponent<RectTransform>().DOScale (1, 1f).SetEase(Ease.OutExpo).SetDelay((.5f+delay));
			delay += .2f;
		}
	}

}
