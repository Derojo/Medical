using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;
using UnityEngine.SceneManagement;
using LitJson_Gamedonia;

[Prefab("RuntimeData", true, "")]
public class RuntimeData : Singleton<RuntimeData> {

	public QuestionDatabase QuestionDatabase;
	public int test = 0;

//	public List<Question> allQuestions;
	// Use this for initialization

	public bool Load() {return true;}

	void Start () {
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

	public void startRandomMatch() {
		MatchManager.I.StartRandomMatch ();
	}

	// Process incoming notification
	void OnGameUpdateNotification(Dictionary<string,object> notification) {
		Scene currentScene = SceneManager.GetActiveScene();
		Hashtable payload = notification["payload"] != null ? (Hashtable) notification["payload"] : new Hashtable();
		string type = payload.ContainsKey("type") ? payload["type"].ToString() : "";
		string matchID = payload.ContainsKey("notif_id") ? payload["notif_id"].ToString() : "";
		string oppName = payload.ContainsKey("notif_name") ? payload["notif_name"].ToString() : "";
		switch(type) {

		case "matchInvite":
			Debug.Log ("MATCHINVITE");
			
			GamedoniaData.Search ("matches", "{_id: { $oid: '" + matchID +"' } }", delegate (bool invitesuccess, IList data) {
				if (invitesuccess) {
					if (data != null) {
						Dictionary<string, object> matchD = (Dictionary<string, object>)data[0];
						Match match = MatchManager.I.GetMatch(matchID);
						if(match == null) {
							match  = new Match();
							Debug.Log("match created");
							match.m_ID = matchD["_id"].ToString();
							List<string> uids = JsonMapper.ToObject<List<string>> (JsonMapper.ToJson (matchD ["u_ids"]));
							match.u_ids = uids;
							Debug.Log("added user ids");
							List<Turn> turns = new List<Turn> ();
							List<object> t_turns = new List<object> ();
							t_turns = (List<object>)matchD ["m_trns"];
							
							foreach (Dictionary<string, object> t_turn  in t_turns) {
								Turn turn = new Turn (int.Parse (t_turn ["t_ID"].ToString ()), t_turn ["p_ID"].ToString (), int.Parse (t_turn ["q_ID"].ToString ()), int.Parse (t_turn ["c_ID"].ToString ()), int.Parse (t_turn ["t_st"].ToString ()));
								turns.Add (turn);
							}
							match.m_cp = matchD ["m_cp"].ToString ();
							Debug.Log("added current player");
							match.m_trns = turns;
							Debug.Log("added turns");
							match.m_status = matchD ["m_status"].ToString ();
							Debug.Log("added status");
							MatchManager.I.AddMatch(match, false, false);
								Debug.Log("added match");
						}

						if(currentScene.name == "Home") {
							//GameObject.FindObjectOfType<CurrentMatches>().showInvites();
							GameObject.FindObjectOfType<CurrentMatches> ().updateMatches ();							
						}
					} else {
						Debug.Log ("Data is null");
					}
		
				}
			});
			break;
		case "matchTurn":
			Debug.Log ("MATCHTURN");
			//TODO: process the message
			Match match = MatchManager.I.GetMatch (matchID);
			if (match != null) {
				// Update match information
				// Get match from gamedonia server
				GamedoniaData.Search ("matches", "{_id: { $oid: '" + matchID +"' } }", delegate (bool success, IList data) {
					if (success) {
						if (data != null) {
							// *************** Server side match information ********************
							Dictionary<string, object> matchD = (Dictionary<string, object>)data[0];
							List<Turn> turns = new List<Turn>();
							// Conver incoming turn data to Turn class
							List<object> t_turns = new List<object>();
							t_turns = (List<object>)matchD["m_trns"];
							foreach(Dictionary<string, object> t_turn  in t_turns) {
								Turn turn = new Turn(int.Parse(t_turn["t_ID"].ToString()), t_turn["p_ID"].ToString(), int.Parse(t_turn["q_ID"].ToString()), int.Parse(t_turn["c_ID"].ToString()), int.Parse(t_turn["t_st"].ToString()));
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
							match.m_status = matchD["m_status"].ToString();

							if(currentScene.name == "Home") {
								GameObject.FindObjectOfType<CurrentMatches>().updateMatches();
							}
						} else {
							Debug.Log ("Data is null");
						}
					}
				});
			} else {
				Debug.Log ("MATCH DOES NOT EXIST");
			}


			break;
		case "matchFinish":
			Debug.Log ("MATCH FINISH");
			Match finishMatch = MatchManager.I.GetMatch (matchID);

			GamedoniaData.Search ("matches", "{_id: { $oid: '" + matchID +"' } }", delegate (bool success, IList data) {
				if (success) {
					if (data != null) {
						Debug.Log ("MATCH FOUND PROCESS INFORMATION");
						// *************** Server side match information ********************
						Dictionary<string, object> matchD = (Dictionary<string, object>)data[0];
						List<Turn> turns = new List<Turn>();
						// Convert incoming turn data to Turn class
						List<object> t_turns = new List<object>();
						t_turns = (List<object>)matchD["m_trns"];
						foreach(Dictionary<string, object> t_turn  in t_turns) {
							Turn turn = new Turn(int.Parse(t_turn["t_ID"].ToString()), t_turn["p_ID"].ToString(), int.Parse(t_turn["q_ID"].ToString()), int.Parse(t_turn["c_ID"].ToString()), int.Parse(t_turn["t_st"].ToString()));
							turns.Add(turn);
						}
						List<string> uids = JsonMapper.ToObject<List<string>>(JsonMapper.ToJson(matchD["u_ids"]));
						// *************** Update local match ********************
						finishMatch.u_ids = uids;
						finishMatch.m_cc = 0;
						finishMatch.m_trns = turns;
						finishMatch.m_cp = PlayerManager.I.player.playerID;
						finishMatch.m_status = matchD["m_status"].ToString();

						if(currentScene.name == "Home") {
							Debug.Log ("SHOW POPUP");
							Loader.I.showFinishedPopup (oppName, matchD["m_won"].ToString());
						}

					} else {
						Debug.Log ("Data is null");
					}
				}
			});



			break;
		case "matchDeny":
			Debug.Log ("MATCH DENY");
			Debug.Log (matchID);
			GamedoniaData.Delete ("matches", matchID);
			Debug.Log ("DELETED MATCH");
			Match matchDeny = MatchManager.I.GetMatch (matchID);
			MatchManager.I.matches.Remove (matchDeny);
			MatchManager.I.Save ();
			GameObject.FindObjectOfType<CurrentMatches> ().showInvites ();
			GameObject.FindObjectOfType<CurrentMatches> ().deleteRow (matchDeny.m_ID);
			break;
		default:
			// Do nothing
			break;
		}
	}

//	private void ShowFinishedMatchPopup(Match match) {
//		Debug.Log ("Show Popup");
//		string opponentId = MatchManager.I.GetOppenentId (match);
//		GamedoniaUsers.GetUser (opponentId, delegate (bool success, GDUserProfile data) { 
//			if (success) {
//				Dictionary<string, object> oppProfile = new Dictionary<string, object> ();
//				Loader.I.showFinishedPopup (oppProfile ["name"].ToString ());
//			}
//		});
//	}

}
