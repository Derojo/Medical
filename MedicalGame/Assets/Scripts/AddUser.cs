using UnityEngine;
using System.Collections;
using Gamedonia.Backend;

public class AddUser : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// First create the credentials object for the account. In this example email credentials are used

		Credentials credentials = new Credentials();
		credentials.email = "r.bakker@quiz.com";
		credentials.password = "123";

		// Create the user account with some profile data and attach it to the credentials created in the previous block  

		GDUser user = new GDUser();
		user.credentials = credentials;
		user.profile["email"] = "r.bakker@quiz.com";
		user.profile["name"] = "";
		user.profile["color"] = "";
		user.profile["hobby"] = "";
		user.profile["film"] = "";
		user.profile["age"] = 0;

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
