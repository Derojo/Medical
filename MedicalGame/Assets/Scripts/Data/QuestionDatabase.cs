﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestionDatabase : ScriptableObject {
	[SerializeField]
	private List<Question> database;
	[SerializeField]
	private List<string> categories = new List<string>();
	
	void OnEnable() {
//		database = new List<Question>();
		if( database == null )
			database = new List<Question>();
	}
	
	public void Add( Question question ) {
		database.Add( question );
	}
	
	public void Remove( Question question ) {
		database.Remove( question );
	}
	
	public void RemoveAt( int index ) {
		database.RemoveAt( index );
	}
	
	public int COUNT {
		get { return database.Count; }
	}
	
	//.ElementAt() requires the System.Linq
	public Question Question( int index ) {
		return database.ElementAt( index );
	}
	
	public void SortAlphabeticallyAtoZ() {
		database.Sort((x, y) => string.Compare(x.q_Question, y.q_Question));
	}

	public string[] GetCategories() {
		return categories.ToArray ();
	}

	public int GetAnswerIndex(string answer) {
		switch (answer) {
			case "A":
				return 0;
			case "B":
				return 1;
			case "C":
				return 2;
			case "D":
				return 3;
			default:
				return 0;
		}

	}

		
}