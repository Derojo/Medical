using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CountDownTimer : MonoBehaviour {

    public float timeRemaining = 15f;
    public Text timerText;
    private bool activateTime = false;

	// Use this for initialization
	void Start ()
    {
        //get the text component
        timerText = GetComponent<Text>();   
        timeRemaining = PlayerPrefs.GetFloat("remainingtime", 0.0f);
        
	}// end start

    public void OnButtonClick()
    {
        if(timeRemaining == 0)
        {
            timeRemaining = 15f;
        }
        activateTime = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (timeRemaining >= 0 && activateTime)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = timeRemaining.ToString("f0");
            print(timeRemaining);
        }

        if (timeRemaining <= 1)
        {
            timerText.text = ("Helaas, de tijd is voorbij");
            activateTime = false;
        }
    }// end update

    void OnDestroy()
    {
        timeRemaining = 0f;
        PlayerPrefs.SetFloat("remainingtime",timeRemaining);
        PlayerPrefs.Save();
    }
  
     
	
}
