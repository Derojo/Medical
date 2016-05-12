using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Player
{


	/************** Player general data ******************/
	// Player id from gamedonia
	[SerializeField] public string playerID = "";
	// Facebook user id
	[SerializeField] public string fbuserid = "";
	// Player logged in
	[SerializeField] public bool loggedIn = false;
	// Player created profile
	[SerializeField] public bool createdProfile = false;


	/************** Player game data *********************/
	[SerializeField] public float playerXP = 0;
	[SerializeField] public string playerRank = "";
	[SerializeField] public int playerLvl = 1;
	[SerializeField] public int playedMatches = 0;
	[SerializeField] public int playerWonAttr = 0;
    /************** Player game data without logics *********************/

    // counting TOTAL won matches
    [SerializeField] public int wonMatches = 0;

    // counting won mathes in a ROW
    [SerializeField]public int wonMatchesRow= 0;

    //counting TOTAL active games
    [SerializeField] public int activeGames = 0;

    //counting right answers in a ROW
    [SerializeField] public int rightAnswersRow = 0;

    //counting TOTAl right answers
    [SerializeField] public int rightAnswersTotal = 0;

    //counting TOTAL answers in catagory SPORT
    [SerializeField] public int sportAnswers = 0;

    //counting TOTAL answers in catagory TV & ENTERTAINMENT
    [SerializeField]public int entertainmentAnswers = 0;

    //counting TOTAL answers in catagory HISTORY
    [SerializeField]public int historyAnswers = 0;

    //counting TOTAL answers in catagory GEOGRAPHICS
    [SerializeField]public int geographicAnswers = 0;

    //counting TOTAL answers in catagory CARE
    [SerializeField]public int careAnswers = 0;

    //Counting TOTAL answers in catagory religion
    [SerializeField]public int religionAnswers = 0;

    /************** Player profile data *********************/
    [SerializeField] public PlayerProfile profile;

	public Player
        ( 
             string _playerID = "",
             bool _loggedIn = false, 
             bool _createdProfile = false, 
             float _playerXP = 0, 
             string _playerRank = "",
             int _playerLvl = 1, 
             int _playedMatches = 0, 
             int _wonMatches = 0,
             int _wonMatchesRow = 0,
             int _activeGames = 0,
             int _rightAnswersRow = 0,
             int _rightAnswersTotal = 0,
             int _sportAnswers = 0,
             int _entertainmentAnswers = 0,
             int _historyAnswers = 0,
             int _geographicAnswers = 0,
             int _careAnswers = 0,
             int _religionAnswers = 0,
             PlayerProfile _profile = null
        )
    {
		playerID = _playerID;
		loggedIn = _loggedIn;
		createdProfile = _createdProfile;
		playerXP = _playerXP;	
		playerRank =_playerRank;
		playerLvl = _playerLvl;
		playedMatches = _playedMatches;

        /************** Player profile data without logics *********************/
        wonMatches = _wonMatches;
        wonMatchesRow = _wonMatchesRow;
        activeGames = _activeGames;
        rightAnswersRow = _rightAnswersRow;
        rightAnswersTotal = _rightAnswersTotal;
        sportAnswers = _sportAnswers;
        entertainmentAnswers = _entertainmentAnswers;
        historyAnswers = _historyAnswers;
        geographicAnswers = _geographicAnswers;
        careAnswers = _careAnswers;
        religionAnswers = _religionAnswers;
        if (_profile != null)
        {
			profile = _profile;
		}
	}
}//end class Player