using UnityEngine;
using System.Collections;

public class AudioManagerScript : MonoBehaviour {

    private AudioSource menuButtonSound;
    private AudioSource normalButtonSound;

    // Use this for initialization

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start () {
        AudioSource[] a_sources = GetComponents<AudioSource>();
        menuButtonSound = a_sources[0];
        normalButtonSound = a_sources[1];

	}
    //Menu button function
    public void OnMenuclick()
    {
        menuButtonSound.Play();
    }

    //Normal button function
    public void OnButtonclick()
    {
        normalButtonSound.Play();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
