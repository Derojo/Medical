using UnityEngine;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GamedoniaUsers.LoginUserWithSessionToken (delegate (bool success) {
			if (success) {
				if(PlayerPrefs.HasKey("createdProfile")) {
					SceneManager.LoadScene("Home");
				} else {
					SceneManager.LoadScene("Profile_Create");
				}
			} else {
                SceneManager.LoadScene("Login");
			}
		});
	}
}