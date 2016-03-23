using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Globalization;
using System.Text;
using System;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {

	public InputField email_input;
	public InputField password_input;
	public GameObject errorPopup;
	public GameObject errorLogin;
	public Color errorColor;
	public Color defaultColor;
	public float waitForConnectionTime = 10;
	private bool i_access = false;

	private string email = "";
	private string password = "";
	private string errorMsg = "";
	private string statusMsg = "";
	private string errorMessageEmail = "Voer aub uw gebruikersnaam in";
	private string errorMessagePassword = "Voer aub uw wachtwoord in";
	private bool isValidated = false;
	private Loader loader = null;



	void Awake() {
		checkInternet ();
		loader = GameObject.Find ("Loader").GetComponent<Loader> ();
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
		// Set password type to password
		password_input.contentType = (password_input.isFocused ? (password_input.text == errorMessageEmail? InputField.ContentType.Standard : InputField.ContentType.Password) : InputField.ContentType.Password);
		// Hide error messages
		email_input.text = (email_input.isFocused ? (email_input.text == errorMessageEmail? "" : email_input.text) : email_input.text);
		password_input.text = (password_input.isFocused ? (password_input.text == errorMessagePassword ? "" : password_input.text) : password_input.text);
		// Change color outline
		if(email_input.isFocused) {
			email_input.GetComponent<Outline> ().effectColor = defaultColor;
		}
		if(password_input.isFocused) {
			password_input.GetComponent<Outline> ().effectColor = defaultColor;
		}
	}

	public void LoginUser() {
		errorLogin.SetActive (false);
		validateFields ();
		if (isValidated) {
			// Show loading screen
			loader.enableLoader ();
			// Try to Login, if there is no access we are starting the waitForConnection
			if (i_access) {
				// Login with email and password, store session token
				GamedoniaUsers.LoginUserWithEmail (email.ToLower (), password, OnLogin);
				if (errorMsg != "") {
					Debug.Log (errorMsg);
				}

				if (statusMsg != "") {
					Debug.Log (statusMsg);
				}
			} else {
				// Wait for connection, if there is a connection we try to login again
				StartCoroutine (waitForConnection (waitForConnectionTime));
			}
		}
	}
	private void validateFields() {
		if (email_input.text == "") {
			email_input.text = errorMessageEmail;
			email_input.GetComponent<Outline> ().effectColor = errorColor;
		}
		if (password_input.text == "") {
			password_input.contentType = InputField.ContentType.Standard;
			password_input.text = errorMessagePassword;
			password_input.GetComponent<Outline> ().effectColor = errorColor;
		}
		if (password_input.text != "" && email_input.text != "") {
			isValidated = true;
		}
	}
	private IEnumerator waitForConnection(float time) {
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time)
		{		
			if (!i_access) {
				checkInternet ();
				yield return null;
			} else {
				LoginUser ();
			}
		}
		loader.disableLoader ();
		loader.enableBackground ();
		errorPopup.SetActive (true);
	}
	public void OnLogin (bool success) {
		statusMsg = "";
		if (success) {
			// Set playerprefs loggedIn to true so we dont need to log in again via http
			PlayerPrefs.SetInt("loggedIn", 1);
			SceneManager.LoadScene("Profile_Create");
		} else {
			loader.disableLoader();
			errorMsg = GamedoniaBackend.getLastError().ToString();
			int errorCode = GamedoniaBackend.getLastError ().httpErrorCode;
			if (errorCode == 400 || errorCode == 401) {
				email_input.GetComponent<Outline> ().effectColor = errorColor;
				password_input.GetComponent<Outline> ().effectColor = errorColor;
			}
			if (errorCode == 401) {
				errorLogin.SetActive (true);
			}
		}

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
