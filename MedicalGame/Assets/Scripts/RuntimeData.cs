using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;

public class RuntimeData : Singleton<RuntimeData> {

	public QuestionDatabase QuestionDatabase;
//	public List<Question> allQuestions;
	// Use this for initialization



	void Start () {
		MatchManager.Instance.Load ();

		Debug.Log(PlayerPrefs.GetString("playerID"));
//		Loader.Instance.Load ();
//		MatchManager.Instance.returnAllMatches();
//		for (int cnt = 0; cnt < allQuestions.Count; cnt++) {
//			Debug.Log (allQuestions [cnt].q_Question);
//			Debug.Log (allQuestions [cnt].q_AnswerA);
//			Debug.Log (allQuestions [cnt].q_AnswerB);
//		}
	}
	public void startMatch() {
		MatchManager.Instance.StartNewMatch ();
	}
	// Update is called once per frame
	void Update () {
	
	}

}
