using UnityEngine;
using System.Collections;

public class AudioManagerScript : MonoBehaviour {

    
    private AudioSource normalButtonSound;
    private AudioSource sliderButtonSound;

    // Use this for initialization

 

    void Start () {
        AudioSource[] a_sources = GetComponents<AudioSource>();
        normalButtonSound = a_sources[0];
        sliderButtonSound = a_sources[1];
      

	}

    //Normal button function
    public void OnButtonclick()
    {
        normalButtonSound.Play();
    }

    public void OnSliderClick()
    {
        sliderButtonSound.Play();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
