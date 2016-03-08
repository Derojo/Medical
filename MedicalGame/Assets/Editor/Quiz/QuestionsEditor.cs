using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
public class QuestionsEditor : EditorWindow {
	private enum State
	{
		DEFAULT,
		EDIT,
		ADD
	}
	
	private State state;
	private int selectedQuestion;
	private string newQuestion;
	private int newQuestionCat;
	// Answers
	private string newAnswerA;
	private string newAnswerB;
	private string newAnswerC;
	private string newAnswerD;
	private int newCorrectAnswerIndex;
	private string newCorrectAnswer;

	private const string DATABASE_PATH = @"Assets/Database/questions.asset";
	private QuestionDatabase questions;
	private Vector2 _scrollPos;

	private bool toggleOn = false;

	GUIStyle boxStyle;
	GUIStyle editStyle;
	GUIStyle centerButton;
	GUIStyle correctStyle;
	GUIStyle incorrectStyle;

	[MenuItem("Quiz/Database/Questions %#w")]
	public static void Init() {
		QuestionsEditor window = EditorWindow.GetWindow<QuestionsEditor>();
		window.minSize = new Vector2(800, 400);
		window.Show();
	}
	
	void OnEnable() {
		if (questions == null)
			LoadDatabase();
		state = State.DEFAULT;
	}
	
	void OnGUI() {
		boxStyle = new GUIStyle();
		boxStyle.margin = new RectOffset(10, 10, 10, 10);
		boxStyle.padding = new RectOffset (10, 10, 10, 10);
		boxStyle.normal.background = MakeTex( 2, 2, new Color( 0.8f, 0.8f, 0.8f, 1f ) );

		editStyle = new GUIStyle();
		editStyle.margin = new RectOffset(10, 10, 10, 10);
		editStyle.padding = new RectOffset (10, 10, 10, 10);
		editStyle.normal.background = MakeTex( 2, 2, new Color( 0.8f, 0.8f, 0.8f, 1f ) );

		centerButton = new GUIStyle (EditorStyles.largeLabel);
		centerButton.alignment = TextAnchor.MiddleCenter;

		correctStyle = new GUIStyle(GUI.skin.textField);
		correctStyle.normal.background = MakeTex( 2, 2, new Color( 0, 0.9f, 0, 1f ) );
		correctStyle.active.background = MakeTex( 2, 2, new Color( 0, 0.9f, 0, 1f ) );
		correctStyle.hover.background = MakeTex( 2, 2, new Color( 0, 0.9f, 0, 1f ) );
		correctStyle.focused.background = MakeTex( 2, 2, new Color( 0, 0.9f, 0, 1f ) );

		incorrectStyle = new GUIStyle(GUI.skin.textField);
		incorrectStyle.normal.background = MakeTex( 2, 2, new Color( 0.9f, 0, 0, 0.4f ) );
		incorrectStyle.active.background = MakeTex( 2, 2, new Color(0.9f, 0, 0, 0.4f  ) );
		incorrectStyle.hover.background = MakeTex( 2, 2, new Color( 0.9f, 0, 0, 0.4f ) );
		incorrectStyle.focused.background = MakeTex( 2, 2, new Color( 0.9f, 0, 0, 0.4f ) );
		EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
		DisplayListArea();
		EditorGUILayout.EndHorizontal();
	}
	
	void LoadDatabase() {
		questions = (QuestionDatabase)AssetDatabase.LoadAssetAtPath(DATABASE_PATH, typeof(QuestionDatabase));
		
		if (questions == null)
			CreateDatabase();
	}
	
	void CreateDatabase() {
		questions = ScriptableObject.CreateInstance<QuestionDatabase>();
		AssetDatabase.CreateAsset(questions, DATABASE_PATH);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}
	
	void DisplayListArea() {
		EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
		EditorGUILayout.Space();
		
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, "box", GUILayout.ExpandHeight(true));
		
