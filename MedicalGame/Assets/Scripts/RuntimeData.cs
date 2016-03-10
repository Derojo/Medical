using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RuntimeData : MonoBehaviour {

	public QuestionDatabase questions;
	public List<Question> allQuestions;
	// Use this for initialization

	void Awake() {
		allQuestions = questions.getAllQuestions();
	}
	void Start () {
		for (int cnt = 0; cnt < allQuestions.Count; cnt++) {
			Debug.Log (allQuestions [cnt].q_Question);
			Debug.Log (allQuestions [cnt].q_AnswerA);
			Debug.Log (allQuestions [cnt].q_AnswerB);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
