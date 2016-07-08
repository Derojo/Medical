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
using DG.Tweening;

public class Login : MonoBehaviour {


	public string facebookAppID = "";
	private static string [] READ_PERMISSIONS = new string[] {"read_stream", "read_friendlists"};

	public InputField email_input;
	public InputField password_input;
	public Image emailImg;
	public Image passwordImg;
	public GameObject errorPopup;
	public GameObject errorLogin;
	public GameObject errorLoginImg;
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

	private string fbUserName = null;
	private string fbUserId = null;


	void Awake() {
		FacebookBinding.Init (facebookAppID);	
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
	}


	/** DEFAULT LOGIN **/
	public void LoginUser() {
		errorLogin.SetActive (false);
		errorLoginImg.SetActive (false);
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

	public void validateFields() {
		if (email_input.text == "") {
			Debug.Log ("test");
			email_input.text = errorMessageEmail;
			emailImg.DOColor (errorColor, 1);
			//email_input.GetComponent<Outline> ().effectColor = errorColor;
		}
		if (password_input.text == "") {
			password_input.contentType = InputField.ContentType.Standard;
			password_input.text = errorMessagePassword;
			passwordImg.DOColor (errorColor, 1);
			//password_input.GetComponent<Outline> ().effectColor = errorColor;
		}
		if (password_input.text != errorMessagePassword && email_input.text != errorMessageEmail) {
			Debug.Log ("is validated");
			isValidated = true;
			emailImg.DOColor (defaultColor, 1);
			passwordImg.DOColor (defaultColor, 1);
		}
	}

	public void validateField(string name) {
		if (name == "email") {
			if (email_input.text == "") {
				email_input.text = errorMessageEmail;
				emailImg.DOColor (errorColor, 1);
			} else {
				emailImg.DOColor (defaultColor, 1);
			}
		}
		if (name == "password") {
			if (password_input.text == "") {
				password_input.contentType = InputField.ContentType.Standard;
				password_input.text = errorMessagePassword;
				passwordImg.DOColor (errorColor, 1);
			} else {
				passwordImg.DOColor (defaultColor, 1);
			}
		}
	}

	public void OnLogin (bool success) {
		statusMsg = "";
		if (success) {
			// Set playerprefs loggedIn to true so we dont need to log in again via http
			PlayerManager.I.player.loggedIn = true;
			PlayerManager.I.Save ();
			PlayerManager.I.LoadFriends ();
			if (!PlayerManager.I.player.createdProfile) {
				SceneManager.LoadScene ("Profile_Create");
			} else {
				SceneManager.LoadScene ("Home");
			}
		} else {
			loader.disableLoader();
			errorMsg = GamedoniaBackend.getLastError().ToString();
			int errorCode = GamedoniaBackend.getLastError ().httpErrorCode;
			if (errorCode == 400 || errorCode == 401) {
				//email_input.GetComponent<Outline> ().effectColor = errorColor;
				//password_input.GetComponent<Outline> ().effectColor = errorColor;
				emailImg.DOColor(errorColor, 1);
				passwordImg.DOColor (errorColor, 1);
			}
			if (errorCode == 401) {
				errorLogin.SetActive (true);
				errorLoginImg.SetActive(true);
			}
		}

	}

	/** FACEBOOK LOGIN **/

	public void LoginWithFacebook() {
		if (!Application.isEditor) {
			statusMsg = "Initiating Facebook session...";
			FacebookBinding.OpenSessionWithReadPermissions(READ_PERMISSIONS, OnFacebookOpenSession);
		}
		else {
			errorMsg = "Facebook won't work on Unity Editor, try it on a device.";
		}
	}

	void OnFacebookOpenSession(bool success, bool userCancelled, string message) {

		if (success) {
			statusMsg = "Recovering Facebook profile...";
			FacebookBinding.RequestWithGraphPath("/me",null,"GET",OnFacebookMe);
		}else {
			errorMsg = "Unable to open session with Facebook";
		}
	}

	void OnFacebookLogin (bool success) {

		if (success) {
			Dictionary<string,object> profile = new Dictionary<string, object>();
			profile.Add("name", fbUserName);
			profile.Add("color", "");
			profile.Add("hobby", ""); 
			profile.Add("film","");
			profile.Add("age", 0);
			profile.Add("lvl", 1);
			profile.Add("wonAttr", 0);
			profile.Add("friends", new Dictionary<string, object> ());
			profile.Add("admin", false);
			profile.Add("avatar", "");
			GamedoniaUsers.UpdateUser(profile, OnLogin);

		} else {
			errorMsg = GamedoniaBackend.getLastError().ToString();
			Debug.Log(errorMsg);
		}

	}
	private void OnFacebookMe(IDictionary data) {

		statusMsg = "Initiating Gamedonia session...";
		fbUserId = data ["id"] as string;
		PlayerManager.I.player.fbuserid = fbUserId;
		fbUserName = data ["name"] as string;

		Debug.Log ("AccessToken: " + FacebookBinding.GetAccessToken() + " fbuid: " + fbUserId);


		Dictionary<string,object> facebookCredentials = new Dictionary<string,object> ();
		facebookCredentials.Add("fb_uid",fbUserId);
		facebookCredentials.Add("fb_access_token",FacebookBinding.GetAccessToken());

		GamedoniaUsers.Authenticate (GamedoniaBackend.CredentialsType.FACEBOOK,facebookCredentials, OnFacebookLogin);

	}



	/** CHECK FOR INTERNET CONNECTION **/
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

}
