using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;
using UnityEngine.UI;
using LitJson_Gamedonia;
using DG.Tweening;

public class FriendList : MonoBehaviour {

    public InputField friendSearcher;
    public GameObject noFriendFound;
	public GameObject lives;
	public Text livesText;
	private int livesLeft = 5;
    // Use this for initialization
    void Start ()
    {
		showFriends ();
		if (MatchManager.I.matches != null && MatchManager.I.matches.Count > 0) {
			livesLeft = RuntimeData.I.livesAmount - MatchManager.I.getTotalActiveMatches();
			if(livesLeft  < 0 ) {
				livesLeft = 0;
			}
			livesText.text = livesLeft.ToString();
		}
	}

	
	private void showFriends() {
		foreach (KeyValuePair<string, object> friend in PlayerManager.I.friends) {
            setFriendInformation(friend.Key);
        }

	}


	private void setFriendInformation(string id) {
        GameObject friendRow = Instantiate(Resources.Load("FriendRow")) as GameObject;

        string friendKey = id;
        List<int> attributesList = PlayerManager.I.GetFriendAttributes(friendKey);
        int ammountOfAttributes = attributesList.Count;
        friendRow.name = friendKey;
        if (PlayerManager.I.friendProfiles != null && PlayerManager.I.friendProfiles.Count > 0)
        {
            Dictionary<string, object> oppProfile = (Dictionary<string, object>)PlayerManager.I.friendProfiles[friendKey];
            GameObject horizontal = friendRow.transform.GetChild(0).transform.GetChild(0).gameObject;
            GameObject inputField = friendRow.transform.GetChild(1).transform.GetChild(0).gameObject;
            horizontal.GetComponentInChildren<Text>().text = oppProfile["name"].ToString();
            horizontal.GetComponentInChildren<Image>().sprite = PlayerManager.I.GetRankSprite(int.Parse(oppProfile["lvl"].ToString()));
			Debug.Log(livesLeft);
			if(livesLeft > 0) {
				if(!MatchManager.I.checkForPlayingWithFriend(friendKey)) {
					horizontal.GetComponentInChildren<Button>().onClick.AddListener(delegate { MatchManager.I.StartFriendMatch(friendKey); });
				}
			}

            //displaying unlocked info
            if (ammountOfAttributes > 0)
            {
                inputField.transform.GetChild(10).gameObject.SetActive(false);
                inputField.transform.GetChild(5).GetComponent<Text>().text = oppProfile["age"].ToString();
            }
            if (ammountOfAttributes > 1)
            {
                inputField.transform.GetChild(11).gameObject.SetActive(false);
                inputField.transform.GetChild(6).GetComponent<Text>().text = oppProfile["color"].ToString();
            }

            if (ammountOfAttributes > 2)
            {
                inputField.transform.GetChild(12).gameObject.SetActive(false);
                inputField.transform.GetChild(7).GetComponent<Text>().text = oppProfile["hobby"].ToString();
            }
            if (ammountOfAttributes > 3)
            {
                inputField.transform.GetChild(13).gameObject.SetActive(false);
                inputField.transform.GetChild(8).GetComponent<Text>().text = oppProfile["film"].ToString();
            }
            if (ammountOfAttributes > 4)
            {
                inputField.transform.GetChild(14).gameObject.SetActive(false);
                inputField.transform.GetChild(9).GetComponent<Text>().text = oppProfile["film"].ToString();
            }

        }
        friendRow.transform.SetParent(this.transform, false);
    }

  /////////*****Friendsearch*****/////////


    public void searchForFriends()
    {
        noFriendFound.SetActive(false);
        noFriendFound.GetComponent<Text>().text = "";

        if (friendSearcher.text != "")
        {
            string friendcode = friendSearcher.text;

            GamedoniaUsers.Search("{\"profile.name\":\"" +friendcode+ "\"}", delegate (bool success, IList data)
            {
                if (success)
                {
                    if (data != null) {
                        Dictionary<string, object> userData = (Dictionary<string, object>)data[0];
                        string id = userData["_id"].ToString();
                        if (!PlayerManager.I.friends.ContainsKey(id))
                        {
                            PlayerManager.I.AddFriend(id);
                            StartCoroutine(setFriendInformationAfterTime(1f, id));
                        }
                        else
                        {
                            noFriendFound.GetComponent<Text>().text = "Je hebt " + friendcode + " al als vriend";
                            noFriendFound.SetActive(true);
                        }
                    } else {
                        noFriendFound.GetComponent<Text>().text = "Vriend niet gevonden";
                        noFriendFound.SetActive(true);
                    }
                }
                else
                {
  
                    Debug.Log("Error");
                }

            });
            Debug.Log("looking for friends");
        }
        
    }

    public IEnumerator setFriendInformationAfterTime(float time, string id) {
        yield return new WaitForSeconds(time);
        setFriendInformation(id);

    }
}
