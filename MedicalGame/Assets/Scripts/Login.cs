using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Globalization;
using System.Text;
using System;
using Gamedonia.Backend;
using UnityEngine.UI;

public class Login : MonoBehaviour {

	public InputField email_input;
	public InputField password_input;
	private bool i_access = true;

	private string email = "";
	private string password = "";
	private string errorMsg = "";
	private string statusMsg = "";

	void Awake() {
		checkInternet ();
	}
	void Start() {
		if (i_access) {
			if (GamedoniaBackend.INSTANCE == null) {

				statusMsg = "Missing Api Key/Secret. Check the README.txt for more info.";
			}
		}

	}

	void Update() {
		email = email_input.text;
		password = password_input.text;
	}

	public void LoginUser() {
		if (i_access) {
			// Login with email and password, store session token
			GamedoniaUsers.LoginUserWithEmail (email.ToLower (), password, OnLogin);
			if (errorMsg != "") {
				Debug.Log (errorMsg);
			}

			if (statusMsg != "") {
				Debug.Log (statusMsg);
			}
		}
	}

	public void OnLogin (bool success) {
		statusMsg = "";
		if (success) {
			GameObject.Find ("Login").GetComponent<Animator> ().SetInteger ("AnimState", 1);
			StartCoroutine (ChangeScene());
		} else {
			errorMsg = GamedoniaBackend.getLastError().ToString();
			Debug.Log(errorMsg);
		}

	}

	public IEnumerator ChangeScene() {
		yield return new WaitForSeconds (1f);
		Application.LoadLevel("Profile");
	}

	private void checkInternet() {
		GamedoniaBackend.isInternetConnectionAvailable(delegate (bool success) { 
			if (success) { 
				i_access = true;
			} else {
				i_access = false;
				errorMsg = "No internet access";
				Debug.Log(errorMsg);
			}
		});
	}
		
}
