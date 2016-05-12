using UnityEngine;
using System.Collections;
using Gamedonia.Backend;
using System.Collections.Generic;


public class AddUser : MonoBehaviour {
	public string email;
	public string password;
	// Use this for initialization
	void Start () {

		// First create the credentials object for the account. In this example email credentials are used

		Credentials credentials = new Credentials();
		credentials.email = email;
		credentials.password = password;

		// Create the user account with some profile data and attach it to the credentials created in the previous block  
		GDUser user = new GDUser();
		user.credentials = credentials;
		user.profile["email"] = email;
		user.profile["name"] = "";
		user.profile["color"] = "";
		user.profile["hobby"] = "";
		user.profile["film"] = "";
		user.profile["age"] = 0;
		user.profile["lvl"] = 1;
		user.profile["wonAttr"] = 0;
		user.profile ["friends"] = new Dictionary<string, object> ();

		// Make the request to Gamedonia Backend to create the account and process the result in a block.   
		GamedoniaUsers.CreateUser(user, delegate (bool success){
			if (success){
				Debug.Log("Success");
			}
			else{
				Debug.Log("Fail");
			}
		});
	}
}
