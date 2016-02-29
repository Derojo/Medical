using UnityEngine;
using System.Collections;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Profile_Create : MonoBehaviour {

	public GameObject Loader;

	public InputField p_name;
	public InputField p_age;
	public InputField p_color;
	public InputField p_hobby;
	public InputField p_film;

	private string errorMsg = "";
	private GDUserProfile LoggedInUser;

	// Use this for initialization
	void Start() {
		GamedoniaUsers.GetMe(OnGetMe);
	}

	public void AddUserInformation() {
		Loader.SetActive (true);
		// Add filled in form to profile
		ChangeProfileDictionary();
		// Callback to Gamedonia server
		GamedoniaUsers.UpdateUser(LoggedInUser.profile, delegate (bool success) {
			if (success) {
				Loader.SetActive (false);
				PlayerPrefs.SetInt("createdProfile", 1);
				SceneManager.LoadScene("Home");
			} else {
				Debug.Log("Error");
			}
		}, true);
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
		LoggedInUser.profile ["name"] = p_name.text;
		LoggedInUser.profile ["age"] = int.Parse (p_age.text);
		LoggedInUser.profile ["color"] = p_color.text;
		LoggedInUser.profile ["hobby"] = p_hobby.text;
		LoggedInUser.profile ["film"] = p_film.text;
	}
}
