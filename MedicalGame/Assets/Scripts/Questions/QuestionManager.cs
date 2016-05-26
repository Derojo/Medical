using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using LitJson_Gamedonia;

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
    public GameObject continueToEnd;
    public GameObject XPPopUp;
    public bool winAnim = false;
    public bool loseAnim = false;

	public Sprite goodAnswer;
	public Sprite wrongAnswer;
	public Sprite rightRound;
	public Sprite wrongRound;

	// Player information
	public GameObject playerTurns;
	public Text playerScore;
	public Text playerName;
	public Image playerRankImg;

	// Opponent information
	public Text oppScore;
	public Text oppName;
	public Image oppRankImg;
	public GameObject oppTurns;

	public GameObject Timer;
	public Text xpText;
	public GameObject xpCoins;
	private int currentCategory;
	private string nextScene = "";
	private bool answeredQuestion = false;
	private string opponentId;
	private List<Turn> playerTurnL = new List<Turn>();
	private List<Turn> oppTurnL = new List<Turn>();



	void Start()
    {
        //allowing answers
        answeredQuestion = false;

        // Match initialization 
        Match currentMatch = MatchManager.I.GetMatch (MatchManager.I.currentMatchID);
		opponentId = MatchManager.I.GetOppenentId (currentMatch);

		if (opponentId != "") {
			PlayerManager.I.GetPlayerInformationById (opponentId);
            winAnim = false;
            loseAnim = false;		
		}
		// Turn lists
		currentCategory = MatchManager.I.currentCategory;
		if (currentMatch.m_trns != null && currentMatch.m_trns.Count > 0) {
			playerTurnL = MatchManager.I.GetMatchTurnsByPlayerID (PlayerManager.I.player.playerID, currentMatch);
			oppTurnL = MatchManager.I.GetMatchTurnsByPlayerID (MatchManager.I.GetOppenentId(currentMatch), currentMatch);
			if (oppTurnL.Count > playerTurnL.Count) {

				// Opponent played more turns, get his last turn
				for (int i = 0; i < oppTurnL.Count; i++) {
					if (oppTurnL [i].t_ID == playerTurnL.Count + 1) {
//						MatchManager.I.currentCategory = oppTurns [i].c_ID;
						currentQuestion = questionDatabase.getQuestionById(oppTurnL[i].q_ID); // Last question played by opponent.

					}
				}
			} else {
				// Get random question
				currentQuestion = questionDatabase.getRandomCategoryQuestion (currentCategory);
			}
		} else {
			currentQuestion = questionDatabase.getRandomCategoryQuestion (currentCategory);
		}

		SetCategoryTitle ();
		SetPlayersInformation ();
		SetQuestionReady ();
	}


	/** Check whether the given answer is correct or not correct, switch to next category or home scene according to the outcome**/
	public void checkAnswer(string Answer) {
         
        // Hide Timer
        if (Answer != "")
        {
			Timer.SetActive (false);

        }
		if (!answeredQuestion) {
			Button selectedAnswer = getButtonByAnswer (Answer);
			Button rightAnswer = getButtonByAnswer (currentQuestion.q_Correct);
			int newturnID = (playerTurnL.Count != 9 ? (playerTurnL.Count + 1) : playerTurnL.Count);
			Turn newTurn;
			/***************************** CORRECT ANSWER ********************************/
			if (Answer == currentQuestion.q_Correct)
            {
                //Tweening
                foreach (Text text in XPPopUp.GetComponentsInChildren<Text>())
                {
                    text.DOFade(1, 0.3f);
                    text.DOFade(0, 0.2f).SetDelay(0.5f);
                }
                // Set score
				playerScore.text = (int.Parse(playerScore.text)+1).ToString();
                // Turn button color to green
                rightAnswer.GetComponent<Image> ().sprite = goodAnswer;
				rightAnswer.GetComponentInChildren<Text> ().color = Color.white;
				// Change progress question image
				playerTurns.transform.GetChild((playerTurnL.Count == 9 ? 8 : playerTurnL.Count)).GetComponent<Image>().sprite = rightRound;
				// Change turn information -- Set player id to 1 - to be done: change to gamedonia player id
				newTurn = new Turn (newturnID, PlayerManager.I.player.playerID, currentQuestion.q_Id, currentCategory, 1);
				// Set next question string
				nextScene = "Category";
                //set bool win animation
                winAnim = true;
                //total questions answered right counter
                PlayerManager.I.player.rightAnswersTotal  ++;
                //Row questions answered right counter
                PlayerManager.I.player.rightAnswersRow ++;

				// Right answered questions in row,  set xp and show brain coins
				switch (PlayerManager.I.player.rightAnswersRow)
				{
					case 3: 
						PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 20;
						showBrainCoinTween (2, 20);
						break;
					case 6:
						PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 50;
						showBrainCoinTween (3, 50);
						break;
					case 9:
						PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 100;
						showBrainCoinTween (4, 100);
						break;
					default:
						PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP += 10;
						showBrainCoinTween (1, 10);
						break;
				}

                // Keep data of good answered questions in category x
                
				switch (currentCategory)
				{
					case 7: //total right questions in veslavingszorg
                        PlayerManager.I.player.verslavingsAnswers++;
						break;
					case 8: //total right questions in ouderenzorg
                        PlayerManager.I.player.oldieAnswers++;
						break;
					case 9: //total right questions in ziekenhuiszorg
						PlayerManager.I.player.ziekenhuisAnswers++;
						break;
					case 10: //total right questions in algemenezorg
                        PlayerManager.I.player.algemeenAnswers++;
						break;
					case 11: //total right questions in tand & huisarts
						PlayerManager.I.player.artsAnswers++;
						break;
					case 12: //total right questions in gehandicaptenzorg
						PlayerManager.I.player.gehandicaptenAnswers++;
						break;
				}

				// Game ends when player has answered the 9th question correctly
				if(newturnID == 9) {
					MatchManager.I.ChangeLastTurn (newTurn, true);
                    //check all after game achievements
                    AchievementManager.I.checkAchievementsAfterGame();
                    //turn on to endscreen button
                    Continue.SetActive(false);
                    continueToEnd.SetActive(true);
                    // add games played
                    PlayerManager.I.player.playedMatches++;
                    StartCoroutine(ShowEndScreen());

                /////////Check if players wins or loses//////
                    //Player loses
                    if(int.Parse(playerScore.text) <= int.Parse(oppScore.text))
                    {
                        MatchManager.I.winningMatch = false;
                    }

                    //Player wins
                    if (int.Parse(playerScore.text) >= int.Parse(oppScore.text))
                    {
                        MatchManager.I.winningMatch = true;
						PlayerManager.I.UnlockNewAttribute ();
                    }
                }


			/***************************** WRONG ANSWER ********************************/		
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
					playerTurns.transform.GetChild((playerTurnL.Count == 9 ? 8 : playerTurnL.Count)).GetComponent<Image>().sprite = wrongRound;
                    //set lose animation
                    loseAnim = true;
                    //sound
                    AudioManagerScript.I.wrongAnwserSound.Play();
				}
                PlayerManager.I.player.rightAnswersRow = 0;
                // Change turn information
				newTurn = new Turn (newturnID, PlayerManager.I.player.playerID, currentQuestion.q_Id, currentCategory, 0); 
				// Switch to home scene
				nextScene = "Home";
			}

            // check for completed achievements & lvl up
            AchievementManager.I.checkAchievementAfterAnswer();
            PlayerManager.I.CheckLevelUp();

            // Save new turn to match
			if (playerTurnL.Count != 9) {
				MatchManager.I.AddTurn (newTurn);
			} else {
				MatchManager.I.ChangeLastTurn (newTurn, false);
			}
			
			if (Answer != "")
            {
				Continue.SetActive (true);
                answeredQuestion = true;
            }
		}
	}
		
	public void switchScene()
    {
		MatchManager.I.clearCurrentCategory ();
        if(nextScene == "Home")
        {
            PlayerManager.I.player.rightAnswersRow = 0;
        }
		Loader.I.enableLoader ();
		Loader.I.LoadScene (nextScene);
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

	private void SetPlayersInformation() {

		// Player information
		playerName.text = PlayerManager.I.player.profile.name;
		playerRankImg.sprite = PlayerManager.I.GetRankSprite();
		// Turn round information
		SetPlayerTurnRounds();
		// Opponent Information
		// Set information only if we already connected to an opponent
		if (opponentId != "") {
			SetOppTurnRounds ();
			StartCoroutine (SetOpponentInfo ());
		}

	}

	private IEnumerator SetOpponentInfo(float time=0) {
		while(PlayerManager.I.currentOpponentInfo == null) {
			yield return null;
		}
		oppName.text = PlayerManager.I.currentOpponentInfo ["name"].ToString();
		oppRankImg.sprite = PlayerManager.I.GetRankSprite (int.Parse(PlayerManager.I.currentOpponentInfo["lvl"].ToString()));
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

	private void SetPlayerTurnRounds() {
		float total = 0;

		if (playerTurnL != null) {
			for (int i = 0; i < playerTurnL.Count; i++) {
				if (playerTurnL[i].t_st == 1) {
//					playerRounds [i].sprite = rightRound;
					playerTurns.transform.GetChild (i).GetComponent<Image> ().sprite = rightRound;
					total++;
				} else {
					playerTurns.transform.GetChild (i).GetComponent<Image> ().sprite = wrongRound;
				}
			}
		}
		playerScore.text = total.ToString ();

	}

	private void SetOppTurnRounds() {
		float total = 0;

		if (oppTurnL != null) {
			for (int i = 0; i < oppTurnL.Count; i++) {
				if (oppTurnL[i].t_st == 1) {
					oppTurns.transform.GetChild (i).GetComponent<Image> ().sprite = rightRound;
					total++;
				} else {
					oppTurns.transform.GetChild (i).GetComponent<Image> ().sprite = wrongRound;
				}
			}
		}
		oppScore.text = total.ToString ();

	}

	private void showBrainCoinTween(int numbers, int coinValue) {
		switch (numbers) 
		{
		case 1:
			// Tween 1 coin
			xpCoins.transform.GetChild (0).gameObject.SetActive(true);
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 70f, 0);
			break;
		case 2:
			// Tween 2 coins
			xpCoins.transform.GetChild (0).gameObject.SetActive(true);
			xpCoins.transform.GetChild (1).gameObject.SetActive(true);
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 70f, 0);
			popupCoin(xpCoins.transform.GetChild (1).GetComponent<RectTransform> (), xpCoins.transform.GetChild (1).GetComponent<Image> (), xpCoins.transform.GetChild (1).GetComponent<AudioSource> (), 80f, 0.1f);
			break;
		case 3:
			// Tween 3 coins
			xpCoins.transform.GetChild (0).gameObject.SetActive(true);
			xpCoins.transform.GetChild (1).gameObject.SetActive(true);
			xpCoins.transform.GetChild (2).gameObject.SetActive(true);
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 70f, 0);
			popupCoin(xpCoins.transform.GetChild (1).GetComponent<RectTransform> (), xpCoins.transform.GetChild (1).GetComponent<Image> (), xpCoins.transform.GetChild (1).GetComponent<AudioSource> (), 80f, 0.1f);
			popupCoin(xpCoins.transform.GetChild (2).GetComponent<RectTransform> (), xpCoins.transform.GetChild (2).GetComponent<Image> (), xpCoins.transform.GetChild (2).GetComponent<AudioSource> (), 80f, 0.2f);
			break;
		case 4:
			// Tween 4 coins
			xpCoins.transform.GetChild (0).gameObject.SetActive(true);
			xpCoins.transform.GetChild (1).gameObject.SetActive(true);
			xpCoins.transform.GetChild (2).gameObject.SetActive(true);
			xpCoins.transform.GetChild (3).gameObject.SetActive(true);
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
		StartCoroutine(hideCoin(delay+1.5f, rect.transform.gameObject));
		// Show experience gained
	}

    //Waiting to change to end scene after Q9
    IEnumerator ShowEndScreen()
    {
        yield return new WaitForSeconds(5);
        //load end game scene
        SceneManager.LoadScene("Match_End");
    }

	private IEnumerator hideCoin(float time, GameObject coin) {
		yield return new WaitForSeconds (time);
		coin.SetActive(false);
		
	}
}