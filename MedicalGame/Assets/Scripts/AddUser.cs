using UnityEngine;
using System.Collections;
using Gamedonia.Backend;

public class AddUser : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// First create the credentials object for the account. In this example email credentials are used

		Credentials credentials = new Credentials();
		credentials.email = "g.tempel@quiz.com";
		credentials.password = "gtempel";

		// Create the user account with some profile data and attach it to the credentials created in the previous block  

		GDUser user = new GDUser();
		user.credentials = credentials;
		user.profile["email"] = "g.tempel@quiz.com";
		user.profile["nickname"] = "Gijs Tempel";
		user.profile["age"] = 24;

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
