using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class CurrentMachtes : MonoBehaviour {

	public List<Match> matches;
	// Use this for initialization
	void Start () {
		matches = MatchManager.I.matches;
		for (int i = 0; i < matches.Count; i++)
		{
			GameObject matchUI = Instantiate (Resources.Load ("MatchUIRow")) as GameObject;
			string matchId = matches [i].m_ID;
			// Housekeeping
			matchUI.name = matchId;
			matchUI.transform.SetParent (this.transform, false);
			// Set match information
			matchUI.GetComponentInChildren<Text>().text = matchUI.name  = matchId;

			matchUI.GetComponent<Button> ().onClick.AddListener (delegate {LoadMatch (matchId); });

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadMatch(string id) {
		MatchManager.I.LoadCurrentMatch (id);
	}

}
