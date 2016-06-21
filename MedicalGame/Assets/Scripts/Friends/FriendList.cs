using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;
using UnityEngine.UI;
using LitJson_Gamedonia;

public class FriendList : MonoBehaviour {

	// Use this for initialization
	void Start () {
		showFriends ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		

	private void showFriends() {
		foreach (KeyValuePair<string, object> friend in PlayerManager.I.friends) {
            GameObject friendRow = Instantiate (Resources.Load ("FriendRow")) as GameObject;
            string friendKey = friend.Key;
            List<int> attributesList = PlayerManager.I.GetFriendAttributes(friendKey);
            int ammountOfAttributes = attributesList.Count;
            friendRow.name = friendKey;
			if (PlayerManager.I.friendProfiles != null && PlayerManager.I.friendProfiles.Count > 0) {
				Dictionary<string, object> oppProfile = (Dictionary<string, object>)PlayerManager.I.friendProfiles [friend.Key];
				GameObject horizontal = friendRow.transform.GetChild (0).transform.GetChild (0).gameObject;
                GameObject inputField = friendRow.transform.GetChild (1).transform.GetChild(0).gameObject;
                horizontal.GetComponentInChildren<Text>().text = oppProfile["name"].ToString();
				horizontal.GetComponentInChildren<Image>().sprite = PlayerManager.I.GetRankSprite (int.Parse(oppProfile["lvl"].ToString()));
				horizontal.GetComponentInChildren<Button> ().onClick.AddListener (delegate {MatchManager.I.StartFriendMatch (friendKey); });

                //displaying unlocked info
                if (ammountOfAttributes > 0)
                {
                    inputField.transform.GetChild(8).gameObject.SetActive(false);
                    inputField.transform.GetChild(4).GetComponent<Text>().text = oppProfile["age"].ToString();
                }
                if (ammountOfAttributes > 1)
                {
                    inputField.transform.GetChild(9).gameObject.SetActive(false);
                    inputField.transform.GetChild(5).GetComponent<Text>().text = oppProfile["color"].ToString();
                }

                if (ammountOfAttributes > 2)
                {
                    inputField.transform.GetChild(10).gameObject.SetActive(false);
                    inputField.transform.GetChild(6).GetComponent<Text>().text = oppProfile["hobby"].ToString();
                }
                if (ammountOfAttributes > 3)
                {
                    inputField.transform.GetChild(11).gameObject.SetActive(false);
                    inputField.transform.GetChild(7).GetComponent<Text>().text = oppProfile["film"].ToString();
                }                  
                      

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
