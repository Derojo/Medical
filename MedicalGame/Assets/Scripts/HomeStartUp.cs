using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomeStartUp : MonoBehaviour {



	// Use this for initialization
	void Start ()
    {
        //Earning first achievement
        AchievementManager.I.checkAchievementConnect();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
