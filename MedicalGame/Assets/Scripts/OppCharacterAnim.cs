using UnityEngine;
using System.Collections;

public class OppCharacterAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(QuestionManager.I.winAnim)
        {
            GetComponent<Animation>().CrossFade("Lose_Anim", 0.2f);
        }
        else if (QuestionManager.I.loseAnim)
        {
            GetComponent<Animation>().CrossFade("Win_Anim", 0.2f);
        }
        else
        {
           
            GetComponent<Animation>().CrossFade("Idle2_Anim", 0.2f);
            
        }
    }
}
