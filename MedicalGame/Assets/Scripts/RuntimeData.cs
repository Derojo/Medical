using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Gamedonia.Backend;

[Prefab("RuntimeData", true, "")]
public class RuntimeData : Singleton<RuntimeData> {

	public QuestionDatabase QuestionDatabase;
	public PlayerDB PDB;

//	public List<Question> allQuestions;
	// Use this for initialization

	public bool Load() {return true;}

	void Start () {
//		PlayerDatabase.changeValue ();
//		Debug.Log (PlayerDatabase.testDictionary ["test1"]);
		MatchManager.I.Load ();
//		Debug.Log (PDB.profile [0].age);
//		PDB.profile [0].age = 24;
	}

	public void startMatch() {
		MatchManager.I.StartNewMatch ();
	}

}
