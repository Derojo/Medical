using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;
using UnityEngine.UI;

public class FriendList : MonoBehaviour {

	// Use this for initialization
	void Start () {
		showFriends ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void showFriends() {
		Debug.Log (PlayerManager.I.friends.Count);
		foreach (KeyValuePair<string, object> friend in PlayerManager.I.friends) {
			GameObject friendRow = Instantiate (Resources.Load ("FriendRow")) as GameObject;
			friendRow.name =  friend.Key;
//			friendRow.transform.GetChild (0).transform.GetChild (0).GetComponentInChildren<Text> ().text = PlayerManager.I.friends [friend.Key + "_name"].ToString ();
			if (PlayerManager.I.friendProfiles != null && PlayerManager.I.friendProfiles.Count > 0) {
				Dictionary<string, object> oppProfile = (Dictionary<string, object>)PlayerManager.I.friendProfiles [friend.Key];
				GameObject horizontal = friendRow.transform.GetChild (0).transform.GetChild (0).gameObject;
				horizontal.GetComponentInChildren<Text>().text = oppProfile["name"].ToString();
				horizontal.GetComponentInChildren<Image>().sprite = PlayerManager.I.GetRankSprite (int.Parse(oppProfile["lvl"].ToString()));
			} else {
				setFriendInformation (friend.Key, friendRow);
			}
			friendRow.transform.SetParent (this.transform, false);
		}

	}


	private void setFriendInformation(string oppId, GameObject parent) {
		if (oppId != "") {
			GamedoniaUsers.GetUser (oppId, delegate (bool success, GDUserProfile data) { 
				if (success) {
					Dictionary<string, object> oppProfile = new Dictionary<string, object> ();
					oppProfile = data.profile;
					parent.transform.GetChild(0).transform.GetChild(0).GetComponentInChildren<Text>().text = oppProfile["name"].ToString();
				}
			});
		}
	}
}
