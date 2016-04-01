using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class QuestionManager : Singleton<QuestionManager> {


	public Question currentQuestion;
	public QuestionDatabase questionDatabase;
	public Text CategoryTitle;
	public Text Question;
	public Button AnswerA;
	public Button AnswerB;
	public Button AnswerC;
	public Button AnswerD;
	public GameObject Continue;
    public GameObject XPPopUp;
    public List<Image> playerRounds = new List<Image> ();
	public Sprite goodAnswer;
	public Sprite wrongAnswer;
	public Sprite rightRound;
	public Sprite wrongRound;
	public Text playerScore;
	public Text playerName;
	public Image playerRankImg;
	public GameObject Timer;
	public Text xpText;
	public GameObject xpCoins;
	private int currentCategory;
	private string nextScene = "";
	private bool answeredQuestion = false;




	void Start() {
		currentCategory = MatchManager.I.currentCategory;
		// Get random question from current category
		currentQuestion = questionDatabase.getRandomCategoryQuestion(currentCategory);
		SetCategoryTitle ();
		SetQuestionReady ();
		SetTurnRounds ();
		playerName.text = PlayerManager.I.player.profile.name;
		playerRankImg.sprite = PlayerManager.I.GetRankSprite ();
	}

	public void checkAnswer(string Answer) {
       
        // Hide Timer
        if (Answer != "") {
			Timer.SetActive (false);
		}
		if (!answeredQuestion) {
			Button selectedAnswer = getButtonByAnswer (Answer);
			Button rightAnswer = getButtonByAnswer (currentQuestion.q_Correct);
			int newturnID = MatchManager.I.returnTurnId () + 1;
			Turn newTurn;
			if (Answer == currentQuestion.q_Correct)
            {
                //Tweening
                foreach (Text text in XPPopUp.GetComponentsInChildren<Text>())
                {
                    text.DOFade(1, 0.3f);
                    text.DOFade(0, 0.2f).SetDelay(0.5f);
                }
                //set XP
                PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 10;
				showBrainCoinTween (1, 10);
                // Set score
                playerScore.text = (getScore()+1).ToString();
                // Turn button color to green
                rightAnswer.GetComponent<Image> ().sprite = goodAnswer;
				rightAnswer.GetComponentInChildren<Text> ().color = Color.white;
				// Change progress question image
				playerRounds [MatchManager.I.returnTurnId ()].sprite = rightRound;
				// Change turn information -- Set player id to 1 - to be done: change to gamedonia player id
				newTurn = new Turn (newturnID, PlayerManager.I.player.playerID, currentQuestion.q_Id, true);
				// Set next question string
				nextScene = "Category";

                //total questions answered right counter
                PlayerManager.I.player.rightAnswersTotal  ++;
                //Row questions answered right counter
                PlayerManager.I.player.rightAnswersRow ++;

                //Row questions XP count
                if (PlayerManager.I.player.rightAnswersRow == 3)
                {
                    PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 20;
					showBrainCoinTween (2, 20);
                }
                if (PlayerManager.I.player.rightAnswersRow == 6)
                {
                    PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 50;
					showBrainCoinTween (3, 50);
                }
                if (PlayerManager.I.player.rightAnswersRow == 9)
                {
                    PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 100;
					showBrainCoinTween (4, 100);
                }

                //total right questions in TV_Entertainment
                if (currentCategory ==1)
                {  
                    PlayerManager.I.player.entertainmentAnswers  ++;
                }

                //total right questions in Geloof_Cultuur
                if (currentCategory == 2)
                {
                    PlayerManager.I.player.religionAnswers++;
                }
                //total right questions in Zorg_wetenschap
                if (currentCategory == 3)
                {
                    PlayerManager.I.player.careAnswers ++;
                }
                //total right questions in Geschiedenis
                if (currentCategory == 4)
                {
                    PlayerManager.I.player.historyAnswers++;
                }
                //total right questions in Sport
                if (currentCategory == 5)
                {

                    PlayerManager.I.player.sportAnswers++;
                }
                //total right questions in Geografie
                if (currentCategory == 6)
                {

                    PlayerManager.I.player.geographicAnswers++;
                }
            } else {
				if (Answer != "")
                {      
                    // Show correct answer
                    rightAnswer.GetComponent<Image> ().sprite = goodAnswer;
					rightAnswer.GetComponentInChildren<Text> ().color = Color.white;
					// Turn button color to red
					selectedAnswer.GetComponent<Image> ().sprite = wrongAnswer;
					selectedAnswer.GetComponentInChildren<Text> ().color = Color.white;
					// Change progress question image
					playerRounds [MatchManager.I.returnTurnId ()].sprite = wrongRound;
				}
                PlayerManager.I.player.rightAnswersRow = 0;
                // Change turn information
                newTurn = new Turn (newturnID, PlayerManager.I.player.playerID, currentQuestion.q_Id, false); 
				// Switch to home scene
				nextScene = "Home";
			}

            // check for completed achievements
            AchievementManager.I.checkAchievementAfterAnswer();
           
            // Save new turn to match
            MatchManager.I.AddTurn (newTurn);
			if (Answer != "")
            {
				Continue.SetActive (true);    
			}
		}
	}
		
	public void switchScene()
    {
        if(nextScene == "Home")
        {
            PlayerManager.I.player.rightAnswersRow = 0;
        }
		Loader.Instance.LoadScene (nextScene);
	}
    
	private Button getButtonByAnswer(string Answer) {
		Button returnButton;
		switch (Answer)
		{
			case "A":
				return AnswerA;
				break;
			case "B":
				return AnswerB;
				break;
			case "C":
				return AnswerC;
				break;
			case "D":
				return AnswerD;
				break;
			default :
				return null;
		}
	}

	private void SetCategoryTitle() {
		CategoryTitle.text = Categories.getCategoryNameById(currentCategory);
	}

	private void SetQuestionReady() {
		Question.text = currentQuestion.q_Question;
		AnswerA.GetComponentInChildren<Text>().text = currentQuestion.q_AnswerA;
		AnswerB.GetComponentInChildren<Text>().text = currentQuestion.q_AnswerB;
		AnswerC.GetComponentInChildren<Text>().text = currentQuestion.q_AnswerC;
		AnswerD.GetComponentInChildren<Text>().text = currentQuestion.q_AnswerD;
	}

	private void SetTurnRounds() {
		float total = 0;
		Match match = MatchManager.I.GetMatch (MatchManager.I.currentMatchID);
		if (match.m_trns != null) {
			for (int i = 0; i < match.m_trns.Count; i++) {
				if (match.m_trns [i].t_st) {
					playerRounds [i].sprite = rightRound;
					total++;
				} else {
					playerRounds [i].sprite = wrongRound;
				}
			}
		}
		playerScore.text = total.ToString ();

	}

	private int getScore() {
		int total = 0;
		Match match = MatchManager.I.GetMatch (MatchManager.I.currentMatchID);
		if (match.m_trns != null) {
			for (int i = 0; i < match.m_trns.Count; i++) {
				if (match.m_trns [i].t_st) {
					total++;
				}
			}
		}
		return total;
	}

	private void showBrainCoinTween(int numbers, int coinValue) {
		switch (numbers) 
		{
		case 1:
			// Tween 1 coin
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 70f, 0);
			break;
		case 2:
			// Tween 2 coins
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 70f, 0);
			popupCoin(xpCoins.transform.GetChild (1).GetComponent<RectTransform> (), xpCoins.transform.GetChild (1).GetComponent<Image> (), xpCoins.transform.GetChild (1).GetComponent<AudioSource> (), 80f, 0.1f);
			break;
		case 3:
			// Tween 3 coins
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 70f, 0);
			popupCoin(xpCoins.transform.GetChild (1).GetComponent<RectTransform> (), xpCoins.transform.GetChild (1).GetComponent<Image> (), xpCoins.transform.GetChild (1).GetComponent<AudioSource> (), 80f, 0.1f);
			popupCoin(xpCoins.transform.GetChild (2).GetComponent<RectTransform> (), xpCoins.transform.GetChild (2).GetComponent<Image> (), xpCoins.transform.GetChild (2).GetComponent<AudioSource> (), 80f, 0.2f);
			break;
		case 4:
			// Tween 4 coins
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 70f, 0);
			popupCoin(xpCoins.transform.GetChild (1).GetComponent<RectTransform> (), xpCoins.transform.GetChild (1).GetComponent<Image> (), xpCoins.transform.GetChild (1).GetComponent<AudioSource> (), 80f, 0.1f);
			popupCoin(xpCoins.transform.GetChild (2).GetComponent<RectTransform> (), xpCoins.transform.GetChild (2).GetComponent<Image> (), xpCoins.transform.GetChild (2).GetComponent<AudioSource> (), 80f, 0.2f);
			popupCoin(xpCoins.transform.GetChild (3).GetComponent<RectTransform> (), xpCoins.transform.GetChild (3).GetComponent<Image> (), xpCoins.transform.GetChild (3).GetComponent<AudioSource> (), 70f, 0.3f);
			break;
		default:
			break;
		}

		xpCoins.GetComponent<AudioSource> ().Play();
		xpText.text = "+ "+coinValue.ToString();
		xpText.DOFade (1, 1f).SetDelay(0f);
		xpText.DOFade (0, 1f).SetDelay(1f);
		
	}

	private void popupCoin(RectTransform rect, Image img, AudioSource audio,  float height, float delay=0) {

		audio.PlayDelayed (delay);
		audio.DOFade (1, .3f).SetDelay (delay);
		rect.DOLocalMoveY ((rect.rect.y+height), 1f).SetEase (Ease.OutElastic).SetDelay (delay);
		rect.DOScale (1.5f, 2f).SetEase (Ease.OutElastic).SetDelay (delay);
		img.DOFade(0, .5f).SetEase(Ease.InExpo).SetDelay(delay+1);
		rect.DOLocalMoveY ((rect.rect.y+10), .5f).SetEase(Ease.OutSine).SetDelay(delay+1);
		// Show experience gained
	}
}