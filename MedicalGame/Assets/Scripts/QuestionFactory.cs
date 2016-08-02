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
	public InputField QuestionTitleText;
	public InputField AnswerAText;
	public InputField AnswerBText;
	public InputField AnswerCText;
	public InputField AnswerDText;

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
		Debug.Log(QuestionBackend.I.changeQuestion);
		if(QuestionBackend.I.changeQuestion) {
			Debug.Log("test");
			SetQuestionInformation();
		}

	}
	
	private void SetQuestionInformation() {
		QuestionTitle.SetActive(true);
		QuestionTitleText.text = QuestionBackend.I.currentQuestion.qT;
		categoryId.value = (QuestionBackend.I.currentQuestion.cId-1);
		AnswerA.SetActive(true);
		AnswerAText.text = QuestionBackend.I.currentQuestion.qA;
		AnswerB.SetActive(true);
		AnswerBText.text = QuestionBackend.I.currentQuestion.qB;
		AnswerC.SetActive(true);
		AnswerCText.text = QuestionBackend.I.currentQuestion.qC;
		AnswerD.SetActive(true);
		AnswerDText.text = QuestionBackend.I.currentQuestion.qD;
		CorrectAnswer.SetActive(true);
		int correctAnswer = 0;
		string cAnswer = QuestionBackend.I.currentQuestion.qCA;
		if(cAnswer == "A") {
			correctAnswer = 0;
		} else if(cAnswer == "B") {
			correctAnswer = 1;
		} else if(cAnswer == "C") {
			correctAnswer = 2;
		} else if(cAnswer == "D") {
			correctAnswer = 3;
		}
		correctAnswerId.value = correctAnswer;
		Submit.SetActive(true);
		Submit.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Vraag aanpassen";
		Submit.transform.GetChild(2).gameObject.SetActive(false);
		Submit.transform.GetChild(3).gameObject.SetActive(false);
		Submit.transform.GetChild(4).gameObject.SetActive(false);
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
		if(QuestionBackend.I.changeQuestion) {
			question["_id"] = QuestionBackend.I.currentQuestion.q_Id;
		}
		question["cId"] = (categoryId.value+1);
		question["qT"] = QuestionTitleText.text;
		question["qA"] = AnswerAText.text;
		question["qB"] = AnswerBText.text;
		question["qC"] = AnswerCText.text;
		question["qD"] = AnswerDText.text;
		question["qCA"] = correctAnswer;
		question["sID"] = PlayerManager.I.player.playerID;
		question["qAp"] = 0;
		
		// Add 50 exp to the player
		if(!QuestionBackend.I.changeQuestion) {
			PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 50;
		}
		  
		// Make the request to store the entity inside the desired collection
		if(!QuestionBackend.I.changeQuestion) {
			GamedoniaData.Create("questions", question, delegate (bool success, IDictionary data){
				if (success){
					Loader.I.LoadScene("QuestionAddedSuccess");
					//TODO Your success processing 
				}
				else{
					//TODO Your fail processing
				}
			});
		} else {
			QuestionBackend.I.changeQuestion = false;
			QuestionBackend.I.currentQuestion = null;
			GamedoniaData.Update("questions", question, delegate (bool success, IDictionary data){
				if (success){
					Loader.I.LoadScene("MyQuestions");
				} 
				else{
					//TODO Your fail processing
				}
			});
		}
	}
}
