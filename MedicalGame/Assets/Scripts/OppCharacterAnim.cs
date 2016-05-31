using UnityEngine;
using System.Collections;

public class OppCharacterAnim : MonoBehaviour {

    public Animator animControl;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(QuestionManager.I.winAnim)
        {
            animControl.SetBool("IsLosing", true);
        }
        else if (QuestionManager.I.loseAnim)
        {
            animControl.SetBool("IsWinning", true);
        }
    }
}
