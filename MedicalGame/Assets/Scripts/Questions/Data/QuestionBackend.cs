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
	public bool retrievedQuestions = false;
		
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
									question["sID"].ToString(),
									(question["qAp"].ToString() == "True" ? true : false)
								);
			questionLoaded = true;
			} else {
				questionLoaded = true;
				//TODO: the script throwed an error
			}
		});
		return currentQuestion;

	}

	public Question setQuestionById(string id) {
		currentQuestion = null;
			GamedoniaData.Search("questions", "{_id:{$oid:'"+id+"'}}", delegate (bool success, IList data){
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
									question["sID"].ToString(),
									(question["qAp"].ToString() == "True" ? true : false)
								);
					questionLoaded = true;
			   }
			   questionLoaded = true;
			}
			else {
				questionLoaded = true;
				//TODO Your fail processing
			}
		});
		return currentQuestion;

	}
	
	public List<Question> getQuestionsByPlayerId() {
		List<Question> questionList = new List<Question>();
			GamedoniaData.Search("questions", "{\"sID\":\""+PlayerManager.I.player.playerID+"\"}", delegate (bool success, IList data){
			if (success){
				//TODO Your success processing
			   if (data != null && data.Count > 0) {
				   
				   for (int i = 0; i < data.Count; i++) {
						Dictionary<string,object> questionD = (Dictionary<string, object>) data[i];
							Question question = new Question(
							questionD["_id"].ToString(), 
							int.Parse(questionD["cId"].ToString()),
							questionD["qT"].ToString(),
							questionD["qA"].ToString(),
							questionD["qB"].ToString(),
							questionD["qC"].ToString(),
							questionD["qD"].ToString(),
							questionD["qCA"].ToString(),
							questionD["sID"].ToString(),
							(questionD["qAp"].ToString() == "True" ? true : false)
						);
						questionList.Add(question);
					}


					retrievedQuestions = true;
			   }
			   retrievedQuestions = true;
			}
			else {
				retrievedQuestions = true;
			}
		});
		return questionList;
	}
	
	
	public List<Question> getNonApprovedQuestions() {
		List<Question> questionList = new List<Question>();
			GamedoniaData.Search("questions", "{\"qAp\":false}", delegate (bool success, IList data){
			if (success){
				//TODO Your success processing
			   if (data != null && data.Count > 0) {
				   
				   for (int i = 0; i < data.Count; i++) {
						Dictionary<string,object> questionD = (Dictionary<string, object>) data[i];
							Question question = new Question(
							questionD["_id"].ToString(), 
							int.Parse(questionD["cId"].ToString()),
							questionD["qT"].ToString(),
							questionD["qA"].ToString(),
							questionD["qB"].ToString(),
							questionD["qC"].ToString(),
							questionD["qD"].ToString(),
							questionD["qCA"].ToString(),
							questionD["sID"].ToString(),
							(questionD["qAp"].ToString() == "True" ? true : false)
						);
						questionList.Add(question);
					}


					retrievedQuestions = true;
			   }
			   retrievedQuestions = true;
			}
			else {
				retrievedQuestions = true;
			}
		});
		return questionList;
	}

	public void ApproveQuestion(string questionID) {
		Dictionary<string,object> question = new Dictionary<string,object>();
		question["_id"] = questionID;
		question["qAp"] = true;
		GamedoniaData.Update("questions", question, delegate (bool success, IDictionary data){
			if (success){
				//TODO Your success processing
			} 
			else{
				//TODO Your fail processing
			}
		});
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