using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Gamedonia.Backend;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class Registration : MonoBehaviour {

	public Text email;
	public InputField password;
	public InputField repassword;
	public Text errorMsgText;
	public Image emailImg;
	public Image passImg;
	public Image passreImg;
	public Color errorColor;
	public Color defaultColor;



	public string errorMsg = "";


	public void Registrate() {
		

		if (email.text == "") {
			emailImg.DOColor (errorColor, 1);
			errorMsg = "Vul al de velden correct in";
		} else {
			emailImg.DOColor (defaultColor, 1);
		}
		if (password.text == "") {
			passImg.DOColor (errorColor, 1);
			errorMsg = "Vul al de velden correct in";
		} else {
			passImg.DOColor (defaultColor, 1);
		}
		if (repassword.text == "") {
			passreImg.DOColor (errorColor, 1);
			errorMsg = "Vul al de velden correct in";
		} else {
			passreImg.DOColor (defaultColor, 1);
		}
		checkPasswords ();

		if ((email.text != "")
			&& (password.text != "")
			&& (repassword.text != "")
			&& (password.text == repassword.text)) {
			Loader.I.enableLoader ();
			Credentials credentials = new Credentials();
			credentials.email = email.text;
			credentials.password = password.text;
			GDUser user = new GDUser();
			user.credentials = credentials;

			user.profile.Add("email", email.text);
			user.profile.Add("name", "");
			user.profile.Add("color", "");
			user.profile.Add("hobby", "");
			user.profile.Add("film", "");
			user.profile.Add("age", 0);
			user.profile.Add("instelling", "");
			user.profile.Add("lvl", 1);
			user.profile.Add("wonAttr", 1);
			user.profile ["friends"] = new Dictionary<string, object> ();
			user.profile.Add("admin", false);
			GamedoniaUsers.CreateUser(user,OnCreateUser);

		}else {
			errorMsgText.text = errorMsg;
			errorMsgText.DOFade (1, 1);        
		}

	}

	public void validateField(string inputname) {
		switch (inputname) {
			case "email":
				if (email.text == "") {
					emailImg.DOColor (errorColor, 1);
					errorMsg = "Vul al de velden correct in";
				} else {
					emailImg.DOColor (defaultColor, 1);
				}
				break;
			case "password":
				if (password.text == "") {
					passImg.DOColor (errorColor, 1);
					errorMsg = "Vul al de velden correct in";
				} else {
					checkPasswords ();
				}
				break;
			case "repassword":
				if (repassword.text == "") {
					passreImg.DOColor (errorColor, 1);
					errorMsg = "Vul al de velden correct in";
				} else {
					checkPasswords ();
				}
				break;
		}
	}

	private void checkPasswords() {
		if (password.text != "" && repassword.text != "") {
			if (password.text == repassword.text) {
				passImg.DOColor (defaultColor, 1);
				passreImg.DOColor (defaultColor, 1);
				errorMsgText.DOFade (0, 1);
			} else {
				passImg.DOColor (errorColor, 1);
				passreImg.DOColor (errorColor, 1);
				errorMsg = "Wachtwoorden komen niet overeen";
				errorMsgText.text = errorMsg;
				errorMsgText.DOFade (1, 1);
			}
		}
	}

	void OnCreateUser(bool success) {

		if (success) {              
			GamedoniaUsers.LoginUserWithEmail(email.text,password.text,OnLogin);                  
		}else {  
			Loader.I.disableLoader ();
			if (GamedoniaBackend.getLastError ().code == 10001) {
				emailImg.DOColor (errorColor, 1);
				errorMsgText.text = "Dit e-mailadres is al in gebruik";
				errorMsgText.DOFade (1, 1);
			}
		}

	}
	void OnLogin(bool success) {

		if (success) {          
			// Set playerprefs loggedIn to true so we dont need to log in again via http
			PlayerManager.I.player.loggedIn = true;
			PlayerManager.I.Save ();
			if (!PlayerManager.I.player.createdProfile) {
				Loader.I.LoadScene ("Profile_Create");
			} else {
				Loader.I.LoadScene ("Home");
			}    
		}else
        {
            //TODO
		}

	}

}
