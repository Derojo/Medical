using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Gamedonia.Backend;
using System.Collections.Generic;

public class QuestionFactory : MonoBehaviour {
	
	private int currentStep = 1;
	public GameObject QuestionTitle;
	public GameObject AnswerA;
	public GameObject AnswerB;
	public GameObject AnswerC;
	public GameObject AnswerD;
	public GameObject CorrectAnswer;
	public GameObject Submit;
	public GameObject Smoke1;
	public GameObject Smoke2;
		
	public Dropdown categoryId;
	public Dropdown correctAnswerId;
	public Text QuestionTitleText;
	public Text AnswerAText;
	public Text AnswerBText;
	public Text AnswerCText;
	public Text AnswerDText;

	public RectTransform content;
	// Use this for initialization
	void Start () {

		
		Sequence smoke1 = DOTween.Sequence();
		smoke1.Append(Smoke1.transform.DOMoveY(230, 10f));
		smoke1.PrependInterval(2);
		smoke1.SetLoops(-1);
		smoke1.Insert(0, Smoke1.GetComponent<Image>().DOFade(0, 15f));
		smoke1.Insert(0, Smoke1.transform.DOScale(1.2f, 10f));
		
		Sequence smoke2 = DOTween.Sequence();
		smoke2.Append(Smoke2.transform.DOMoveY(230, 12f));
		smoke2.PrependInterval(4);
		smoke2.SetLoops(-1);
		smoke2.Insert(0, Smoke2.GetComponent<Image>().DOFade(0, 15f));
		smoke2.Insert(0, Smoke2.transform.DOScale(1.4f, 10f));
		


	}
	
	
	public void SetNextStep(int step) {
		if(step == 2) {
			QuestionTitle.SetActive(true);
			Debug.Log(categoryId.value);
		}
		if(step == 3) {
			AnswerA.SetActive(true);
		}
		if(step == 4) {
			AnswerB.SetActive(true);
		}
		if(step == 5) {
			AnswerC.SetActive(true);
			content.DOLocalMoveY (100f, 1f);
		}
		if(step == 6) {
			AnswerD.SetActive(true);
			content.DOLocalMoveY (200f, 1f);
		}
		if(step == 7) {
			CorrectAnswer.SetActive(true);
			content.DOLocalMoveY (300f, 1f);
		}
		if(step == 8) {
			Submit.SetActive(true);
			content.DOLocalMoveY (450f, 1f);
	
		}
	}
	
	public void SubmitQuestion() {
		Loader.I.enableLoader();
		string correctAnswer = "";
		if(correctAnswerId.value == 0) {
			correctAnswer = "A";
		} else if(correctAnswerId.value == 1) {
			correctAnswer = "B";
		} else if(correctAnswerId.value == 2) {
			correctAnswer = "C";
		} else if(correctAnswerId.value == 3) {
			correctAnswer = "D";
		}
		Dictionary<string,object> question = new Dictionary<string,object>();
		question["cId"] = (categoryId.value+1);
		question["qT"] = QuestionTitleText.text;
		question["qA"] = AnswerAText.text;
		question["qB"] = AnswerBText.text;
		question["qC"] = AnswerCText.text;
		question["qD"] = AnswerDText.text;
		question["qCA"] = correctAnswer;
		question["sID"] = PlayerManager.I.player.playerID;
		question["qAp"] = false;

		  
		// Make the request to store the entity inside the desired collection
		GamedoniaData.Create("questions", question, delegate (bool success, IDictionary data){
			if (success){
				Loader.I.LoadScene("QuestionAddedSuccess");
				//TODO Your success processing 
			}
			else{
				//TODO Your fail processing
			}
		});
	}
}
