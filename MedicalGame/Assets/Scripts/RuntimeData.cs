using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;

[Prefab("RuntimeData", true, "")]
public class RuntimeData : Singleton<RuntimeData> {

	public QuestionDatabase QuestionDatabase;

//	public List<Question> allQuestions;
	// Use this for initialization

	public bool Load() {return true;}

	void Start () {
		MatchManager.I.Load ();
		PlayerManager.I.Load ();
        AchievementManager.I.Load();
	}

	public void startMatch() {
		MatchManager.I.StartNewMatch ();
	}

}
