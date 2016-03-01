using UnityEngine;
using System.Collections;

public class ButtonAudioScript : MonoBehaviour {

    public AudioSource buttonSound;
    public AudioSource menuButtonSound;

    // Audio for button pressed
    public void onButtonClick ()
    {
        buttonSound.Play();
	}

    //Audio for menu button pressed
    public void onMenuButtonClick()
    {
        menuButtonSound.Play();
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
