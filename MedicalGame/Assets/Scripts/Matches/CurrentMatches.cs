using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using Gamedonia.Backend;
using UnityEngine.EventSystems;
using System;


public class CurrentMatches : MonoBehaviour {

	public float hoursForMatch = 0;
	public List<Match> invites;
	public List<Match> yourTurn;
	public List<Match> hisTurn;
	public List<Match> finishedMatches;
	public GameObject currentGamesPanel;
	public GameObject NoGames;
	public GameObject accept;
	public GameObject yourTurnUI;
	public GameObject hisTurnUI;
	public GameObject finishedUI;
	public GameObject lives;
	public Text livesText;

	// Use this for initialization
	void Start () {
		StartLives();
		if (MatchManager.I.matches != null && MatchManager.I.matches.Count > 0) {
			int lives = RuntimeData.I.livesAmount - MatchManager.I.getTotalActiveMatches();
			if(lives  < 0 ) {
				lives = 0;
			}
			livesText.text = lives.ToString();
			StartCoroutine (StartUp ());
		} else {
			StartCoroutine(FirstStartup());

		}
	}
	public void updateLives() {
		if (MatchManager.I.matches != null && MatchManager.I.matches.Count > 0) {
			int lives = RuntimeData.I.livesAmount - MatchManager.I.getTotalActiveMatches();
			if(lives  < 0 ) {
				lives = 0;
			}
			livesText.text = lives.ToString();
		} 
	}
	private void StartLives() {
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(lives.transform.DOScale(1.2f, 0.5f).SetLoops(-1).SetEase (Ease.OutBack));
		mySequence.Append(lives.transform.DOScale(1, 0.2f).SetLoops(-1).SetEase (Ease.OutQuint));
		mySequence.Append(lives.transform.DOScale(1.25f, 0.3f).SetLoops(-1).SetEase (Ease.OutBack));
		mySequence.Append(lives.transform.DOScale(1, 0.2f).SetLoops(-1).SetEase (Ease.OutQuint));
		mySequence.PrependInterval(3);
		mySequence.SetLoops(-1);
	}
	private IEnumerator StartUp() {
		Loader.I.enableLoader ();
		MatchManager.I.checkForUpdateMatches ();
		MatchManager.I.CheckForInvites();
		while(MatchManager.I.checkUpdates == false) {
			yield return null;

		}
		yield return new WaitForSeconds (0.1f);
		updateMatches ();
		Loader.I.disableLoader ();
		Loader.I.CheckInternetConnection();
	}

	private IEnumerator FirstStartup() {
		Loader.I.enableLoader ();
		MatchManager.I.getAllMatchesOnStartGame();
		MatchManager.I.CheckForInvites ();
	
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
		showInvites ();
		showYourTurnGames ();
		showHisTurnGames ();
		showFinishedGames ();
		if (yourTurn.Count == 0 && hisTurn.Count == 0 && invites.Count == 0) {
			NoGames.SetActive(true);
		} else {
			NoGames.SetActive(false);
		}
			int lives = RuntimeData.I.livesAmount - MatchManager.I.getTotalActiveMatches();
			if(lives  < 0 ) {
				lives = 0;
			}
			livesText.text = lives.ToString();	

	}

	public void deleteRow(string name) {

		GameObject row = GameObject.Find (name);
		//		Destroy (GameObject.Find (name));
		foreach (Text text in row.GetComponentsInChildren<Text>()) {
			text.DOFade (0, 0.5f);
		}
		foreach (Image img in row.GetComponentsInChildren<Image>()) {
			img.DOFade (0, 0.5f).OnComplete (new TweenCallback(delegate {
				Destroy(row);
				if(invites.Count == 0) {
					accept.transform.GetChild (0).gameObject.SetActive (false);
				}
			}));
		}
	}
	
	public void checkMatchCount() {
		if (yourTurn.Count == 0 && hisTurn.Count == 0 && invites.Count == 0) {
			NoGames.SetActive(true);
		} else {
			NoGames.SetActive(false);
		}
	}

