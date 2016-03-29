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

	public string returnCurrentRank() {
		for (int i = 0; i < ranks.Count; i++) {
			string[] splitScope = ranks [i].levelScope.Split ("/");
			if(int.Parse(splitScope[0]) > player.playerLvl &&  int.Parse(splitScope[1]) < player.playerLvl) {
				return ranks [i].name;
			}
		}
	}

	public void checkLevelUp() {
	
	}

	private void OnApplicationQuit() { Save (); }

	private void OnApplicationPause() { Save (); }
}
