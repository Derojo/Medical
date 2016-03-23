using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class QuestionManager : Singleton<QuestionManager> {


	public Question currentQuestion;
	public QuestionDatabase questionDatabase;
	public Text CategoryTitle;
	public Text Question;
	public Button AnswerA;
	public Button AnswerB;
	public Button AnswerC;
	public Button AnswerD;
	public GameObject Continue;
	public List<Image> playerRounds = new List<Image> ();
	public Sprite goodAnswer;
	public Sprite wrongAnswer;
	public Sprite rightRound;
	public Sprite wrongRound;
	public Text playerScore;
	public Text playerName;
	public GameObject Timer;
	private int currentCategory;
	private string nextScene = "";
	private bool answeredQuestion = false;




	void Start() {
		currentCategory = MatchManager.Instance.currentCategory;
		// Get random question from current category
		currentQuestion = questionDatabase.getRandomCategoryQuestion(currentCategory);
		SetCategoryTitle ();
		SetQuestionReady ();
		SetTurnRounds ();
		playerName.text = PlayerPrefs.GetString("playerName");
	}

	public void checkAnswer(string Answer) {
		// Hide Timer
		if (Answer != "") {
			Timer.SetActive (false);
		}
		if (!answeredQuestion) {
			Button selectedAnswer = getButtonByAnswer (Answer);
			Button rightAnswer = getButtonByAnswer (currentQuestion.q_Correct);
			int newturnID = MatchManager.Instance.returnTurnId () + 1;
			Turn newTurn;
			if (Answer == currentQuestion.q_Correct) {
				// Set score
				playerScore.text = getScore().ToString();
				// Turn button color to green
				rightAnswer.GetComponent<Image> ().sprite = goodAnswer;
				rightAnswer.GetComponentInChildren<Text> ().color = Color.white;
				// Change progress question image
				playerRounds [MatchManager.Instance.returnTurnId ()].sprite = rightRound;
				// Change turn information -- Set player id to 1 - to be done: change to gamedonia player id
				newTurn = new Turn (newturnID, PlayerPrefs.GetString("playerID"), currentQuestion.q_Id, true);
				// Set next question string
				nextScene = "Category";
			} else {
				if (Answer != "") {
					// Show correct answer
					rightAnswer.GetComponent<Image> ().sprite = goodAnswer;
					rightAnswer.GetComponentInChildren<Text> ().color = Color.white;
					// Turn button color to red
					selectedAnswer.GetComponent<Image> ().sprite = wrongAnswer;
					selectedAnswer.GetComponentInChildren<Text> ().color = Color.white;
					// Change progress question image
					playerRounds [MatchManager.Instance.returnTurnId ()].sprite = wrongRound;
				}
				// Change turn information
				newTurn = new Turn (newturnID, PlayerPrefs.GetString("playerID"), currentQuestion.q_Id, false); 
				// Switch to home scene
				nextScene = "Home";
			}
			// Save new turn to match
			MatchManager.Instance.AddTurn (newTurn);
			if (Answer != "") {
				Continue.SetActive (true);
			}
		}
	}
		
	public void switchScene() {
		Loader.Instance.LoadScene (nextScene);
	}

	private Button getButtonByAnswer(string Answer) {
		Button returnButton;
		switch (Answer)
		{
			case "A":
				return AnswerA;
				break;
			case "B":
				return AnswerB;
				break;
			case "C":
				return AnswerC;
				break;
			case "D":
				return AnswerD;
				break;
			default :
				return null;
		}
	}
	private void changeButtonColor(Color color) {
		
	}

	private void SetCategoryTitle() {
		Debug.Log (currentCategory);

		CategoryTitle.text = Categories.getCategoryNameById(currentCategory);
	}

	private void SetQuestionReady() {
		Question.text = currentQuestion.q_Question;
		AnswerA.GetComponentInChildren<Text>().text = currentQuestion.q_AnswerA;
		AnswerB.GetComponentInChildren<Text>().text = currentQuestion.q_AnswerB;
		AnswerC.GetComponentInChildren<Text>().text = currentQuestion.q_AnswerC;
		AnswerD.GetComponentInChildren<Text>().text = currentQuestion.q_AnswerD;
	}

	private void SetTurnRounds() {
		float total = 0;
		Match match = MatchManager.Instance.GetMatch (MatchManager.Instance.currentMatchID);
		if (match.m_trns != null) {
			for (int i = 0; i < match.m_trns.Count; i++) {
				if (match.m_trns [i].t_st) {
					playerRounds [i].sprite = rightRound;
					total++;
				} else {
					playerRounds [i].sprite = wrongRound;
				}
			}
		}
		playerScore.text = total.ToString ();

	}

	private int getScore() {
		int total = 0;
		Match match = MatchManager.Instance.GetMatch (MatchManager.Instance.currentMatchID);
		if (match.m_trns != null) {
			for (int i = 0; i < match.m_trns.Count; i++) {
				if (match.m_trns [i].t_st) {
					total++;
				}
			}
		}
		return total;
	}


}