		for (int cnt = 0; cnt < questions.COUNT; cnt++)
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("-", GUILayout.Width(25)))
			{
				questions.RemoveAt(cnt);
				questions.SortAlphabeticallyAtoZ();
				EditorUtility.SetDirty(questions);
				state = State.DEFAULT;
				return;
			}
			string category = questions.GetCategories()[questions.Question(cnt).q_Cat];


			GUILayout.Label (category, EditorStyles.helpBox, GUILayout.Width (120));
			if (GUILayout.Button (questions.Question (cnt).q_Question, GUILayout.ExpandWidth (true))) {
				selectedQuestion = cnt;
				if (!toggleOn) {
					toggleOn = true;
					state = State.EDIT;
				} else {
					toggleOn = false;
					state = State.DEFAULT;
				}
			}

//			} else {
//				if (questions.Question (selectedQuestion).q_Id == questions.Question (cnt).q_Id) {
//					questions.Question (selectedQuestion).q_Cat = EditorGUILayout.Popup (questions.Question (selectedQuestion).q_Cat, questions.GetCategories (), GUILayout.Width (120));
//					questions.Question (selectedQuestion).q_Question = EditorGUILayout.TextField (questions.Question (selectedQuestion).q_Question, GUILayout.ExpandWidth (true));
//					if (GUILayout.Button ("save", GUILayout.Width (50))) {
//						toggleOn = false;
//						state = State.DEFAULT;
//					}
//				} else {
//					if (GUILayout.Button (questions.Question (cnt).q_Question, GUILayout.ExpandWidth (true))) {
//						selectedQuestion = cnt;
//						if (!toggleOn) {
//							toggleOn = true;
//							state = State.EDIT;
//						} else {
//							toggleOn = false;
//							state = State.DEFAULT;
//						}
//					}
//				}
//			}
			
			EditorGUILayout.EndHorizontal();
		}
		if (state == State.EDIT) {
			EditorGUILayout.BeginVertical(editStyle);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField ("Categorie:", GUILayout.Width (145));
			questions.Question(selectedQuestion).q_Cat = EditorGUILayout.Popup(questions.Question(selectedQuestion).q_Cat, questions.GetCategories(), EditorStyles.popup);
			EditorGUILayout.EndHorizontal();
			questions.Question(selectedQuestion).q_Question = EditorGUILayout.TextField(new GUIContent("Vraag: "), questions.Question(selectedQuestion).q_Question);
			EditorGUILayout.Space ();
			questions.Question(selectedQuestion).q_AnswerA = EditorGUILayout.TextField(new GUIContent("A:"), questions.Question(selectedQuestion).q_AnswerA , (questions.Question(selectedQuestion).q_Correct == "A" ? correctStyle : incorrectStyle),  GUILayout.ExpandWidth(true));
			questions.Question(selectedQuestion).q_AnswerB = EditorGUILayout.TextField(new GUIContent("B:"), questions.Question(selectedQuestion).q_AnswerB , (questions.Question(selectedQuestion).q_Correct == "B" ? correctStyle : incorrectStyle),  GUILayout.ExpandWidth(true));
			questions.Question(selectedQuestion).q_AnswerC = EditorGUILayout.TextField(new GUIContent("C:"), questions.Question(selectedQuestion).q_AnswerC , (questions.Question(selectedQuestion).q_Correct == "C" ? correctStyle : incorrectStyle),  GUILayout.ExpandWidth(true));
			questions.Question(selectedQuestion).q_AnswerD = EditorGUILayout.TextField(new GUIContent("D:"), questions.Question(selectedQuestion).q_AnswerD , (questions.Question(selectedQuestion).q_Correct == "D" ? correctStyle : incorrectStyle),  GUILayout.ExpandWidth(true));
				EditorGUILayout.LabelField ("Goede antwoord:", EditorStyles.centeredGreyMiniLabel);
				string[] answers = new string[]{ "A", "B", "C", "D" };
			int index = questions.GetAnswerIndex (questions.Question (selectedQuestion).q_Correct);
			index = EditorGUILayout.Popup (index, answers, EditorStyles.popup);
			questions.Question(selectedQuestion).q_Correct = answers[index];
			if (GUILayout.Button ("save", GUILayout.Width (50))) {
				toggleOn = false;
				state = State.DEFAULT;
			}
			EditorGUILayout.EndVertical ();
		}

		if (state == State.ADD) {
			EditorGUILayout.BeginVertical(boxStyle);
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField ("Categorie:", GUILayout.Width (145));
				newQuestionCat = EditorGUILayout.Popup(newQuestionCat, questions.GetCategories(), EditorStyles.popup);
			EditorGUILayout.EndHorizontal();
			newQuestion = EditorGUILayout.TextField(new GUIContent("Vraag: "), newQuestion);
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField ("Antwoorden:", EditorStyles.centeredGreyMiniLabel);
			newAnswerA = EditorGUILayout.TextField(new GUIContent("A:"), newAnswerA, (newCorrectAnswer != "" ? (newCorrectAnswer == "A" ? correctStyle : incorrectStyle) : EditorStyles.textField), GUILayout.ExpandWidth(true));
			newAnswerB = EditorGUILayout.TextField(new GUIContent("B:"), newAnswerB, (newCorrectAnswer != "" ? (newCorrectAnswer == "B" ? correctStyle : incorrectStyle) : EditorStyles.textField), GUILayout.ExpandWidth(true));
			newAnswerC = EditorGUILayout.TextField(new GUIContent("C:"), newAnswerC, (newCorrectAnswer != "" ? (newCorrectAnswer == "C" ? correctStyle : incorrectStyle) : EditorStyles.textField), GUILayout.ExpandWidth(true));
			newAnswerD = EditorGUILayout.TextField(new GUIContent("D:"), newAnswerD, (newCorrectAnswer != "" ? (newCorrectAnswer == "D" ? correctStyle : incorrectStyle) : EditorStyles.textField), GUILayout.ExpandWidth(true));
			EditorGUILayout.Space ();
			if (newAnswerA != "" && newAnswerB != "" && newAnswerC != "" && newAnswerD != "") {
	
				EditorGUILayout.LabelField ("Goede antwoord:", EditorStyles.centeredGreyMiniLabel);
				string[] answers = new string[]{ "A", "B", "C", "D" };
				newCorrectAnswerIndex = EditorGUILayout.Popup (newCorrectAnswerIndex, answers, EditorStyles.popup);
				newCorrectAnswer = answers [newCorrectAnswerIndex];
			}
	
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Save", centerButton)) {
				if (newQuestion != "" && newAnswerA != "" && newAnswerB != "" && newAnswerC != "" && newAnswerD != "") {
					questions.Add (new Question (questions.COUNT, newQuestionCat, newQuestion, newAnswerA, newAnswerB, newAnswerC, newAnswerD, newCorrectAnswer));
					newQuestion = string.Empty;
					EditorUtility.SetDirty (questions);
					clearFields ();
					state = State.DEFAULT;
				}
			} else if (GUILayout.Button ("Cancel", centerButton)) {
				clearFields ();
				state = State.DEFAULT;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		EditorGUILayout.EndScrollView();


		EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
		EditorGUILayout.LabelField("Vragen: " + questions.COUNT, GUILayout.Width(100));
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("+", GUILayout.Width(25)))
			state = State.ADD;
		
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();
	}

	
	private void clearFields() {
		newAnswerA = "";
		newAnswerB = "";
		newAnswerC = "";
		newAnswerD = "";
		newQuestion = "";
		newQuestionCat = 0;
		newCorrectAnswerIndex = 0;
		newCorrectAnswer = "";
	}

	private Texture2D MakeTex( int width, int height, Color col )
	{
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix [i] = col;
		}
		Texture2D result = new Texture2D (width, height);
		result.SetPixels (pix);
		result.Apply ();
		return result;
	}
}