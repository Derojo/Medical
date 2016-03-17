using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour {


	public Question currentQuestion;
	public QuestionDatabase questionDatabase;
	public Text CategoryTitle;
	public Text Question;
	public Button AnswerA;
	public Button AnswerB;
	public Button AnswerC;
	public Button AnswerD;
	public GameObject Continue;
	private int currentCategory;
	private string nextScene = "";


	void Start() {
//		currentCategory = MatchManager.Instance.currentCategory;
		currentCategory = 2;
		// Get random question from current category
		currentQuestion = questionDatabase.getRandomCategoryQuestion(currentCategory);
		SetCategoryTitle ();
		SetQuestionReady ();
	}

	public void checkAnswer(string Answer) {
		Button selectedAnswer = getButtonByAnswer (Answer);
		Button rightAnswer = getButtonByAnswer (currentQuestion.q_Correct);
		int newturnID = MatchManager.Instance.returnTurnId() + 1;
		Turn newTurn;
		if (Answer == currentQuestion.q_Correct) {
			// Turn button color to green
			selectedAnswer.GetComponent<Image> ().color = Color.green;
			// Change turn information -- Set player id to 1 - to be done: change to gamedonia player id
			newTurn = new Turn(newturnID, RuntimeData.Instance.LoggedInUser._id, currentQuestion.q_Id, true);
			// Set next question string
			nextScene = "Category";
		} else {
			// Show correct answer
			rightAnswer.GetComponent<Image> ().color = Color.green;
			// Turn button color to red
			selectedAnswer.GetComponent<Image> ().color = Color.red;
			// Change turn information
			newTurn = new Turn(newturnID, RuntimeData.Instance.LoggedInUser._id, currentQuestion.q_Id, false); 
			// Switch to home scene
			nextScene = "Home";
		}
		// Save new turn to match
		MatchManager.Instance.AddTurn(newTurn);
		Continue.SetActive (true);

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

}