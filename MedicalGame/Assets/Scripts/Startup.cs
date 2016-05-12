using UnityEngine;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class Startup : MonoBehaviour {

	public string sceneName;
	// Use this for initialization
	void Start () {
		RuntimeData.I.Load ();
		Debug.Log (PlayerManager.I.player.loggedIn);
		if (!PlayerManager.I.player.loggedIn) {
			SceneManager.LoadScene ("Login");
		} else {
			if (PlayerManager.I.player.createdProfile) {
                sceneName = "Home";
			} else {
				sceneName = "Profile_Create";
			}
			GamedoniaUsers.LoginUserWithSessionToken (delegate (bool success) {
				if (success) {
					PlayerManager.I.LoadFriends ();
					SceneManager.LoadScene (sceneName);
				} else {
					SceneManager.LoadScene ("Login");
				}
			});
		}
	}

}