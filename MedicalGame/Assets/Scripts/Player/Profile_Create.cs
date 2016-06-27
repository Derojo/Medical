using UnityEngine;
using System.Collections;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Profile_Create : MonoBehaviour {

	// Main loader
	public GameObject Loader;
	// Input form fields
	public InputField p_name;
	public InputField p_age;
	public InputField p_color;
	public InputField p_hobby;
	public InputField p_film;
    public InputField p_instelling;

	// state colors
	public Color error_color;
	public Color succes_color;
	// Error message handling
	public List<string> errorMessages = new List <string>();
	public Dictionary<string, string> errorData = new Dictionary <string, string>();

	private string errorMsg = "";
	private GDUserProfile LoggedInUser;
	private bool isValidated = false;
	private bool Errors = false;

	private Loader loader = null;
	private string fbpictureurl = "";


	void Awake() {
		if (loader == null) {
			loader = GameObject.Find ("Loader").GetComponent<Loader> ();
		}
		if (errorData.Count != errorMessages.Count) {
			foreach (string message in errorMessages) {
				string[] messageSplit = message.Split (':');
				errorData.Add (messageSplit [0], messageSplit [1]);
			}
		}
	}
	// Use this for initialization
	void Start() {
		GamedoniaUsers.GetMe(OnGetMe);
	}


	void Update() {
		if (Errors) {
			p_name.text = (p_name.isFocused ? (p_name.text == getErrorMessage (p_name.name) ? "" : p_name.text) : p_name.text);
			p_age.text = (p_age.isFocused ? (p_age.text == getErrorMessage (p_age.name) ? "" : p_age.text) : p_age.text);
			p_color.text = (p_color.isFocused ? (p_color.text == getErrorMessage (p_color.name) ? "" : p_color.text) : p_color.text);
			p_hobby.text = (p_hobby.isFocused ? (p_hobby.text == getErrorMessage (p_hobby.name) ? "" : p_hobby.text) : p_hobby.text);
			p_film.text = (p_film.isFocused ? (p_film.text == getErrorMessage (p_film.name) ? "" : p_film.text) : p_film.text);
            p_instelling.text = (p_instelling.isFocused ? (p_instelling.text == getErrorMessage(p_instelling.name) ? "" : p_instelling.text) : p_instelling.text);
        }
	}

	public void AddUserInformation() {
		// Add filled in form to profile
		ChangeProfileDictionary();
		// Callback to Gamedonia server
		if (isValidated) {
			loader.enableLoader ();
			GamedoniaUsers.UpdateUser (LoggedInUser.profile, delegate (bool success) {

					if (success) {
						if (PlayerManager.I.changingProfile) {
							Loader.SetActive (false);
							SceneManager.LoadScene("Profile");
						}
						else {
							Loader.SetActive (false);
							PlayerManager.I.player.createdProfile = true;
							SceneManager.LoadScene("Avatar");
						}

	                } 

					else {
						Debug.Log ("Error");
					}


 		
			}, true);
		}
	}

	void OnGetMe(bool success, GDUserProfile userProfile) {
		if (success) {
			LoggedInUser = userProfile;
			// Set playerprefs, store so we dont need to use http requests when we dont need it
			PlayerManager.I.player.playerID = LoggedInUser._id; // Player id
			fillExistingProfile ();
		}else {
			errorMsg = GamedoniaBackend.getLastError().ToString();
			Debug.Log(errorMsg);
		}
	}

	private void ChangeProfileDictionary() {
		isValidated = validateAll ();

		if(isValidated) {
			// Store to database
			LoggedInUser.profile ["name"] =  p_name.text;
			LoggedInUser.profile ["age"] = int.Parse (p_age.text);
			LoggedInUser.profile ["color"] =  p_color.text;
			LoggedInUser.profile ["hobby"] = p_hobby.text;
			LoggedInUser.profile ["film"] = p_film.text;
            LoggedInUser.profile ["instelling"] = p_instelling.text;

            // Store locally
            PlayerManager.I.changeProfile (new PlayerProfile (p_name.text, int.Parse (p_age.text), p_color.text, p_hobby.text, p_film.text, p_instelling.text));
			PlayerManager.I.player.avatar = LoggedInUser.profile ["avatar"].ToString();
			PlayerManager.I.player.playerLvl = int.Parse(LoggedInUser.profile ["lvl"].ToString());
		}
	}

	public void ValidateField(InputField inputfield) {
		Text icon = GameObject.Find (inputfield.name + "_check").GetComponent<Text> ();
		if (inputfield.text == "") {
			Errors = true;
			changeInputColor (inputfield, error_color);
			inputfield.text = getErrorMessage(inputfield.name);
			// Hide succes icon if visibile
			if (icon.enabled) {
				icon.enabled = false;
			}
		} else {
			if (inputfield.text != getErrorMessage (inputfield.name)) {
				changeInputColor (inputfield, succes_color);
				// Show succes icon
				icon.enabled = true;
			}

		}
	}

	private void changeInputColor(InputField inputfield, Color color) {
		ColorBlock cb = inputfield.colors;
		cb.normalColor = color;
		inputfield.colors = cb;
	}

	private void clearField(InputField inputfield) {
		ColorBlock cb = inputfield.colors;
		cb.normalColor = new Color (255, 255, 255, 1);
		inputfield.colors = cb;
	}


	private string getErrorMessage(string name) {
		string error = "";
		errorData.TryGetValue (name, out error);
		return error;
	}

	private bool validateAll() {
		bool returnValue = true;
		foreach(KeyValuePair<string,string> error in errorData)
		{
			Debug.Log (error.Key);
			InputField input = (InputField)GameObject.Find (error.Key).GetComponent<InputField>();
			ValidateField (input);
			if (input.text == error.Value) {
				returnValue = false;
			}
		}
		return returnValue;
	}

	private void fillExistingProfile() {
		p_name.text = (LoggedInUser.profile ["name"].ToString() != "" ? LoggedInUser.profile ["name"].ToString() : "");
		p_age.text = ( LoggedInUser.profile ["age"].ToString() != "0" ? LoggedInUser.profile ["age"].ToString () : "");
		p_color.text = (LoggedInUser.profile ["color"].ToString() != "" ? LoggedInUser.profile ["color"].ToString() : "");
		p_hobby.text = (LoggedInUser.profile ["hobby"].ToString() != "" ? LoggedInUser.profile ["hobby"].ToString() : "");
		p_film.text = (LoggedInUser.profile ["film"].ToString() != "" ? LoggedInUser.profile ["film"].ToString() : "");
        p_instelling.text = (LoggedInUser.profile["instelling"].ToString() != "" ? LoggedInUser.profile["instelling"].ToString() : "");
    }
}