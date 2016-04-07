using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Prefab("MatchManager", true, "")]
public class MatchManager : Singleton<MatchManager> {

	public Matches matchManager;
//	public static MatchManager mInstance = null;

	// Used for debugging, test purposes
	public List<Match> matches;
	public string currentMatchID;
	public int currentCategory;
	public Loader loader;


	public bool Load() {return true;}

	void Awake() {
			


//		matchManager = null;
		if (matchManager == null) {
			matchManager = new Matches ();
		}
		LoadMatches ();
		matches = new List<Match> (); 
		matches = matchManager.matches;
	}


	public void StartNewMatch() {
		Loader.Instance.enableLoader ();
		// Generate Match code
		string matchCode = GenerateMatchCode();
		// Store for later use
		currentMatchID = matchCode;
//		// Create match, set player ids, category id  !----- TO DO : Create auto match with gamedonia db -----!
		Match match  = new Match(matchCode, PlayerManager.I.player.playerID, "56ea94f2e4b027e49c1ef3e1", "playing", 1, PlayerManager.I.player.playerID, 0, null);
		AddMatch (match);
//		// Switch to category scene
		Loader.Instance.LoadScene("Category");
	}

	public void EndMatch() {
		// Set status to finished
		GetMatch(currentMatchID).m_status = "finished";
	}

	public void LoadCurrentMatch(string id) {
		Loader.Instance.enableLoader ();
		currentMatchID = id;
		Loader.Instance.LoadScene("Category");
	}
		
	private void AddMatch(Match match) {
		matchManager.matches.Add (match);
		Save ();
	}

	public void AddTurn(Turn turn, string match_ID = "") {
		if (match_ID == "") {
			match_ID = currentMatchID;
		}
		Match match = GetMatch (match_ID);

		match.AddTurn (turn);
		if (!turn.t_st) {
			match.m_cp = match.o_ID;
		} else {
			match.m_cp = match.p_ID;
		}
		Save ();
	}

	public void ChangeTurn(Turn turn) {
		Match match = GetMatch (currentMatchID);
		Debug.Log ((turn.t_ID - 1));
		match.m_trns [(turn.t_ID - 1)] = turn;
		Save ();
	}
	public void clearCurrentCategory() {
		Match match = GetMatch (currentMatchID);
		match.m_cc = 0;
		Save ();
	}

	public Match GetMatch(string match_ID) {
		for (int i = 0; i < matchManager.matches.Count; i++) {
			if (matchManager.matches[i].m_ID == match_ID) {
				return matchManager.matches [i];
			}
		}
		return null;
	}

	public List<Match> GetPlayingMatches(bool player) {
		List<Match> tempList = new List<Match> ();
		string pID = PlayerManager.I.player.playerID;
		for (int i = 0; i < matchManager.matches.Count; i++) {
			if (matchManager.matches[i].m_status == "playing") {
				if (player) {
					if (matchManager.matches [i].m_cp == pID) {
						tempList.Add (matchManager.matches [i]);
					}
				} else {
					
					if (matchManager.matches [i].m_cp != pID) {
						tempList.Add (matchManager.matches [i]);
					}
				}

			}
		}
		return tempList;
	}

	public List<Match> GetFinishedMatches() {
		List<Match> tempList = new List<Match> ();
		for (int i = 0; i < matchManager.matches.Count; i++) {
			if (matchManager.matches[i].m_status == "finished") {
				tempList.Add(matchManager.matches[i]);
			}
		}
		return tempList;
	}

	public int getMatchScore(string id) {
		int total = 0;
		Match match = GetMatch (id);
		if (match.m_trns != null) {
			for (int i = 0; i < match.m_trns.Count; i++) {
				if (match.m_trns [i].t_st) {
					total++;
				}
			}
		}
		return total;
	}

	public int returnTurnId(string id = "") {
		if (id == "") {
			id = currentMatchID;
		}
		Match match = GetMatch (id);
		if (match.m_trns == null) {
			return 0;
		} else {
			return match.m_trns.Count;
		}
	}

	public List<int> GetQuestionsInMatch() {
		List<int> returnValue = new List<int> ();
		List<Turn> turns = GetMatch (currentMatchID).m_trns;
		if (turns != null) {
			for (int i = 0; i < turns.Count; i++) {
				if (turns [i].p_ID == PlayerManager.I.player.playerID) {
					returnValue.Add (turns [i].q_ID);
				}
			}
		}
		return returnValue;
	}

	public List<Match> returnAllMatches() {
		return matches;
	}
		
	private void clearAllMatches() {
		matchManager.matches = new List<Match> ();
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();

//		var unityJson = JsonUtility.ToJson(this);
//		FileStream file = File.WriteAllText (Application.persistentDataPath + "/matches.json", unityJson);
		FileStream file = File.Create(Application.persistentDataPath + "/matches.gd");
		bf.Serialize(file, matchManager.matches);


		file.Close();

//		var unityJson = JsonUtility.ToJson(this);
//		File.WriteAllText (Application.persistentDataPath + "/matches.json", unityJson);
	}

	public void LoadMatches() {
		if(File.Exists(Application.persistentDataPath + "/matches.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/matches.gd", FileMode.Open);
			matchManager.matches = (List<Match>)bf.Deserialize(file);
			file.Close();
		}
//		if(File.Exists(Application.persistentDataPath + "/matches.json")) {
//			string jsonString = File.ReadAllText (Application.persistentDataPath + "/matches.json");
//
//			CreateFromJSON (jsonString);
//
//		}
	}


	private string GenerateMatchCode() {
		int desiredCodeLength = 15;
		string code = "";

		char[] characters =  "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
		while(code.Length < desiredCodeLength) {
			code += characters[Random.Range(0, characters.Length)];
		}
		if (!checkCode(code)) {
			return(GenerateMatchCode ());
		} else {
			return code;
		}
	}

	private bool checkCode(string code) {
		for (int i = 0; i < matchManager.matches.Count; i++) {
			if (matchManager.matches[i].m_ID == code) {
				return false;
			}
		}
		return true;
	}
		
}