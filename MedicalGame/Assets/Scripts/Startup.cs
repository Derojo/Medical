using UnityEngine;
using Gamedonia.Backend;
using UnityEngine.UI;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GamedoniaUsers.LoginUserWithSessionToken (delegate (bool success) {
			if (success) {
				Debug.Log("profile");
				Application.LoadLevel("Profile");
			} else {
				Debug.Log("login");
				Application.LoadLevel("Login");
			}
		});
	}
}