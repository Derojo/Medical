using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Gamedonia.Backend;
using System;
using LitJson_Gamedonia;


[Prefab("PlayerManager", true, "")]
public class PlayerManager : Singleton<PlayerManager> {

	// Player ranking
	public List<Rank> ranks = new List<Rank>();

	public Player player;
	public Dictionary<string, object> currentOpponentInfo;
	public Dictionary<string, object> friends;
	public Dictionary<string, object> friendProfiles;
	public bool Load() {return true;}
    public bool lvlUp = false;
    public bool completedIntro = false;

    void Awake() {
		
		LoadPlayer ();
		if (friends == null) {
			friends = new Dictionary<string, object>();
		}
		if (friendProfiles == null) {
			friendProfiles = new Dictionary<string, object>();
		}
//		player = null;
		if (player == null) {
			Debug.Log ("new player");
			player = new Player ();
			Save ();
		}

		CheckCurrentRank ();
		CheckLevelUp ();
	}
		
	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/player.gd");
		bf.Serialize(file, player);
		file.Close();
	}

	public void LoadPlayer() {
		if(File.Exists(Application.persistentDataPath + "/player.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/player.gd", FileMode.Open);

			Player _player = (Player)bf.Deserialize(file);
			if (_player.loggedIn && _player.createdProfile) {
				player = _player;
			}

			file.Close();
		}
			
	}

	public void LoadFriends() {
		GamedoniaUsers.GetMe(delegate (bool success, GDUserProfile data){
			if (success){
				friends = (Dictionary<string, object>)data.profile["friends"];
				foreach (KeyValuePair<string, object> friend in PlayerManager.I.friends) {
					GamedoniaUsers.GetUser (friend.Key, delegate (bool succesFriends, GDUserProfile friendProfile) { 
						if (success) {
							Dictionary<string, object> oppProfile = friendProfile.profile;
							friendProfiles.Add(friend.Key, oppProfile);
						}
					});
				}
			}
		});
	}

	public void changeProfile(PlayerProfile playerprofile) {
		if (player.profile != null) {
			player.profile = playerprofile;
			Save ();
		}

	}

	public void CheckCurrentRank() {
		for (int i = 0; i < ranks.Count; i++) {
			string[] splitScope = ranks [i].levelScope.Split (new string[]{"/"}, System.StringSplitOptions.None);
			int low = int.Parse(splitScope[0]);
			int high = int.Parse(splitScope[1]);

			if(player.playerLvl >= low && player.playerLvl <= high) {
				player.playerRank = ranks [i].name;
			}
		}
	}

	public int CurrentRankKey(int lvl = 0) {
		if (lvl == 0) {
			lvl = player.playerLvl;
		}
		int key = 0;
		for (int i = 0; i < ranks.Count; i++) {
			string[] splitScope = ranks [i].levelScope.Split (new string[]{"/"}, System.StringSplitOptions.None);
			int low = int.Parse(splitScope[0]);
			int high = int.Parse(splitScope[1]);
			if(low < lvl && high > lvl) {
				key = i;
			}
		}
		return key;
	}

	/**
	 * Get experience percentage needed for experience bar
	 * - retrun value is between 0 and 1 where 1 is 100 % and the total of the required experience
	 */
	public float GetExperiencePercentage() {
		// Total experience required in current rank
		float totalXP = ranks [CurrentRankKey ()].reqXP;
		float PSum = (player.playerXP / totalXP) * 1f;

		return PSum;

	}

	public int GetRemainingLevels() {
		// Total experience required in current rank
		string[] splitScope = ranks [CurrentRankKey ()].levelScope.Split (new string[]{"/"}, System.StringSplitOptions.None);

		return int.Parse(splitScope[1]) - player.playerLvl;
	}

	public string GetNextRankName() {
		return ranks [(CurrentRankKey () + 1)].name;
	}

	public Sprite GetRankSprite(int lvl = 0) {
		return ranks [CurrentRankKey(lvl)].sprite;
	}

	public Sprite GetNextRankSprite() {
		return ranks [(CurrentRankKey () + 1)].sprite;
	}

	public void GetPlayerInformationById(string playerID) {;
		GamedoniaUsers.GetUser(playerID, delegate (bool success, GDUserProfile data) { 
			if (success) {
				//returnInformation["name"] = data.profile["name"].ToString();
				currentOpponentInfo = data.profile;
				currentOpponentInfo.Add("_id", data._id);
			}
		});
	}

	public Dictionary<string, object> GetPlayerById(string playerID) {
		Dictionary<string, object> returnprofile = new Dictionary<string, object> ();
		GamedoniaUsers.GetUser(playerID, delegate (bool success, GDUserProfile data) { 
			if (success) {
				returnprofile = data.profile;
			}
		});
		return returnprofile;
	}

	public void CheckLevelUp() {
		float neededXP = ranks [CurrentRankKey ()].reqXP;
		// subtract player experience with needed experience
		 float XPSum = neededXP - player.playerXP;
		// We need to level up
		if (XPSum <= 0) {
			// Add 1 to new player level
			player.playerLvl++;
            //setting lvlUp bool to true for popup
            lvlUp = true;
			// Check new ranking
			CheckCurrentRank();
			// Remaining experience;
			player.playerXP = Mathf.Abs (XPSum);
			// Check if we need to level up more then once
			if (player.playerXP > neededXP)
            {
				CheckLevelUp ();
			} 
		}
	}

	public void AddFriend(string name) {
		friends.Add (name, new List<int> ());
		Dictionary<string, object> profile = GetPlayerById (player.playerID);
		profile ["friends"] = friends;
		GamedoniaUsers.UpdateUser (profile);
	}
		
	public List<int> GetFriendAttributes(string id) {
		List<int> attributesList = new List<int>();
		// Get friend by id from dictionary
		if (friends.Count > 0) {
			Debug.Log ("PlayerID:" + id);
			if (friends.ContainsKey (id)) {
				Debug.Log ("PlayerID EXIST");
				List<int> attributes = JsonMapper.ToObject<List<int>>(JsonMapper.ToJson(friends[id]));

				for (int i = 0; i < attributes.Count; i++) {
					attributesList.Add (Convert.ToInt32 (attributes [i]));
				}
			}
		} 


		return attributesList;
	}

	public void UnlockNewAttribute(string id = "") {

		UpdatePlayerWonAttribute ();
		// Get friend by id from dictionary
		if(id == "") {
			id = currentOpponentInfo["_id"].ToString();
		}
	
		List<int> attributesList = GetFriendAttributes (id);

		if (attributesList.Count != 4) {
			player.playerWonAttr++;
			if (attributesList.Count == 0) {
				attributesList.Add (0);
				// We use this in the end scene to determine which attribute to show
				MatchManager.I.lastAttributeKey = 0;
			} else {
				attributesList.Add (attributesList.Count);
				MatchManager.I.lastAttributeKey = attributesList.Count;
			}

			string attributeName = GetPlayerAttribute (attributesList.Count);
			if (friends.ContainsKey (id)) {
				Dictionary<string, object> updateProfile = new Dictionary<string, object> ();
				friends [id] = attributesList;
				updateProfile ["_id"] = player.playerID;
				updateProfile ["wonAttr"] = player.playerWonAttr;
				updateProfile ["friends"] = friends;
				GamedoniaUsers.UpdateUser (updateProfile, delegate (bool success) {
					if (success) {
						Debug.Log ("Updated user information");
					} 
				});
			}
		} else {
			MatchManager.I.lastAttributeKey = -1;
		}

	}

	private void UpdatePlayerWonAttribute() {
		
	}

	public string GetPlayerAttribute (int key, string id = "") {
		string attribute = "";
		// Get player (opponent, friend)
		if(id == "") {
			attribute = currentOpponentInfo [DetermineWhichAttribute(key)].ToString();
		}
		// Store unlocked attribute state in own player profile

		// Return unlocked attribute
		return attribute;
	}


	public string GetAttributeTitleByKey(int key) {
		string attributeKey = "";
		switch (key)
		{
		case 0:
			attributeKey = "Leeftijd";
			break;
		case 1:
			attributeKey = "Favoriete Kleur";
			break;
		case 2:
			attributeKey = "Favoriete Hobby";
			break;
		case 3:
			attributeKey = "Favoriete Film";
			break;
		}
		return attributeKey;
	}

	private string DetermineWhichAttribute(int key) {
		string attributeKey = "";
		switch (key)
		{
			case 0:
				attributeKey = "age";
				break;
			case 1:
				attributeKey = "color";
				break;
			case 2:
				attributeKey = "hobby";
				break;
			case 3:
				attributeKey = "film";
				break;
		}
		return attributeKey;
	}

	private void OnApplicationQuit() { Save (); }

	private void OnApplicationPause() { Save (); }
}
