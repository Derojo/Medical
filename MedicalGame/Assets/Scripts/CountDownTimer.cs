using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CountDownTimer : MonoBehaviour {

    public float timeRemaining = 15f;
    public Text timerText;
    private bool activateTime = false;

    // timer fill
    public float startTimer;
    public float timerPercent;
    public Image image;
   

    //TimesupPopUp
    public GameObject timeUpPopup;

	// Use this for initialization
	void Start ()
    {
        startTimer = timeRemaining;
        activateTime = true;
        timeUpPopup.SetActive(false);
      
        //Get loaded stuff
        //  timeRemaining = PlayerPrefs.GetFloat("remainingtime", 0.0f);

    }// end start

    //resetting timer
   /* public void OnButtonClick()
    {
        //resetting timer -> normally back to main menu
        if (timeRemaining <= 0)
        {
            timeRemaining = 15f;
            timerText.text = "";
        }
        activateTime = true;
        timeUpPopup.SetActive(false);
    }*/

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining >= 0 && activateTime)
        {
            timeRemaining -= Time.deltaTime;
            timerPercent = timeRemaining / startTimer;
            image.fillAmount = timerPercent;
            timerText.text = timeRemaining.ToString("f0");
            print(timeRemaining);
        }

        if (timeRemaining <= 0)
        {
            //timerText.text = "Helaas, de tijd is voorbij";
            activateTime = false;
            timeUpPopup.SetActive(true);
        }
    }// end update

    /*void OnDestroy()
    {
        //onn destroy timer to 0 and save
        timeRemaining = 0f;
        PlayerPrefs.SetFloat("remainingtime",timeRemaining);
        PlayerPrefs.Save();
    }*/
  
     
	
}
