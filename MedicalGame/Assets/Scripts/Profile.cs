using UnityEngine;
using System.Collections;
using System;
using Gamedonia.Backend;
using UnityEngine.UI;


public class Profile : MonoBehaviour {


	public Text name;
	public Text age;
	public Text email;
	private string errorMsg = "";

	private GDUserProfile userProfile;

	void Awake() {
		GameObject.Find ("Login").GetComponent<Animator> ().SetInteger ("AnimState", 2);
	}
	void Start() {
		GamedoniaUsers.GetMe(OnGetMe);
	}


	public void LogOut() {
		GamedoniaUsers.LogoutUser(OnLogout);
	}

	public void OnLogout(bool success) {

		if (success) {

			Application.LoadLevel("Login");

		}else {

			errorMsg = GamedoniaBackend.getLastError().ToString();
			Debug.Log(errorMsg);	

		}

	}

	void OnGetMe(bool success, GDUserProfile userProfile) {

		if (success) {
			Debug.Log ("success");
			this.userProfile = userProfile;
			if (userProfile != null && userProfile.profile ["nickname"] != null)
				name.text = userProfile.profile ["nickname"] as string;

			if (userProfile != null && userProfile.profile ["age"] != null)
				age.text = userProfile.profile ["age"] as string;

			if (userProfile != null && userProfile.profile ["email"] != null)
				email.text = userProfile.profile ["email"] as string;
		}else {
			errorMsg = GamedoniaBackend.getLastError().ToString();
			Debug.Log(errorMsg);
		}

	}
}
