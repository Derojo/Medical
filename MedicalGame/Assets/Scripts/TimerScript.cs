using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour {

    public float currentTimer;
    public float startTimer = 10.0f;
	// Use this for initialization
	void Awake ()
    {
        currentTimer = startTimer;
	}
	
	// Update is called once per frame
	void Update ()
    {
        currentTimer -= Time.deltaTime;
        Debug.Log(currentTimer);

        if(currentTimer <= 0)
        {
            Application.LoadLevel("OutOfTimeScene");
        }
	}


}
