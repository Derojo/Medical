using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class CountDownTimer : MonoBehaviour {
	
    public float timeRemaining = 20f;
    public Text timerText;
	public Color timesUpColorOrange;
	public Color timesUpColorRed;
    private bool activateTime = false;
	private bool pulsating = false;

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
		StartCoroutine(WaitBeforeQuestionIsLoaded());

    }// end start
	
	private IEnumerator WaitBeforeQuestionIsLoaded() {
		while(!QuestionManager.I.questionReady) {
			yield return new WaitForSeconds(1f);
		}
		// Start the timer, question is loaded
		this.GetComponent<Image>().DOFade(1, 0.5f);
		image.DOFade(1, 0.5f);
		timerText.DOFade(1, 0.5f);
		activateTime = true;
        timeUpPopup.SetActive(false);
		
	}
	
	private void pulsateTimer() {

		if(!pulsating) {
			Sequence mySequence = DOTween.Sequence();
			mySequence.Append(gameObject.transform.DOScale(1.2f, 0.5f).SetLoops(-1).SetEase (Ease.OutBack));
			mySequence.Append(gameObject.transform.DOScale(1, 0.4f).SetLoops(-1).SetEase (Ease.OutQuint));
			mySequence.SetLoops(-1);
			pulsating = true;
		}
	}
	
	private void changeTextColor(Color toColor, float time) {
		timerText.DOColor(toColor, time);
	}
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
			if(timeRemaining < 10f)
			{
				pulsateTimer();
			}
			if(timeRemaining < 10f && timeRemaining > 5f) {
				changeTextColor(timesUpColorOrange, 5f);
			} else if(timeRemaining < 5f && timeRemaining > 0f) {

				changeTextColor(timesUpColorRed, 5f);
			}

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