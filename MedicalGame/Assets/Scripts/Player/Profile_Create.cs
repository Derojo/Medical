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
	public GameObject Response;
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
	private bool nameIsAvailable = true;
	private bool searchComplete = false;



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
		StartCoroutine(processInformation());
		// Callback to Gamedonia server
	}

	void OnGetMe(bool success, GDUserProfile userProfile) {
		if (success) {
			LoggedInUser = userProfile;
			// Set playerprefs, store so we dont need to use http requests when we dont need it
			Debug.Log(LoggedInUser._id);
			PlayerManager.I.player.playerID = LoggedInUser._id; // Player id
			fillExistingProfile ();
		}else {
			errorMsg = GamedoniaBackend.getLastError().ToString();
		}
	}
	
	public void ValidateField(InputField inputfield) {
		Debug.Log("validate field:"+inputfield.name);
		Text icon = GameObject.Find (inputfield.name + "_check").GetComponent<Text> ();
		bool nameCheck = false;
		if(inputfield.name == "p_name") {
			if(!nameIsAvailable) {
				nameCheck = true;
			} else {
				nameCheck = false;
			}
		}
		if (inputfield.text == "" || nameCheck) {
			Errors = true;
			if(!nameCheck) {
				changeInputColor (inputfield, error_color);
				inputfield.text = getErrorMessage(inputfield.name);
			}
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

	private IEnumerator processInformation() {
		checkAvailableName();
		while(!searchComplete) {
			yield return new WaitForSeconds(1);
		}
		bool returnValue = true;
		foreach(KeyValuePair<string,string> error in errorData)
		{
			InputField input = (InputField)GameObject.Find (error.Key).GetComponent<InputField>();
			ValidateField (input);
			if (input.text == error.Value) {
				returnValue = false;
			}
		}
		
		if(returnValue && nameIsAvailable) {
			// Store to database
			LoggedInUser.profile ["name"] =  p_name.text;
			LoggedInUser.profile ["age"] = int.Parse (p_age.text);
			LoggedInUser.profile ["color"] =  p_color.text;
			LoggedInUser.profile ["hobby"] = p_hobby.text;
			LoggedInUser.profile ["film"] = p_film.text;
            LoggedInUser.profile ["instelling"] = p_instelling.text;
			LoggedInUser.profile ["created_profile"] = true;

            // Store locally
            PlayerManager.I.changeProfile (new PlayerProfile (p_name.text, int.Parse (p_age.text), p_color.text, p_hobby.text, p_film.text, p_instelling.text));
			PlayerManager.I.player.avatar = LoggedInUser.profile ["avatar"].ToString();
			PlayerManager.I.player.admin = (LoggedInUser.profile ["admin"].ToString() == "True" ? true : false);
			if (!PlayerManager.I.changingProfile) {
				PlayerManager.I.player.playerLvl = int.Parse(LoggedInUser.profile ["lvl"].ToString());
				int attributesAmount = PlayerManager.I.getTotalFriendAttributes((Dictionary<string, object>) LoggedInUser.profile["friends"]);
				PlayerManager.I.player.playerWonAttr = attributesAmount;
				LoggedInUser.profile ["wonAttr"] = attributesAmount;

			}
			
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
			}, true);
		
		}
	}

	private void checkAvailableName() {
		searchComplete = false;
		string checkName = p_name.text;
		if(LoggedInUser.profile ["name"].ToString() != checkName) {
			GamedoniaUsers.Search("{\"profile.name\":'" + checkName +"'}", delegate (bool success, IList data) { 
				if (success) {
					if(data != null && data.Count > 0) {
						nameIsAvailable = false;
						Response.SetActive(true);
						searchComplete = true;
						Debug.Log("name found");
					} else {
						nameIsAvailable = true;
						Response.SetActive(false);
						searchComplete = true;
						Debug.Log("name not found");
					}
				} else {
					Response.SetActive(false);
					nameIsAvailable = false;
					searchComplete = true;
					Debug.Log("no success");
					
				}
			});
		} else {
			nameIsAvailable = true;
			searchComplete = true;
			Response.SetActive(false);
		}

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