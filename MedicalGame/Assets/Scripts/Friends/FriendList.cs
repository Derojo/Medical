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
		Debug.Log ("FRIEND COUNT:"+PlayerManager.I.friends.Count);
		Debug.Log ("FRIEND COUNT2:"+PlayerManager.I.friendProfiles.Count);
		foreach (KeyValuePair<string, object> friend in PlayerManager.I.friends) {
			GameObject friendRow = Instantiate (Resources.Load ("FriendRow")) as GameObject;
			string friendKey = friend.Key;
			friendRow.name = friendKey;
//			Debug.Log ("test");
//			Debug.Log (PlayerManager.I.friendProfiles.Count);
			if (PlayerManager.I.friendProfiles != null && PlayerManager.I.friendProfiles.Count > 0) {
//				Debug.Log ("test2");
				Dictionary<string, object> oppProfile = (Dictionary<string, object>)PlayerManager.I.friendProfiles [friend.Key];
				GameObject horizontal = friendRow.transform.GetChild (0).transform.GetChild (0).gameObject;
				horizontal.GetComponentInChildren<Text>().text = oppProfile["name"].ToString();
				horizontal.GetComponentInChildren<Image>().sprite = PlayerManager.I.GetRankSprite (int.Parse(oppProfile["lvl"].ToString()));
				horizontal.GetComponentInChildren<Button> ().onClick.AddListener (delegate {MatchManager.I.StartFriendMatch (friendKey); });
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
