using UnityEngine;

[System.Serializable]
public class Question {
	[SerializeField] public int q_Id;
	// Category related
	[SerializeField] public int q_Cat;
	// Question line
	[SerializeField] public string q_Question;
	// Multiple choice answers
	[SerializeField] public string q_AnswerA;
	[SerializeField] public string q_AnswerB;
	[SerializeField] public string q_AnswerC;
	[SerializeField] public string q_AnswerD;
	// The correct answer
	[SerializeField] public string q_Correct;
	// Question experience ( for later use )
	[SerializeField] public float q_ExpPoints = 0;
	
	public Question( int id, int cat, string question, string A, string B, string C, string D, string correct, float points = 0) {
		q_Id = id;
		q_Cat = cat;
		q_Question = question;
		q_AnswerA = A;
		q_AnswerB = B;
		q_AnswerC = C;
		q_AnswerD = D;
		q_Correct = correct;
		q_ExpPoints = points;
	}
}