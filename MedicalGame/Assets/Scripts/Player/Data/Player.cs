using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player
{


	/************** Player general data ******************/
	// Player id from gamedonia
	[SerializeField] public string playerID = "";
	// Player logged in
	[SerializeField] public bool loggedIn = false;
	// Player created profile
	[SerializeField] public bool createdProfile = false;


	/************** Player game data *********************/
	[SerializeField] public float playerXP = 0;
	[SerializeField] public string playerRank = "";
	[SerializeField] public int playerLvl = 1;
	[SerializeField] public float playedMatches = 0;


	/************** Player profile data *********************/
	[SerializeField] public PlayerProfile profile;

	public Player( string _playerID = "", bool _loggedIn = false, bool _createdProfile = false, float _playerXP = 0, string _playerRank = "", int _playerLvl = 1, float _playedMatches = 0,  PlayerProfile _profile = null) {
		playerID = _playerID;
		loggedIn = _loggedIn;
		createdProfile = _createdProfile;
		playerXP = _playerXP;	
		playerRank =_playerRank;
		playerLvl = _playerLvl;
		playedMatches = _playedMatches;
		if (_profile != null) {
			profile = _profile;
		}
	}
}