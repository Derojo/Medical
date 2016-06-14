using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CountDownTimer : MonoBehaviour {

    public float timeRemaining = 20f;
    public Text timerText;
    private bool activateTime = false;

    // timer fill
    public float startTimer;
    public float timerPercent;
    public Image image;
    

    //TimesupPopUp
    public GameObject timeUpPopup;

	//QuestionManager
	public QuestionManager questionManager;

	// Use this for initialization
	void Start ()
    {
        startTimer = timeRemaining;
        activateTime = true;
        timeUpPopup.SetActive(false);
      
        //Get loaded stuff
        //  timeRemaining = PlayerPrefs.GetFloat("remainingtime", 0.0f);

    }// end start

    // Update is called once per frame
    void Update()
    {
        //activate timer
        if (timeRemaining >= 0 && activateTime)
        {
            timeRemaining -= Time.deltaTime;
            timerPercent = timeRemaining / startTimer;
            image.fillAmount = timerPercent;
            timerText.text = timeRemaining.ToString("f0");

        }
    
		if (timeRemaining <= 0 && activateTime)
        {
            activateTime = false;
			Loader.I.enableBackground ();
            timeUpPopup.SetActive(true);
            AudioManagerScript.I.TimeUpSound.Play();
			QuestionManager.I.checkAnswer ("");
        }
    }// end update
  
}