using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Prefab("PlayerManager", true, "")]
public class PlayerManager : Singleton<PlayerManager> {

	// Player ranking
	public List<Rank> ranks = new List<Rank>();

	public Player player;


	public bool Load() {return true;}


	void Awake() {
		LoadPlayer ();
		if (player == null) {
			player = new Player ();
			Save ();
		}
		CheckCurrentRank ();
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
			player = (Player)bf.Deserialize(file);
			file.Close();
		}


	}

	public void changeProfile(PlayerProfile playerprofile) {
		if (player.profile != null) {
			player.profile = playerprofile;
		}

	}

	public void CheckCurrentRank() {
		for (int i = 0; i < ranks.Count; i++) {
			string[] splitScope = ranks [i].levelScope.Split (new string[]{"/"}, System.StringSplitOptions.None);
			if(int.Parse(splitScope[0]) > player.playerLvl &&  int.Parse(splitScope[1]) < player.playerLvl) {
				player.playerRank = ranks [i].name;
			}
		}
	}

	public int CurrentRankKey() {
		int key = 0;
		for (int i = 0; i < ranks.Count; i++) {
			string[] splitScope = ranks [i].levelScope.Split (new string[]{"/"}, System.StringSplitOptions.None);
			int low = int.Parse(splitScope[0]);
			int high = int.Parse(splitScope[1]);
			if(low > player.playerLvl && high < player.playerLvl) {
				key = i;
			}
		}
		return key;
	}

	public void CheckLevelUp() {
		float neededXP = ranks [CurrentRankKey ()].reqXP;
		// subtract player experience with needed experience
		 float XPSum = neededXP - player.playerXP;
		// We need to level up
		if (XPSum <= 0) {
			// Add 1 to new player level
			player.playerLvl++;
			// Check new ranking
			CheckCurrentRank();
			// Set remaining XP to playerXP
			player.playerXP = Mathf.Abs (XPSum);
		}
	}

	private void OnApplicationQuit() { Save (); }

	private void OnApplicationPause() { Save (); }
}
