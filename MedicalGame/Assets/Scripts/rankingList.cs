using UnityEngine;
using System.Collections;
using Gamedonia.Backend;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class rankingList : MonoBehaviour {

	
	// Use this for initialization
	void Start () {
		showTopTenPlayers();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void showTopTenPlayers() {
		float delay = 0;
		// Get the top 10 players of the game
		GamedoniaUsers.Search("{}", 10, "{profile.wonAttr:-1}", 0, delegate (bool success, IList data) {
			if (success) {
				if (data != null) {
					for(int i = 0; i < data.Count; i++) {
						Dictionary<string, object> resultUser = (Dictionary<string, object>)data[i];
						Dictionary<string, object> resultProfile = (Dictionary<string, object>)resultUser["profile"];
						GameObject row = gameObject.transform.GetChild(i).gameObject;
						if(i <= 2) {
							row.transform.GetChild(2).GetComponent<Image>().sprite = PlayerManager.I.GetRankSprite (int.Parse(resultProfile["lvl"].ToString()));
							row.transform.GetChild(0).GetComponent<Text>().DOText(resultProfile["name"].ToString(), 2f, true, ScrambleMode.All).SetDelay(delay);
							delay += .5f;
						} else {
							row.transform.GetChild(0).GetComponent<Text>().text = resultProfile["name"].ToString();
						}
						string scoreAdd = (int.Parse(resultProfile["wonAttr"].ToString()) > 1 ? " punten" : " punt");
						row.transform.GetChild(1).GetComponent<Text>().text = resultProfile["wonAttr"].ToString()+scoreAdd;
						// 

					}
				}
			}
			else {

			}
		});
	}
}
