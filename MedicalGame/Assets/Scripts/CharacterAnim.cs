using UnityEngine;
using System.Collections;

public class CharacterAnim : MonoBehaviour
{

	// Update is called once per frame
	void Update ()
    {
	    if(QuestionManager.I.winAnim)
        {
            GetComponent<Animation>().CrossFade("Win_Anim", 0.2f);
        }
        else if (QuestionManager.I.loseAnim)
        {
            GetComponent<Animation>().CrossFade("Lose_Anim", 0.2f);
        }
        else
        {
     
            GetComponent<Animation>().CrossFade("Idle1_Anim", 0.2f);    
        }
    }
}
