using UnityEngine;
using System.Collections;

public class CharacterAnim : MonoBehaviour
{
    public Animator animControl;

    // Update is called once per frame
    void Update ()
    {
	    if(QuestionManager.I.winAnim)
        {
            animControl.SetBool("IsWinning", true);
            //GetComponent<Animator>().CrossFade("Win_Anim", 0.2f);
        }
        else if (QuestionManager.I.loseAnim)
        {
            animControl.SetBool("IsLosing", true);
        }
       
    }
}
