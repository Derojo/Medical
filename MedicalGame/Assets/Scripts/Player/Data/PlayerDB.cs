using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class PlayerDB : ScriptableObject {
	[SerializeField]
	public string playerID = "";
	public bool loggedIn = false;
	public bool createdProfile = false;
	public List<PlayerProfile> profile;


	void OnEnable() {
		if (profile == null) {
			profile = new List<PlayerProfile> ();
		}

	}

	public void changeProfile(PlayerProfile playerprofile) {
		profile = new List<PlayerProfile> ();
		profile.Add (playerprofile);
	}

	public void clearAll() {
		playerID = "";	
		loggedIn = false;
		createdProfile = false;
		profile = null;
	}


}