using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Popcorn : MonoBehaviour
{
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        audio.Play(46100);
    }
}