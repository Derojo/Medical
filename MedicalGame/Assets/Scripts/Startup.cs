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
		if (!PlayerManager.I.player.loggedIn) {
			SceneManager.LoadScene ("Login");
		}
        else
        {
			if (PlayerManager.I.player.createdProfile)
            {
                if (!PlayerManager.I.player.completedIntro)
                {
                    sceneName = "Introduction";
                }
                else
                {
                    sceneName = "Home";
                }
            }
            else
            {
                sceneName = "Profile_Create";
            }
            GamedoniaUsers.LoginUserWithSessionToken (delegate (bool success) {
				if (success) {
					SceneManager.LoadScene (sceneName);
					PlayerManager.I.LoadFriends ();
				} else {
					SceneManager.LoadScene ("Login");
				}
			});
		}
	}

}