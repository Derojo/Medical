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
	public GameObject toHome;
	private int currentCategory;


	void Start() {
		currentCategory = MatchManager.Instance.currentCategory;
//		// Get random question from current category
		currentQuestion = questionDatabase.getRandomCategoryQuestion(currentCategory);
		SetCategoryTitle ();
		SetQuestionReady ();
	}

	public void checkAnswer(string Answer) {
		Button selectedAnswer = getButtonByAnswer (Answer);
		Button rightAnswer = getButtonByAnswer (currentQuestion.q_Correct);
		if (Answer == currentQuestion.q_Correct) {
			selectedAnswer.GetComponent<Image> ().color = Color.green;
			// Turn button color to green

			// Change turn information

			// Continue to next question
		} else {
			// Show correct answer
			rightAnswer.GetComponent<Image> ().color = Color.green;
			selectedAnswer.GetComponent<Image> ().color = Color.red;
			// Change turn information

			// Turn button color to red

			// Switch to home scene
		}
		toHome.SetActive (true);
	}
	public void ToHome() {
		Loader.Instance.LoadScene ("Home");
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