	public void emptyObjects() {
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);
		}
	}

	public void AddInvite(string matchID) {
		Loader.I.enableLoader ();
		accept.transform.GetChild (0).gameObject.SetActive (true);
		string matchId = matchID;
		Match match = MatchManager.I.GetMatch (matchId);
		string opponentId = MatchManager.I.GetOppenentId (match);
		GameObject matchUI = Instantiate (Resources.Load ("MatchUIInviteRow")) as GameObject;
		setChildInformation (opponentId, matchId, matchUI, "inviteTurn");
		matchUI.name = matchId;
		matchUI.transform.SetParent (accept.transform, false);
		Loader.I.disableLoader ();
	}

	public void showInvites() {
		invites = MatchManager.I.GetPlayingMatches(false, "invite");
		if (invites != null && invites.Count > 0) {
			accept.transform.GetChild(0).gameObject.SetActive (true);
			NoGames.SetActive(false);
		} else {
			accept.transform.GetChild(0).gameObject.SetActive (false);
		}
		for (int i = 0; i < invites.Count; i++) {
			string matchId = invites [i].m_ID;
			if (!GameObject.Find (matchId)) {
				Match match = MatchManager.I.GetMatch (matchId);
				string opponentId = MatchManager.I.GetOppenentId (match);
				GameObject matchUI = null;
				if (match.m_cp != opponentId) {
					matchUI = Instantiate (Resources.Load ("MatchUIInviteRow")) as GameObject;
					matchUI.transform.GetChild (2).GetComponent<Button> ().onClick.AddListener (delegate {
						MatchManager.I.AcceptMatch (match);
					});
					matchUI.transform.GetChild (4).GetComponent<Button> ().onClick.AddListener (delegate {
						MatchManager.I.DenyMatch (match);
					});

				} else {
					matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;
				}
				setChildInformation (opponentId, matchId, matchUI, "inviteTurn");
				matchUI.name = matchId;
				matchUI.transform.SetParent (accept.transform, false);
			}
		}
	}

	public void showYourTurnGames() {
		yourTurn = MatchManager.I.GetPlayingMatches(false, "player");
		float delay = 0;
		/************** YOUR TURN MATCHES *******************/
		// Set turn bar above playing matches
		if (yourTurn.Count != 0) {
			yourTurnUI.transform.GetChild(0).gameObject.SetActive (true);
		} else {
			yourTurnUI.transform.GetChild(0).gameObject.SetActive (false);
		}

		// Iterate through playing matches
		for (int i = 0; i < yourTurn.Count; i++)
		{
			string matchId = yourTurn [i].m_ID;
			if (!yourTurnUI.transform.FindChild (matchId) && !GameObject.Find(matchId) && yourTurn [i].m_status != "deny") {
				Match match = MatchManager.I.GetMatch (matchId);
				string opponentId = MatchManager.I.GetOppenentId (match);
				GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;
				matchUI.SetActive (false);
				// Set all the information, playername, score etc
				setChildInformation (opponentId, matchId, matchUI, "yourTurn", i);
				// Housekeeping
				matchUI.name = matchId;
				matchUI.transform.SetParent (yourTurnUI.transform, false);
				// Set match information


				matchUI.GetComponent<Button> ().onClick.AddListener (delegate {
					LoadMatch (matchId);
				});

				matchUI.GetComponent<RectTransform> ().DOScale (1.05f, .5f).SetEase (Ease.InFlash).SetDelay (delay);
				matchUI.GetComponent<RectTransform> ().DOScale (1, 1f).SetEase (Ease.OutExpo).SetDelay ((.5f + delay));
				delay += .2f;
				if (i != (yourTurn.Count - 1)) {
					matchUI.transform.GetChild (4).gameObject.SetActive (true);
				}
			}  else {
				Match match = MatchManager.I.GetMatch (matchId);
				if(match.m_trns != null) {
					UpdateHoursRemaining(match.m_date, GameObject.Find(matchId),"yourTurn", match);
					if(match.m_cp == PlayerManager.I.player.playerID) {
						if (hisTurnUI.transform.Find (matchId)) {
							GameObject matchUI = hisTurnUI.transform.Find (matchId).gameObject;
							string opponentId = MatchManager.I.GetOppenentId (match);
							matchUI.transform.SetParent (yourTurnUI.transform, false);
							setChildInformation (opponentId, matchId, matchUI, "yourTurn", i);
							matchUI.GetComponent<Button> ().onClick.AddListener (delegate {
								LoadMatch (matchId);
							});
						} else if (accept.transform.Find (matchId)) {
							GameObject matchUI = accept.transform.Find (matchId).gameObject;
							string opponentId = MatchManager.I.GetOppenentId (match);
							matchUI.transform.SetParent (yourTurnUI.transform, false);
							setChildInformation (opponentId, matchId, matchUI, "yourTurn", i);
							matchUI.GetComponent<Button> ().onClick.AddListener (delegate {
								LoadMatch (matchId);
							});
						}
					}
				}
			}
		}
	}

	public void showHisTurnGames() {
		hisTurn = MatchManager.I.GetPlayingMatches(false, "opponent");
		float delay = 0;
		/************** HIS TURN MATCHES *******************/
		// Set turn bar above playing matches
		if (hisTurn.Count != 0) {
			hisTurnUI.transform.GetChild(0).gameObject.SetActive (true);
		} else {
			hisTurnUI.transform.GetChild(0).gameObject.SetActive (false);
		}
		// Iterate through playing matches
		for (int i = 0; i < hisTurn.Count; i++)
		{

			string matchId = hisTurn[i].m_ID;
			if (!hisTurnUI.transform.FindChild (matchId) && !GameObject.Find(matchId) && hisTurn[i].m_status != "deny") {
				Match match = MatchManager.I.GetMatch (matchId);
				string opponentId = MatchManager.I.GetOppenentId (match);
				GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;
				matchUI.SetActive (false);
				setChildInformation (opponentId, matchId, matchUI, "hisTurn", i);
				// Housekeeping
				matchUI.name = matchId;
				matchUI.transform.SetParent (hisTurnUI.transform, false);
				// Set match information
				// matchUI.GetC  = matchId;

				matchUI.GetComponent<RectTransform> ().DOScale (1.05f, .5f).SetEase (Ease.InFlash).SetDelay (delay);
				matchUI.GetComponent<RectTransform> ().DOScale (1, 1f).SetEase (Ease.OutExpo).SetDelay ((.5f + delay));
				delay += .2f;
				if (i != (hisTurn.Count - 1)) {
					matchUI.transform.GetChild (4).gameObject.SetActive (true);
				}
			} else {
				Match match = MatchManager.I.GetMatch (matchId);
				UpdateHoursRemaining(match.m_date, GameObject.Find(matchId),"hisTurn",match);
				if (accept.transform.Find (matchId)) {
					GameObject matchUI = accept.transform.Find (matchId).gameObject;
					string opponentId = MatchManager.I.GetOppenentId (match);
					matchUI.transform.SetParent (hisTurnUI.transform, false);
					setChildInformation (opponentId, matchId, matchUI, "hisTurn", i);
				}
			}
		}
	}

	void showFinishedGames() {
		finishedMatches = MatchManager.I.GetFinishedMatches();
		float delay = 0;
		/************** FINISHED MATCHES *******************/
		// Set turn bar above finished matches
		if (finishedMatches.Count != 0) {
			finishedUI.transform.GetChild(0).gameObject.SetActive (true);
		} else {
			finishedUI.transform.GetChild(0).gameObject.SetActive (false);
		}
		// Iterate through finished matches
		for (int i = 0; i < finishedMatches.Count; i++)
		{
			string matchId = finishedMatches [i].m_ID;
			if (!finishedUI.transform.FindChild (matchId) && !GameObject.Find(matchId)) {
				Match match = MatchManager.I.GetMatch (matchId);
				string opponentId = MatchManager.I.GetOppenentId (match);
				GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;
				matchUI.SetActive (false);
				setChildInformation (opponentId, matchId, matchUI, "finished", i);
				// Housekeeping
				matchUI.name = matchId;
				matchUI.transform.SetParent (this.transform, false);
				// Set match information

				matchUI.GetComponent<RectTransform>().DOScale (1.05f, .5f).SetEase(Ease.InFlash).SetDelay(delay);
				matchUI.GetComponent<RectTransform>().DOScale (1, 1f).SetEase(Ease.OutExpo).SetDelay((.5f+delay));
				delay += .2f;
				if (i != (finishedMatches.Count-1)) {
					matchUI.transform.GetChild (4).gameObject.SetActive (true);
				}
				matchUI.transform.GetChild (5).gameObject.SetActive (true);
				matchUI.transform.GetChild (5).GetComponent<Button> ().onClick.AddListener (delegate {
					MatchManager.I.RemoveMatch(match, "", true);
					deleteRow(matchId);
					if ((finishedMatches.Count-1) == 0) {
						finishedUI.transform.GetChild(0).gameObject.SetActive (false);
					}
				});
			}
		 else {
				Match match = MatchManager.I.GetMatch (matchId);
				if (hisTurnUI.transform.Find (matchId)) {
					GameObject matchUI = hisTurnUI.transform.Find (matchId).gameObject;
					string opponentId = MatchManager.I.GetOppenentId (match);
					matchUI.transform.SetParent (finishedUI.transform, false);
					setChildInformation (opponentId, matchId, matchUI, "finished", i);
					matchUI.transform.GetChild (5).gameObject.SetActive (true);
					matchUI.transform.GetChild (5).GetComponent<Button> ().onClick.AddListener (delegate {
						MatchManager.I.RemoveMatch(match, "", true);
						deleteRow(matchId);
					});
				} else if (yourTurnUI.transform.Find (matchId)) {
					GameObject matchUI = yourTurnUI.transform.Find (matchId).gameObject;
					string opponentId = MatchManager.I.GetOppenentId (match);
					matchUI.transform.SetParent (finishedUI.transform, false);
					setChildInformation (opponentId, matchId, matchUI, "finished", i);
					matchUI.transform.GetChild (5).gameObject.SetActive (true);
					matchUI.transform.GetChild (5).GetComponent<Button> ().onClick.AddListener (delegate {
						MatchManager.I.RemoveMatch(match, "", true);
						deleteRow(matchId);
					});
				}
			}
		}
	}

	private void setChildInformation(string oppId, string matchId, GameObject parent, string listname, int i = 0) {
		if (oppId != "") {
			float timeLeft = 0;
			Match match = MatchManager.I.GetMatch (matchId);
			GamedoniaUsers.GetUser (oppId, delegate (bool success, GDUserProfile data) { 
				if (success) {
					Dictionary<string, object> oppProfile = new Dictionary<string, object> ();
					oppProfile = data.profile;
					foreach (Transform child in parent.transform) {
						if (child.name == "playerName") {
							if (oppId != "") {
								string extraText = "";
								if(match.m_status == "invite" && match.m_cp == oppId) {
									extraText = " (wachten op acceptatie)";
								}
								child.GetComponent<Text> ().text = oppProfile ["name"].ToString ()+extraText;
							}
						}
						if (child.name == "Score") {
							if(listname != "inviteTurn") {
								string _score = MatchManager.I.getMatchScore (matchId, oppId);
								child.GetComponent<Text> ().text = MatchManager.I.getMatchScore (matchId, oppId);
							}
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
						if (child.name == "TimeRemaining") {
							timeLeft = getHoursRemaining(match.m_date); 
							if(listname == "finished") {
								child.GetComponent<Text>().text = Mathf.Abs(timeLeft)+" uur geleden beëindigd";
							} else if(listname == "inviteTurn") {
								child.GetComponent<Text>().text = "nog "+timeLeft+" uur";
							} else if(listname == "yourTurn"){
								child.GetComponent<Text>().text = "nog "+timeLeft+" uur om te reageren";
							} else if(listname == "hisTurn"){
								child.GetComponent<Text>().text = "heeft nog "+timeLeft+" uur om te reageren";
							}
							if(timeLeft <= 0) {
								if(listname == "yourTurn") {
									MatchManager.I.setWinner(match, oppId);
									StartCoroutine(waitBeforeUpdateMatches(1f));
								} else if(listname == "hisTurn") {
									if(oppId != "") {
										MatchManager.I.setWinner(match, PlayerManager.I.player.playerID);
										PlayerManager.I.UnlockNewAttribute (oppId);
										Loader.I.showFinishedPopup (oppProfile ["name"].ToString (), PlayerManager.I.player.playerID);
									} else {
										MatchManager.I.RemoveMatch(match,"",true, true);
										GamedoniaData.Delete("randomqueue",match.m_ID, null);
									}
								} else if(listname == "inviteTurn") {
									MatchManager.I.DenyMatch (match);
								}
							}
						}
						if (child.name == "HourGlass") {
							SetHourGlassImage(timeLeft,child.gameObject);
						}
						
					}
					parent.SetActive (true);
				}
			});
		} else {
			foreach (Transform child in parent.transform) {
				if (child.name == "Score") {
					if(listname != "inviteTurn") {
						string _score = MatchManager.I.getMatchScore (matchId, oppId);
						child.GetComponent<Text> ().text = (_score == "" ? "0-0" : _score);
					}
				}
			}
			parent.SetActive (true);
		}
	}
	
	private IEnumerator waitBeforeUpdateMatches(float time) {
		yield return new WaitForSeconds(time);
		if((yourTurn.Count-1 == 0) ) {
			yourTurnUI.transform.GetChild(0).gameObject.SetActive (false);
		}
		updateMatches();
	}
	
	private void UpdateHoursRemaining(string LastTurnDate, GameObject row, string type, Match match) {
		if(LastTurnDate != "" && match != null) {
			float timeLeft = getHoursRemaining(LastTurnDate);
			string oppId = MatchManager.I.GetOppenentId (match);
			if(timeLeft <= 0) {
				if(type == "yourTurn") {
					MatchManager.I.setWinner(match, oppId);
				} else if(type == "hisTurn") {
					if(oppId != "") {
						MatchManager.I.setWinner(match, PlayerManager.I.player.playerID);
						PlayerManager.I.UnlockNewAttribute (oppId);
						Loader.I.showFinishedPopup ("", PlayerManager.I.player.playerID, oppId);
						
					} else {
						MatchManager.I.RemoveMatch(match,"",true, true);
						GamedoniaData.Delete("randomqueue", match.m_ID, null);
					}

				} else if(type == "inviteTurn") {
					MatchManager.I.DenyMatch (match);
				}
			}
			row.transform.GetChild(3).GetComponent<Text>().text = "nog "+timeLeft+" uur om te reageren";
			
			SetHourGlassImage(timeLeft,row.transform.GetChild(6).gameObject);
		}
	}
	
	private float getHoursRemaining(string LastTurnDate) {
		 float hoursRemaining = 0;
		DateTime LastTurnDateParse = DateTime.ParseExact(LastTurnDate, "yyyy-MM-dd hh:mm:ss:tt", System.Globalization.CultureInfo.InvariantCulture);
		string currentDateString = RuntimeData.I.getCorrectDateTime(DateTime.Now);
		DateTime currentDate = DateTime.ParseExact(currentDateString, "yyyy-MM-dd hh:mm:ss:tt", System.Globalization.CultureInfo.InvariantCulture);
		TimeSpan TimeBetweenTurn = currentDate - LastTurnDateParse;

		if(TimeBetweenTurn.Days > 0) {
			hoursRemaining = (TimeBetweenTurn.Days * 24);
		}
		if(TimeBetweenTurn.Hours > 0) {
			hoursRemaining += TimeBetweenTurn.Hours;
		} else {
			hoursRemaining = (TimeBetweenTurn.Minutes/100);
		}
		hoursRemaining = (hoursForMatch - hoursRemaining);

		return hoursRemaining;
	}
	
	private void SetHourGlassImage(float hoursRemaining, GameObject hourGlass) {
		//75%
		if(hoursRemaining <=  ((hoursForMatch/4)*3)) {
			hourGlass.GetComponent<Text>().text = "\uf252";
		}
		//50 %
		if(hoursRemaining <=  (hoursForMatch/2)) {
			hourGlass.GetComponent<Text>().text = "\uf253";
		}
		if(hoursRemaining <=  0) {
			hourGlass.GetComponent<Text>().text = "\uf250";
		}
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(hourGlass.transform.DOPunchRotation(new Vector3(360f,0f,0f), 2f, 0, 1f));
		mySequence.PrependInterval(6);
		mySequence.SetLoops(-1);

	}

}
