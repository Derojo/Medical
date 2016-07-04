using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gamedonia.Backend;

[Prefab("QuestionBackend", false, "")]
public class QuestionBackend : Singleton<QuestionBackend>   {

	private List<string> categories = new List<string>();
	public Question currentQuestion;
	public string[] questionIds;
	public bool questionLoaded = false;
		
	public Question setRandomQuestion(int categoryID){
		currentQuestion = null;
		//string[] questionIds = MatchManager.I.GetQuestionsInMatch ("5778fdfee4b006e8d75e6c3c");
		string[] questionIds = MatchManager.I.GetQuestionsInMatch ();
		

		Dictionary<string,object> parameters = new Dictionary<string, object>(){{"cId",categoryID}};
		parameters.Add("qIds", questionIds);

		GamedoniaScripts.Run("getrandomquestion", parameters, delegate (bool success, object data) {
			if (success) {
				Debug.Log(data);
				Dictionary<string,object> question = (Dictionary<string,object>)data;
				currentQuestion = new Question(
									question["_id"].ToString(), 
									int.Parse(question["cId"].ToString()),
									question["qT"].ToString(),
									question["qA"].ToString(),
									question["qB"].ToString(),
									question["qC"].ToString(),
									question["qD"].ToString(),
									question["qCA"].ToString(),
									question["sID"].ToString()
								);
			questionLoaded = true;
			} else {
				//TODO: the script throwed an error
			}
		});
		return currentQuestion;

	}

	public Question setQuestionById(string id) {
		currentQuestion = null;
			GamedoniaData.Search("question", "{_id:{$oid:'"+id+"'}}", delegate (bool success, IList data){
			if (success){
				//TODO Your success processing
			   if (data != null && data.Count == 1) {
					Dictionary<string, object> question = (Dictionary<string, object>) data[0];
					currentQuestion = new Question(
									question["_id"].ToString(), 
									int.Parse(question["cId"].ToString()),
									question["qT"].ToString(),
									question["qA"].ToString(),
									question["qB"].ToString(),
									question["qC"].ToString(),
									question["qD"].ToString(),
									question["qCA"].ToString(),
									question["sID"].ToString()
								);
					questionLoaded = true;
			   }
			}
			else {
				//TODO Your fail processing
			}
		});
		return currentQuestion;

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