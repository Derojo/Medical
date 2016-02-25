using UnityEngine;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GamedoniaUsers.LoginUserWithSessionToken (delegate (bool success) {
			if (success) {
				Debug.Log("profile");
				SceneManager.LoadScene("Profile");
			} else {
				Debug.Log("login");
                SceneManager.LoadScene("Login");
			}
		});
	}
}