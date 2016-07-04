using UnityEngine;

[System.Serializable]
public class Question {
	[SerializeField] public string q_Id;
	// Category related
	[SerializeField] public int cId;
	// Question title
	[SerializeField] public string qT;
	// Multiple choice answers
	[SerializeField] public string qA;
	[SerializeField] public string qB;
	[SerializeField] public string qC;
	[SerializeField] public string qD;
	// The correct answer
	[SerializeField] public string qCA;
	// Sender id
	[SerializeField] public string sID;
	
	public Question( string id, int cat, string question, string A, string B, string C, string D, string correct, string senderID) {
		q_Id = id;
		cId = cat;
		qT = question;
		qA = A;
		qB = B;
		qC = C;
		qD = D;
		qCA = correct;
		sID = senderID;
	}
}