using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Gamedonia.Backend;
using UnityEngine.EventSystems;


public class CurrentMatches : MonoBehaviour {

	public List<Match> yourTurn;
	public List<Match> hisTurn;
	public List<Match> finishedMatches;
	public GameObject currentGamesPanel;

	// Use this for initialization
	void Start () {
		if (MatchManager.I.matches != null && MatchManager.I.matches.Count > 0) {
			StartCoroutine (StartUp ());
		}
	}

	private IEnumerator StartUp() {
		Loader.I.enableLoader ();
		MatchManager.I.checkForUpdateMatches ();
		while(MatchManager.I.checkUpdates == false) {
			yield return null;
		}
		yield return new WaitForSeconds (0.1f);
		updateMatches ();
		Loader.I.disableLoader ();
	}

	public void LoadMatch(string id) {
		MatchManager.I.LoadCurrentMatch (id);
	}

	public void updateMatches() {
		emptyObjects ();
		showYourTurnGames ();
		showHisTurnGames ();
		showFinishedGames ();
		if (yourTurn.Count == 0 && hisTurn.Count == 0) {
			currentGamesPanel.SetActive (false);
		}
	}

	public void emptyObjects() {
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
	}
	public void showYourTurnGames() {
		yourTurn = MatchManager.I.GetPlayingMatches(false, true);
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
			string opponentId = MatchManager.I.GetOppenentId (match);
			GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;
			matchUI.SetActive (false);
			// Set all the information, playername, score etc
			setChildInformation (opponentId, matchId, matchUI, "yourTurn", i);

			// Housekeeping
			matchUI.name = matchId;
			matchUI.transform.SetParent (this.transform, false);
			// Set match information
			//			matchUI.GetC  = matchId;


			matchUI.GetComponent<Button> ().onClick.AddListener (delegate {LoadMatch (matchId); });

			matchUI.GetComponent<RectTransform>().DOScale (1.1f, .5f).SetEase(Ease.InFlash).SetDelay(delay);
			matchUI.GetComponent<RectTransform>().DOScale (1, 1f).SetEase(Ease.OutExpo).SetDelay((.5f+delay));
			delay += .2f;
			if (i != (yourTurn.Count-1)) {
				matchUI.transform.GetChild (3).gameObject.SetActive (true);
			}
		}
	}

	public void showHisTurnGames() {
		hisTurn = MatchManager.I.GetPlayingMatches(false, false);
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
			Match match = MatchManager.I.GetMatch (matchId);
			string opponentId = MatchManager.I.GetOppenentId (match);
			GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;
			matchUI.SetActive (false);
			setChildInformation (opponentId, matchId, matchUI, "hisTurn", i);
			// Housekeeping
			matchUI.name = matchId;
			matchUI.transform.SetParent (this.transform, false);
			// Set match information
			//			matchUI.GetC  = matchId;
			

			matchUI.GetComponent<RectTransform>().DOScale (1.1f, .5f).SetEase(Ease.InFlash).SetDelay(delay);
			matchUI.GetComponent<RectTransform>().DOScale (1, 1f).SetEase(Ease.OutExpo).SetDelay((.5f+delay));
			delay += .2f;
			if (i != (hisTurn.Count-1)) {
				matchUI.transform.GetChild (3).gameObject.SetActive (true);
			}
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
			string opponentId = MatchManager.I.GetOppenentId (match);
			GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;
			matchUI.SetActive (false);
			setChildInformation (opponentId, matchId, matchUI, "finished", i);
			// Housekeeping
			matchUI.name = matchId;
			matchUI.transform.SetParent (this.transform, false);
			// Set match information
			//			matchUI.GetC  = matchId;


			matchUI.GetComponent<RectTransform>().DOScale (1.1f, .5f).SetEase(Ease.InFlash).SetDelay(delay);
			matchUI.GetComponent<RectTransform>().DOScale (1, 1f).SetEase(Ease.OutExpo).SetDelay((.5f+delay));
			delay += .2f;
			if (i != (finishedMatches.Count-1)) {
				matchUI.transform.GetChild (3).gameObject.SetActive (true);
			}
		}
	}

	private void setChildInformation(string oppId, string matchId, GameObject parent, string listname, int i) {
		if (oppId != "") {
			GamedoniaUsers.GetUser (oppId, delegate (bool success, GDUserProfile data) { 
				if (success) {
					Dictionary<string, object> oppProfile = new Dictionary<string, object> ();
					oppProfile = data.profile;
					foreach (Transform child in parent.transform) {
						if (child.name == "playerName") {
							if (oppId != "") {
								child.GetComponent<Text> ().text = oppProfile ["name"].ToString ();
							}
						}
						if (child.name == "Score") {
							child.GetComponent<Text> ().text = MatchManager.I.getMatchScore (matchId, oppId);
						}
						if (child.name == "line") {
							if (listname == "yourTurn") {
								if (yourTurn.Count == 1 || (yourTurn.Count - 1) == i) {
									child.gameObject.SetActive (false);
								}
							} else if (listname == "hisTurn") {
								if (hisTurn.Count == 1 || (hisTurn.Count - 1) == i) {
									child.gameObject.SetActive (false);
								}
							} else if (listname == "finished") {
								if (finishedMatches.Count == 1 || (finishedMatches.Count - 1) == i) {
									child.gameObject.SetActive (false);
								}
							}
						}
						if (child.name == "rankImg") {
							if (oppId != "") {
								child.GetComponent<Image> ().sprite = PlayerManager.I.GetRankSprite (int.Parse (oppProfile ["lvl"].ToString ()));
							}
						}
					}
					parent.SetActive (true);
				}
			});
		} else {
			foreach (Transform child in parent.transform) {
				if (child.name == "Score") {
					string _score = MatchManager.I.getMatchScore (matchId, oppId);
					child.GetComponent<Text> ().text = (_score == "" ? "0-0" : _score);
				}
			}
			parent.SetActive (true);
		}
	}

}
