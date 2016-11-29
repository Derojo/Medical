using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;
using UnityEngine.SceneManagement;
using LitJson_Gamedonia;
using System;

[Prefab("RuntimeData", true, "")]
public class RuntimeData : Singleton<RuntimeData> {

	public int livesAmount = 0;
	public bool noconnectionSceneLoaded = false;
	private bool pressedReturnButton = false;
	// Use this for initialization

	public bool Load() {return true;}
	
	void Update() {
		Scene currentScene = SceneManager.GetActiveScene();
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				pressedReturnButton = false;
				if(PlayerManager.I.player.loggedIn) {
					if(!pressedReturnButton) {
						pressedReturnButton = true;
						if(currentScene.name == "Home") {
							Application.Quit();
							return;
						} else {
							Loader.I.enableLoader();
							Loader.I.LoadScene("Home");
						}
					}
				} else {
					Application.Quit();
					return;
				}
			}
		}
	}
	
	void Start () {
        //setting framerate
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        // Load Managers
        Loader.I.Load();
		MatchManager.I.Load ();
		PlayerManager.I.Load ();
        AchievementManager.I.Load();
		// Push notifications
		GDPushService service = new GDPushService();
		service.RegisterEvent += new RegisterEventHandler(OnGameUpdateNotification);
		GamedoniaPushNotifications.AddService (service);
	}

	
	/********************************************************PUSH NOTIFICATIONS AREA ****************************************************************************/
	// Process incoming notification
	void OnGameUpdateNotification(Dictionary<string,object> notification) {
		Scene currentScene = SceneManager.GetActiveScene();
		Hashtable payload = notification["payload"] != null ? (Hashtable) notification["payload"] : new Hashtable();
		string type = payload.ContainsKey("type") ? payload["type"].ToString() : "";
		string matchID = payload.ContainsKey("notif_id") ? payload["notif_id"].ToString() : "";
		string oppName = payload.ContainsKey("notif_name") ? payload["notif_name"].ToString() : "";
		switch(type) {

		case "matchInvite":	
			GamedoniaData.Search ("matches", "{_id: { $oid: '" + matchID +"' } }", delegate (bool invitesuccess, IList data) {
				if (invitesuccess) {
					if (data != null) {
						Dictionary<string, object> matchD = (Dictionary<string, object>)data[0];
						Match matchInvite = MatchManager.I.GetMatch(matchID);
						if(matchInvite == null) {
                            matchInvite = new Match();
                            matchInvite.m_ID = matchD["_id"].ToString();
							List<string> uids = JsonMapper.ToObject<List<string>> (JsonMapper.ToJson (matchD ["u_ids"]));
                            matchInvite.u_ids = uids;
							List<Turn> turns = new List<Turn> ();
							List<object> t_turns = new List<object> ();
							t_turns = (List<object>)matchD ["m_trns"];
							
							foreach (Dictionary<string, object> t_turn  in t_turns) {
								Turn turn = new Turn (int.Parse (t_turn ["t_ID"].ToString ()), t_turn ["p_ID"].ToString (), t_turn ["q_ID"].ToString (), int.Parse (t_turn ["c_ID"].ToString ()), int.Parse (t_turn ["t_st"].ToString ()));
								turns.Add (turn);
							}
                            matchInvite.m_cp = matchD ["m_cp"].ToString ();
                            matchInvite.m_trns = turns;
                            matchInvite.m_date = matchD ["m_date"].ToString();
                            matchInvite.m_status = matchD ["m_status"].ToString ();
							MatchManager.I.AddMatch(matchInvite, false, false);
						}

						if(currentScene.name == "Home") {
							//GameObject.FindObjectOfType<CurrentMatches>().showInvites();
							GameObject.FindObjectOfType<CurrentMatches> ().updateMatches ();	
							GameObject.FindObjectOfType<CurrentMatches> ().updateLives ();								
						}
					} 
				}
			});
			break;
		case "matchTurn":
            Debug.Log("matchTurn");
			//TODO: process the message
			Match match = MatchManager.I.GetMatch (matchID);
			if (match != null) {
                    Debug.Log("search for matches");
                    // Update match information
                    // Get match from gamedonia server
                    GamedoniaData.Search ("matches", "{_id: { $oid: '" + matchID +"' } }", delegate (bool success, IList data) {
					if (success) {
						if (data != null) {
                            Debug.Log("Found match");
							// *************** Server side match information ********************
							Dictionary<string, object> matchD = (Dictionary<string, object>)data[0];
							List<Turn> turns = new List<Turn>();
							// Conver incoming turn data to Turn class
							List<object> t_turns = new List<object>();
							t_turns = (List<object>)matchD["m_trns"];
							foreach(Dictionary<string, object> t_turn  in t_turns) {
								Turn turn = new Turn(int.Parse(t_turn["t_ID"].ToString()), t_turn["p_ID"].ToString(), t_turn["q_ID"].ToString(), int.Parse(t_turn["c_ID"].ToString()), int.Parse(t_turn["t_st"].ToString()));
								turns.Add(turn);
							}
							List<string> uids = JsonMapper.ToObject<List<string>>(JsonMapper.ToJson(matchD["u_ids"]));
							// Add friend to list
							string oppId = MatchManager.I.GetOppenentId(match);
							if(!PlayerManager.I.friends.ContainsKey(oppId) && oppId!= "") {
								PlayerManager.I.AddFriend(oppId);
							}
							// *************** Update local match ********************
							match.u_ids = uids;
							match.m_cc = 0;
							match.m_trns = turns;
							match.m_cp = PlayerManager.I.player.playerID;
							match.m_date = matchD["m_date"].ToString();
							match.m_status = matchD["m_status"].ToString();

							if(currentScene.name == "Home") {
								GameObject.FindObjectOfType<CurrentMatches>().updateMatches();
							}
						} else {
							
						}
					}
				});
			} 

			break;
		case "matchFinish":
			Match finishMatch = MatchManager.I.GetMatch (matchID);

			GamedoniaData.Search ("matches", "{_id: { $oid: '" + matchID +"' } }", delegate (bool success, IList data) {
				if (success) {
					if (data != null) {
				
						// *************** Server side match information ********************
						Dictionary<string, object> matchD = (Dictionary<string, object>)data[0];
						List<Turn> turns = new List<Turn>();
						// Convert incoming turn data to Turn class
						List<object> t_turns = new List<object>();
						t_turns = (List<object>)matchD["m_trns"];
						foreach(Dictionary<string, object> t_turn  in t_turns) {
							Turn turn = new Turn(int.Parse(t_turn["t_ID"].ToString()), t_turn["p_ID"].ToString(), t_turn["q_ID"].ToString(), int.Parse(t_turn["c_ID"].ToString()), int.Parse(t_turn["t_st"].ToString()));
							turns.Add(turn);
						}
						List<string> uids = JsonMapper.ToObject<List<string>>(JsonMapper.ToJson(matchD["u_ids"]));
						// *************** Update local match ********************
						finishMatch.u_ids = uids;
						finishMatch.m_cc = 0;
						finishMatch.m_trns = turns;
						finishMatch.m_cp = PlayerManager.I.player.playerID;
						finishMatch.m_date = matchD ["m_date"].ToString();
						finishMatch.m_status = matchD["m_status"].ToString();
						if(matchD["m_won"].ToString() == PlayerManager.I.player.playerID) {
							// Check if we can earn an attribute of our opponent
							PlayerManager.I.UnlockNewAttribute (MatchManager.I.GetOppenentId (finishMatch));
						
						}
						if(currentScene.name == "Home") {
							Loader.I.showFinishedPopup (oppName, matchD["m_won"].ToString());
						}
						GameObject.FindObjectOfType<CurrentMatches> ().updateLives();

					} 
				}
			});



			break;
		case "matchDeny":
			GamedoniaData.Delete ("matches", matchID);
			Match matchDeny = MatchManager.I.GetMatch (matchID);
			MatchManager.I.matches.Remove (matchDeny);
			MatchManager.I.Save ();
			GameObject.FindObjectOfType<CurrentMatches> ().showInvites ();
			GameObject.FindObjectOfType<CurrentMatches> ().deleteRow (matchDeny.m_ID);
			if(currentScene.name == "Home") {
				GameObject.FindObjectOfType<CurrentMatches> ().updateLives ();
			}
			break;
		default:
			// Do nothing
			break;
		}
	}
	/********************************************************PUSH NOTIFICATIONS AREA END ****************************************************************************/
	
	public void startRandomMatch() {
		int livesLeft = RuntimeData.I.livesAmount - MatchManager.I.getTotalActiveMatches();
		if(livesLeft > 0) {
			MatchManager.I.StartRandomMatch ();
		} else {
			// Show response
			Loader.I.showLivesPopup();
		}
	}
	
	public string getCorrectDateTime(DateTime date) {
		System.DateTime theTime = date;
		
		string returnValue = theTime.ToString("yyyy-MM-dd hh:mm:ss:tt", System.Globalization.CultureInfo.InvariantCulture);
		return returnValue;
	}
	
	public void LogOut() {
		PlayerManager.I.loggingOut();
	}
}
