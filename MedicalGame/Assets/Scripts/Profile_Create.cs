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
	// state colors
	public Color error_color;
	public Color succes_color;
	// Error message handling
	public List<string> errorMessages = new List <string>();
	public Dictionary<string, string> errorData = new Dictionary <string, string>();

	private string errorMsg = "";
	private GDUserProfile LoggedInUser;
	private bool isValidated = false;

	void Awake() {
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

	public void AddUserInformation() {
		// Add filled in form to profile
		ChangeProfileDictionary();
		// Callback to Gamedonia server
		if (isValidated) {
			Loader.SetActive (true);
			GamedoniaUsers.UpdateUser (LoggedInUser.profile, delegate (bool success) {
				if (success) {
					Loader.SetActive (false);
					PlayerPrefs.SetInt ("createdProfile", 1);
					SceneManager.LoadScene ("Home");
				} else {
					Debug.Log ("Error");
				}
			}, true);
		}
	}

	void OnGetMe(bool success, GDUserProfile userProfile) {
		if (success) {
			LoggedInUser = userProfile;
		}else {
			errorMsg = GamedoniaBackend.getLastError().ToString();
			Debug.Log(errorMsg);
		}
	}

	private void ChangeProfileDictionary() {
		if(!isValidated) {
			ValidateField (p_name);
			LoggedInUser.profile ["name"] = (isValidated ? p_name.text : "");
			ValidateField (p_age);
			LoggedInUser.profile ["age"] = (isValidated ? int.Parse (p_age.text) : 0);
			ValidateField (p_color);
			LoggedInUser.profile ["color"] = (isValidated ? p_color.text : "");
			ValidateField (p_hobby);
			LoggedInUser.profile ["hobby"] = (isValidated ? p_hobby.text : "");
			ValidateField (p_film);
			LoggedInUser.profile ["film"] = (isValidated ? p_film.text : "");
		}
	}

	public void ValidateField(InputField inputfield) {
		if (inputfield.text == "") {
			isValidated = false;
			changeInputColor (inputfield, error_color);
			inputfield.text = getErrorMessage(inputfield.name);
		} else {
			if (inputfield.text != getErrorMessage (inputfield.name)) {
				changeInputColor (inputfield, succes_color);
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


}
