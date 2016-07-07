using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Prefab("SoundContainer", true, "")]
public class AudioManagerScript : Singleton<AudioManagerScript>
{
    //sound variables
    public AudioSource wrongAnwserSound;
    public AudioSource achievementUnlockSound;
    public AudioSource TimeUpSound;
    public AudioSource lvlUpSound;
	public AudioSource bell;


    public static bool pause;
 
    public bool Load() { return true; }

    void Start()
    {
        AudioSource[] a_sources = GetComponents<AudioSource>();
        wrongAnwserSound = a_sources[0];
		achievementUnlockSound = a_sources[1];
        TimeUpSound = a_sources[2];
        lvlUpSound = a_sources[3];
		bell = a_sources[4];
    }

    public void pauseAudio()
  {
      Debug.Log(pause);
      if(AudioListener.pause)
      {
          AudioListener.pause = false;
      }
      else  AudioListener.pause = true;
  }

}
