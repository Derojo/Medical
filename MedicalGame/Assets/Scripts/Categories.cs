using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Categories : MonoBehaviour {

	public enum categories
	{
        GGZ            	 = 1,
        OUDERENZORG      = 2,
        ZIEKENHUISZORG   = 3,
        ZORG_ALGEMEEN    = 4,
        TAND_HUISARTS    = 5,
        GEHANDICAPTEN    = 6
	}

	public Text catTitle;                    
	public GameObject GGZ_a;
	public GameObject OUDERENZORG_a;
	public GameObject ZIEKENHUISZORG_a;
	public GameObject TAND_HUISARTS_a;
	public GameObject ZORG_ALGEMEEN_a;
	public GameObject GEHANDICAPTEN_a;
	

	private List<Turn> playerTurns;
	private List<Turn> oppTurns;

	// Use this for initialization
	void Start () {
		// CHECK FOR ACHIEVEMENTS
        if (PlayerPrefs.GetInt("Populair") == 0)
        {
            AchievementManager.I.PopulairPlayer();
        }
        // check achievement
        Debug.Log(PlayerPrefs.GetInt("Vrienden worden?"));
        Debug.Log(PlayerManager.I.firstInvite);
        // check for achievement
        if (PlayerManager.I.firstInvite)
        {
            AchievementManager.I.wantToBeFriends();
            PlayerManager.I.firstInvite = false;
        }

        if (PlayerManager.I.firstInviteAccept)
        {
        
            AchievementManager.I.challengeAccepted();
            PlayerManager.I.firstInviteAccept = false;
        }
		// CHECK FOR ACHIEVEMENTS END
		
		// Get current match
		Match currentMatch = MatchManager.I.GetMatch (MatchManager.I.currentMatchID);
		playerTurns = MatchManager.I.GetMatchTurnsByPlayerID (PlayerManager.I.player.playerID, currentMatch);
		oppTurns = MatchManager.I.GetMatchTurnsByPlayerID (MatchManager.I.GetOppenentId(currentMatch), currentMatch);
		//Get random cat, show animation
		if (currentMatch.m_cc != 0) {
			MatchManager.I.currentCategory = currentMatch.m_cc;
		} else {
			if (currentMatch.m_trns != null && currentMatch.m_trns.Count > 0) {
				if (oppTurns.Count > playerTurns.Count) {
					// Opponent played more turns, get the next question
//					int pt  = (playerTurns.Count == 0 ? 1 : playerTurns.Count);
					for (int i = 0; i < oppTurns.Count; i++) {
						if (oppTurns [i].t_ID == playerTurns.Count + 1) {
							MatchManager.I.currentCategory = oppTurns [i].c_ID;
						}
					}
//					int pt = ((oppTurns.Count - playerTurns.Count) == oppTurns.Count ? oppTurns.Count : (playerTurns.Count+1));
//					MatchManager.I.currentCategory = oppTurns [(oppTurns.Count - pt)].c_ID; // Last category played by opponent.
				}	else {
					MatchManager.I.currentCategory = (int)GetRandomCat ();
				}
			} else {
				MatchManager.I.currentCategory = (int)GetRandomCat ();
			}
		}

		currentMatch.m_cc = MatchManager.I.currentCategory;
		ShowCategory (MatchManager.I.currentCategory);
	}

	public void ShowCategory(int catID) {
		switch (catID) {
            case (int)categories.GGZ:
                catTitle.text = "GGZ & Verslavingszorg";
				GGZ_a.SetActive (true);
                break;
            case (int)categories.OUDERENZORG:
                catTitle.text = "Ouderenzorg";
				OUDERENZORG_a.SetActive (true);
                break;
            case (int)categories.ZIEKENHUISZORG:
                catTitle.text = "Ziekenhuiszorg";
				ZIEKENHUISZORG_a.SetActive (true);
                break;
            case (int)categories.ZORG_ALGEMEEN:
                catTitle.text = "Algemeen";
				ZORG_ALGEMEEN_a.SetActive (true);
                break;
            case (int)categories.TAND_HUISARTS:
                catTitle.text = "Tandarts & Huisarts";
				TAND_HUISARTS_a.SetActive (true);
                break;
            case (int)categories.GEHANDICAPTEN:
                 catTitle.text = "Gehandicaptenzorg";
				GEHANDICAPTEN_a.SetActive (true);
                 break;
        }
    }

	public static string getCategoryNameById(int catID) {
		switch (catID) {
            case (int)categories.ZIEKENHUISZORG:
                return "Ziekenhuiszorg";
            case (int)categories.TAND_HUISARTS:
                return "Tandarts & Huisarts";
            case (int)categories.GEHANDICAPTEN:
                return "Gehandicaptenzorg";
            case (int)categories.OUDERENZORG:
                return "Ouderenzorg";
            case (int)categories.ZORG_ALGEMEEN:
                return "Algemeen";
            case (int)categories.GGZ:
                return "GGZ"; 

        }
        return "";
	}

	public void PlayMatch() {
		Loader.I.LoadScene ("Match");
	}

	public void ToHome() {
		PlayerManager.I.player.rightAnswersRow = 0;
		Loader.I.LoadScene ("Home");
	}

	public categories GetRandomCat() {
		System.Array A = System.Enum.GetValues (typeof(categories));
		categories category = (categories)A.GetValue (UnityEngine.Random.Range (0, A.Length));
		return category;
	}

}