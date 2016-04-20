using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Categories : MonoBehaviour {

	public enum categories
	{

		//TV_ENTERTAINMENT = 1,
		//GELOOF_CULTUUR	 = 2,
		///ZORG_WETENSCHAP	 = 3,
		//GESCHIEDENIS	 = 4,
		//SPORT			 = 5,
		GEOGRAFIE		 = 6,
        GGZ            = 7,
        OUDERENZORG      = 8,
        ZIEKENHUISZORG   = 9,
        ZORG_ALGEMEEN    = 10,
        TAND_HUISARTS    = 11,
        GEHANDICAPTEN    = 12


	}

	public Text catTitle;                    
	public GameObject TV_ENTERTAINMENT_a;
	public GameObject GELOOF_CULTUUR_a;
	public GameObject ZORG_WETENSCHAP_a;
	public GameObject GESCHIEDENIS_a;
	public GameObject SPORT_a;
	public GameObject GEOGRAFIE_a;
	public Image image;

	// Use this for initialization
	void Start () {
		// Get current match
		Match currentMatch = MatchManager.I.GetMatch (MatchManager.I.currentMatchID);

		//Get random cat, show animation
		if (currentMatch.m_cc != 0) {
			MatchManager.I.currentCategory = currentMatch.m_cc;
		} else {
			MatchManager.I.currentCategory = (int)GetRandomCat();
		}
		currentMatch.m_cc = MatchManager.I.currentCategory;
		ShowCategory (MatchManager.I.currentCategory);
	}

	public void ShowCategory(int catID) {
		switch (catID) {
			/*case (int)categories.TV_ENTERTAINMENT:
				TV_ENTERTAINMENT_a.SetActive (true);
				catTitle.text = "TV & Entertainment"; 
				break;
			case (int)categories.GELOOF_CULTUUR:
				GELOOF_CULTUUR_a.SetActive (true);
				catTitle.text = "Geloof & Cultuur";
				break;
			case (int)categories.ZORG_WETENSCHAP:
				ZORG_WETENSCHAP_a.SetActive (true);
				catTitle.text = "Zorg & Wetenschap";
				break;
			case (int)categories.GESCHIEDENIS:
				GESCHIEDENIS_a.SetActive (true);
				catTitle.text = "Geschiedenis";
				break;
			case (int)categories.SPORT:
				SPORT_a.SetActive (true);
				catTitle.text = "Sport";
				break;
			case (int)categories.GEOGRAFIE:
				GEOGRAFIE_a.SetActive(true);
				catTitle.text = "Geografie";
				break;*/
                /************** health catagories*********************/
            case (int)categories.GGZ:
                catTitle.text = "GGZ";
                break;
            case (int)categories.OUDERENZORG:
                catTitle.text = "Ouderenzorg";
                break;
            case (int)categories.ZIEKENHUISZORG:
                catTitle.text = "Ziekenhuiszorg";
                break;
            case (int)categories.ZORG_ALGEMEEN:
                catTitle.text = "Algemeen";
                break;
            case (int)categories.TAND_HUISARTS:
                catTitle.text = "Tandarts & Huisarts";
                break;
            case (int)categories.GEHANDICAPTEN:
                 catTitle.text = "Gehandicaptenzorg";
                 break;
        }
    }

	public static string getCategoryNameById(int catID) {
		switch (catID) {/*
			case (int)categories.TV_ENTERTAINMENT:
				return "TV & Entertainment"; 
			case (int)categories.GELOOF_CULTUUR:
				return "Geloof & Cultuur";
			case (int)categories.ZORG_WETENSCHAP:
				return "Zorg & Wetenschap";
			case (int)categories.GESCHIEDENIS:
				return "Geschiedenis";
			case (int)categories.SPORT:
				return "Sport";
			case (int)categories.GEOGRAFIE:
				return "Geografie";/*
                /************** health catagories*********************/
                case (int)categories.GGZ:
                    return "GGZ";
                case (int)categories.OUDERENZORG:
                    return "Ouderenzorg";
                case(int)categories.ZIEKENHUISZORG:
                    return "Ziekenhuiszorg";
                case (int)categories.ZORG_ALGEMEEN:
                    return "Algemeen";
                case (int)categories.TAND_HUISARTS:
                    return "Tandarts & Huisarts";
                case (int)categories.GEHANDICAPTEN:
                    return "Gehandicaptenzorg"; 

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