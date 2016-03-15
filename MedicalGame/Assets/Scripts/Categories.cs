using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Categories : MonoBehaviour {

	public enum categories
	{
		TV_ENTERTAINMENT = 1,
		GELOOF_CULTUUR	 = 2,
		ZORG_WETENSCHAP	 = 3,
		GESCHIEDENIS	 = 4,
		SPORT			 = 5,
		GEOGRAFIE		 = 6

	}

	public Text catTitle;
	public GameObject TV_ENTERTAINMENT_a;
	public GameObject GELOOF_CULTUUR_a;
	public GameObject ZORG_WETENSCHAP_a;
	public GameObject GESCHIEDENIS_a;
	public GameObject SPORT_a;
	public GameObject GEOGRAFIE_a;

	// Use this for initialization
	void Start () {
		// Get current match
		Match currentMatch = MatchManager.Instance.GetMatch (MatchManager.Instance.currentMatchID);

		//Get random cat, show animation
		MatchManager.Instance.currentCategory = (int)GetRandomCat();
		ShowCategory (MatchManager.Instance.currentCategory);
	}

	public void ShowCategory(int catID) {
		switch (catID) {
			case (int)categories.TV_ENTERTAINMENT:
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
				break;
		}
	}

	public void PlayMatch() {
		SceneManager.LoadScene("Match");
	}

	public categories GetRandomCat() {
		System.Array A = System.Enum.GetValues (typeof(categories));
		categories category = (categories)A.GetValue (UnityEngine.Random.Range (0, A.Length));
		return category;
	}
		
}