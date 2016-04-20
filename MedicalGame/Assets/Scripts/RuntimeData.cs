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
		Debug.Log (System.DateTime.Now);
		Hashtable payload = notification["payload"] != null ? (Hashtable) notification["payload"] : new Hashtable();
		string type = payload.ContainsKey("type") ? payload["type"].ToString() : "";
		string matchID = payload.ContainsKey("notif_id") ? payload["notif_id"].ToString() : "";
		switch(type) {
		case "matchStart":
			//TODO: process the message
			break;
		case "matchTurn":
			//TODO: process the message

			Match match = MatchManager.I.GetMatch (matchID);
			if (match != null) {
				// Update match information
				Debug.Log ("MATCH DOES EXIST");
				// Get match from gamedonia server
				GamedoniaData.Search ("matches", "{_id: { $oid: '" + matchID +"' } }", delegate (bool success, IList data) {
					Debug.Log ("SEARCHING FOR MATCH");
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
							// *************** Update local match ********************
							match.u_ids = uids;
							match.m_cc = 0;
							match.m_trns = turns;
							match.m_cp = PlayerManager.I.player.playerID;
							Scene currentScene = SceneManager.GetActiveScene();
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
			//TODO: process the message
			break;
		case "matchEnd":
			//TODO: process the message
			break;
		default:
			// Do nothing
			break;
		}
	}

}
