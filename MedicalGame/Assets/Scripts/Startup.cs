using UnityEngine;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RuntimeData.I.Load ();
//		RuntimeData.I.PDB.clearAll ();
		if (!RuntimeData.I.PDB.loggedIn) {
			SceneManager.LoadScene ("Login");
		} else {
			if (RuntimeData.I.PDB.createdProfile) {
				SceneManager.LoadScene ("Home");
			} else {
				GamedoniaUsers.LoginUserWithSessionToken (delegate (bool success) {
					if (success) {
						SceneManager.LoadScene ("Profile_Create");

					} else {
						SceneManager.LoadScene ("Login");
					}
				});
			}
		}
	}
}