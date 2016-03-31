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
//		player = null;
		if (player == null) {
			player = new Player ();
			Save ();
		}
		CheckCurrentRank ();
		CheckLevelUp ();
		Debug.Log (CurrentRankKey ());
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

	public int CurrentRankKey() {
		int key = 0;
		for (int i = 0; i < ranks.Count; i++) {
			string[] splitScope = ranks [i].levelScope.Split (new string[]{"/"}, System.StringSplitOptions.None);
			int low = int.Parse(splitScope[0]);
			int high = int.Parse(splitScope[1]);
			if(low < player.playerLvl && high > player.playerLvl) {
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
		float PSum = (50 / totalXP) * 1f;

		return PSum;
//		float PSum = (totalXP/100) * player.playerXP;
	}

	public int GetRemainingLevels() {
		// Total experience required in current rank
		string[] splitScope = ranks [CurrentRankKey ()].levelScope.Split (new string[]{"/"}, System.StringSplitOptions.None);

		return int.Parse(splitScope[1]) - player.playerLvl;
		//		float PSum = (totalXP/100) * player.playerXP;
	}

	public string GetNextRankName() {
		return ranks [(CurrentRankKey () + 1)].name;
	}

	public Sprite GetRankSprite() {
		return ranks [CurrentRankKey()].sprite;
	}

	public Sprite GetNextRankSprite() {
		return ranks [(CurrentRankKey () + 1)].sprite;
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
			// Remaining experience;

			player.playerXP = Mathf.Abs (XPSum);
			// Check if we need to level up more then once
			if (player.playerXP > neededXP) {
				CheckLevelUp ();
			} 

		}
	}

	private void OnApplicationQuit() { Save (); }

	private void OnApplicationPause() { Save (); }
}
