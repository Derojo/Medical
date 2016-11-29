using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;
using LitJson_Gamedonia;
using Gamedonia.Backend;

public class QuestionManager : Singleton<QuestionManager> {
	
	// First startup
	public GameObject addedBy;
	public GameObject correctAnswer;
	public GameObject chosenAnswer;
	public GameObject startupVS;
	public GameObject avatarsVS;
	public GameObject defaultAvatars;
	public Text VS;
	public Text playerNameVS;
	public Image playerRankImgVS;
    public Text oppNameVS;
	public Image oppRankImgVS;
	public Text timerText;
	public Color colorOrange;
	public Color colorGreen;
	public GameObject questionTitle;
	public GameObject questionAnswers;
	public Text CategoryTitle;
	public Text Question;
	public Button AnswerA;
	public Button AnswerB;
	public Button AnswerC;
	public Button AnswerD;
	public GameObject Continue;
    public GameObject continueToEnd;
    public GameObject XPPopUp;
    public Text turnCounter;
    //animation controls
    public Animator animControl;
    public Animator oppAnimControl;

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
	public bool questionReady = false;
	private bool showAddedBy = false;



	void Start()
    {
		
        //allowing answers
        answeredQuestion = false;

        // Match initialization 
        Match currentMatch = MatchManager.I.GetMatch (MatchManager.I.currentMatchID);
		opponentId = MatchManager.I.GetOppenentId (currentMatch);
		//opponentId = "";
		if (opponentId != "") {
			PlayerManager.I.GetPlayerInformationById (opponentId);
            
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
						QuestionBackend.I.setQuestionById(oppTurnL[i].q_ID); // Last question played by opponent.
					}
				}
			} else {
				// Get random question
				//QuestionBackend.I.setQuestionById("57aa4968e4b0967309dd20c6"); 
				QuestionBackend.I.setRandomQuestion (currentCategory);
			}
		} else {
            //QuestionBackend.I.setQuestionById("57aa4968e4b0967309dd20c6"); 
            QuestionBackend.I.setRandomQuestion (currentCategory);
        }

        SetPlayersInformation ();
		
		if(playerTurnL.Count == 0) {
			// Hide default avatars
			//defaultAvatars.SetActive(false);
			//Show first startup VS
			startupVS.SetActive(true);
			avatarsVS.SetActive(true);
			VS.DOFade(1,1f);
			StartCoroutine(hideStartupVS());
		} else {
			Loader.I.enableLoader();
			StartCoroutine(waitBeforeQuestionLoaded());
		}
		

	}
	
	private IEnumerator hideStartupVS() {
		bool continueWithQuestion = false;
		timerText.GetComponent<RectTransform>().DOScale(1.3f, 0.5f);
		timerText.GetComponent<RectTransform>().DOScale(1f, 0.5f).SetDelay(0.5f);
		yield return new WaitForSeconds(1f);
		timerText.DOColor(colorOrange, 1f);
		timerText.GetComponent<RectTransform>().DOScale(1.4f, 0.5f);
		timerText.GetComponent<RectTransform>().DOScale(1f, 0.5f).SetDelay(0.5f);
		timerText.text = "2";
		yield return new WaitForSeconds(1f);
		timerText.GetComponent<RectTransform>().DOScale(1.5f, 0.5f);
		timerText.GetComponent<RectTransform>().DOScale(1f, 0.5f).SetDelay(0.5f);
		timerText.text = "1";
		timerText.DOColor(colorGreen, 1f);
		yield return new WaitForSeconds(1f);
		timerText.GetComponent<RectTransform>().DOScale(1.6f, 0.5f);
		timerText.GetComponent<RectTransform>().DOScale(1f, 0.5f).SetDelay(0.5f);
		timerText.text = "0";
		avatarsVS.transform.GetChild(0).transform.DOLocalMoveX (-20, 1f);
		avatarsVS.transform.GetChild(1).transform.DOLocalMoveX (20, 1f);
		startupVS.transform.GetChild(0).GetComponent<Image>().DOFade(0, 1f);
		startupVS.transform.GetChild(1).GetComponent<Image>().DOFade(0, 0.5f);
		startupVS.transform.GetChild(2).GetComponent<Image>().DOFade(0, 0.5f);
		startupVS.transform.GetChild(3).GetComponent<Text>().DOFade(0, 0.5f);
		startupVS.transform.GetChild(4).transform.DOMoveX (-100, 1f);
		startupVS.transform.GetChild(5).transform.DOMoveX (100, 1f);
		startupVS.transform.GetChild(6).transform.DOMoveY (100, 1f);
		startupVS.transform.GetChild(7).transform.DOMoveY (100, 1f);
		defaultAvatars.transform.GetChild(0).transform.DOLocalMoveX (1f, 1f).SetEase(Ease.OutExpo);
		defaultAvatars.transform.GetChild(0).transform.DOLocalMoveZ (-0.37f, 1f).SetEase(Ease.OutExpo);
		defaultAvatars.transform.GetChild(1).transform.DOLocalMoveX(8.5f, 1f).SetEase(Ease.OutExpo);
		defaultAvatars.transform.GetChild(1).transform.DOLocalMoveZ (1, 1f).SetEase(Ease.OutExpo);
		AudioManagerScript.I.bell.Play();
		yield return new WaitForSeconds(0.5f);
		Destroy(startupVS);
		Destroy(avatarsVS);
		StartCoroutine(waitBeforeQuestionLoaded());
	}
	
	private IEnumerator waitBeforeQuestionLoaded() {

		while(!QuestionBackend.I.questionLoaded) {
			yield return new WaitForSeconds (1f);
		}
		QuestionBackend.I.questionLoaded = false;
		Loader.I.disableLoader();
		defaultAvatars.transform.GetChild(0).transform.DOLocalMoveX (1f, 1f).SetEase(Ease.OutExpo);
		defaultAvatars.transform.GetChild(0).transform.DOLocalMoveZ (-0.37f, 1f).SetEase(Ease.OutExpo);
		defaultAvatars.transform.GetChild(1).transform.DOLocalMoveX(8.5f, 1f).SetEase(Ease.OutExpo);
		defaultAvatars.transform.GetChild(1).transform.DOLocalMoveZ (1, 1f).SetEase(Ease.OutExpo);
		SetCategoryTitle ();
		SetQuestionReady ();
		if(QuestionBackend.I.currentQuestion.sID != "admin") {
            Debug.Log("Show added by: "+QuestionBackend.I.currentQuestion.sID);
			showAddedBy = true;
			setAddedByInfo(QuestionBackend.I.currentQuestion.sID);
		}
	}
	
	private void setAddedByInfo(string playerID) {
        
        GamedoniaUsers.GetUser(playerID, delegate (bool success, GDUserProfile data) { 
			if (success) {
				//returnInformation["name"] = data.profile["name"].ToString();
				Dictionary<string, object> playerInfo = data.profile;
				addedBy.transform.GetChild(0).GetComponent<Image>().sprite = PlayerManager.I.GetRankSprite (int.Parse(playerInfo["lvl"].ToString()));
				addedBy.transform.GetChild(2).GetComponent<Text>().text = playerInfo["name"].ToString();
			}
		});
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
			Button rightAnswer = getButtonByAnswer (QuestionBackend.I.currentQuestion.qCA);
			int newturnID = (playerTurnL.Count != 9 ? (playerTurnL.Count + 1) : playerTurnL.Count);
			Turn newTurn;
			/***************************** CORRECT ANSWER ********************************/
			if (Answer == QuestionBackend.I.currentQuestion.qCA)
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
				newTurn = new Turn (newturnID, PlayerManager.I.player.playerID, QuestionBackend.I.currentQuestion.q_Id, currentCategory, 1);
				// Set next question string
				nextScene = "Category";
                //set bool win animation
                animControl.SetBool("IsWinning", true);
                oppAnimControl.SetBool("IsWinning", true);
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
					case 1: //total right questions in veslavingszorg
                        PlayerManager.I.player.verslavingsAnswers++;
						break;
					case 2: //total right questions in ouderenzorg
                        PlayerManager.I.player.oldieAnswers++;
						break;
					case 3: //total right questions in ziekenhuiszorg
						PlayerManager.I.player.ziekenhuisAnswers++;
						break;
					case 4: //total right questions in algemenezorg
                        PlayerManager.I.player.algemeenAnswers++;
						break;
					case 5: //total right questions in tand & huisarts
						PlayerManager.I.player.artsAnswers++;
						break;
					case 6: //total right questions in gehandicaptenzorg
						PlayerManager.I.player.gehandicaptenAnswers++;
						break;
				}

				// Game ends when player has answered the 9th question correctly
				if(newturnID == 9) {
					MatchManager.I.ChangeLastTurn (newTurn, true, true);

                    //turn on to endscreen button
                    Continue.SetActive(false);
                    continueToEnd.SetActive(true);
                    StartCoroutine(ShowEndScreen());

                /////////Check if players wins or loses//////
                    //Player loses
					string winner  = MatchManager.I.getWinner(null, true);
					if(winner == PlayerManager.I.player.playerID) {
						MatchManager.I.winningMatch = true;
						Debug.Log("Unlock new attribute");
						PlayerManager.I.UnlockNewAttribute ();
					} else {
						if(winner == "tie") {
							MatchManager.I.tie = true;
						} else {
							 MatchManager.I.winningMatch = false;
						}
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
					chosenAnswer.GetComponentInChildren<Text>().text = selectedAnswer.GetComponentInChildren<Text>().text;
					selectedAnswer.GetComponentInChildren<Text> ().color = Color.white;
					// Change progress question image
					playerTurns.transform.GetChild((playerTurnL.Count == 9 ? 8 : playerTurnL.Count)).GetComponent<Image>().sprite = wrongRound;
                    //set lose animation
                    animControl.SetBool("IsLosing", true);
                    oppAnimControl.SetBool("IsLosing", true);
                    //sound
                    AudioManagerScript.I.wrongAnwserSound.Play();
				}
                PlayerManager.I.player.rightAnswersRow = 0;
                // Change turn information
				newTurn = new Turn (newturnID, PlayerManager.I.player.playerID, QuestionBackend.I.currentQuestion.q_Id, currentCategory, 0); 
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
				MatchManager.I.ChangeLastTurn (newTurn, false, false);
			}
			
			if (Answer != "")
            {
				if(showAddedBy) {
					questionAnswers.transform.DOScale(0,1f);
					questionAnswers.SetActive(false);
					addedBy.SetActive(true);
					addedBy.GetComponent<Image>().DOFade(0.2f, 1);
					addedBy.transform.GetChild(0).GetComponent<Image>().DOFade(0.27f, 1);
					addedBy.transform.GetChild(1).GetComponent<Text>().DOFade(1, 1);
					addedBy.transform.GetChild(2).GetComponent<Text>().DOFade(1, 1);
					addedBy.transform.GetChild(3).GetComponent<Image>().DOFade(0.42f, 1);
					addedBy.transform.GetChild(3).GetChild(0).GetComponent<Text>().DOFade(1, 1);
					correctAnswer.SetActive(true);
					correctAnswer.GetComponentInChildren<Text>().text = rightAnswer.GetComponentInChildren<Text>().text;
					correctAnswer.GetComponent<Image>().DOFade(1,1);
					correctAnswer.GetComponentInChildren<Text>().DOFade(1,1);
					if(Answer != QuestionBackend.I.currentQuestion.qCA) {
						chosenAnswer.SetActive(true);
						chosenAnswer.GetComponent<Image>().DOFade(1,1);
						chosenAnswer.GetComponentInChildren<Text>().DOFade(1,1);
					}
				}
				Continue.SetActive (true);

                answeredQuestion = true;
            }
		}
	}
	//switching scenes	
	public void switchScene()
    {
		QuestionBackend.I.currentQuestion = null;
		MatchManager.I.clearCurrentCategory ();

        if(nextScene == "Home")
        {
            PlayerManager.I.player.rightAnswersRow = 0;
			if(AdManager.I.playAddAfterQuestionWrong && AdManager.I.enableAds) {
				Loader.I.LoadScene (nextScene, true);
			}
        } else {
			if(AdManager.I.playAddAfterQuestionRight && AdManager.I.enableAds) {
				Loader.I.LoadScene (nextScene);
			}	
		}
		if(!AdManager.I.enableAds) {
			Loader.I.enableLoader ();
			Loader.I.LoadScene (nextScene);
		}
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
		playerNameVS.text = playerName.text;
		playerRankImg.sprite = PlayerManager.I.GetRankSprite();
		playerRankImgVS.sprite = playerRankImg.sprite;
		playerRankImgVS.DOFade(0.36f, 1f);
		// Turn round information
		SetPlayerTurnRounds();
		// Opponent Information
		// Set information only if we already connected to an opponent
		if (opponentId != "") {
			SetOppTurnRounds ();
			StartCoroutine (SetOpponentInfo ());
		} else {
			oppNameVS.text = "Willekeurig!";
			oppRankImgVS.DOFade(0.36f, 1f);
		}

	}

	private IEnumerator SetOpponentInfo(float time=0) {
		while(PlayerManager.I.currentOpponentInfo == null) {
			yield return null;
		}
		oppName.text = PlayerManager.I.currentOpponentInfo ["name"].ToString();
		oppNameVS.text = oppName.text;
		oppRankImg.sprite = PlayerManager.I.GetRankSprite (int.Parse(PlayerManager.I.currentOpponentInfo["lvl"].ToString()));
		oppRankImgVS.sprite = oppRankImg.sprite;
		oppRankImgVS.DOFade(0.36f, 1f);
	}
	private void SetCategoryTitle() {
		CategoryTitle.text = Categories.getCategoryNameById(currentCategory);
	}

	private void SetQuestionReady() {
		Question.text = QuestionBackend.I.currentQuestion.qT;
		AnswerA.GetComponentInChildren<Text>().text = QuestionBackend.I.currentQuestion.qA;
		AnswerB.GetComponentInChildren<Text>().text = QuestionBackend.I.currentQuestion.qB;
		AnswerC.GetComponentInChildren<Text>().text = QuestionBackend.I.currentQuestion.qC;
		AnswerD.GetComponentInChildren<Text>().text = QuestionBackend.I.currentQuestion.qD;
		questionTitle.GetComponent<Animator>().SetBool ("questionReady", true);
		questionAnswers.GetComponent<Animator>().SetBool ("questionReady", true);
		questionReady = true;
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
        if (playerTurnL != null && playerTurnL.Count > 0)
        {
            turnCounter.text = playerTurnL.Count + "/9";
        }


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
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 80f, 0);
			break;
		case 2:
			// Tween 2 coins
			xpCoins.transform.GetChild (0).gameObject.SetActive(true);
			xpCoins.transform.GetChild (1).gameObject.SetActive(true);
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 80f, 0);
			popupCoin(xpCoins.transform.GetChild (1).GetComponent<RectTransform> (), xpCoins.transform.GetChild (1).GetComponent<Image> (), xpCoins.transform.GetChild (1).GetComponent<AudioSource> (), 90f, 0.1f);
			break;
		case 3:
			// Tween 3 coins
			xpCoins.transform.GetChild (0).gameObject.SetActive(true);
			xpCoins.transform.GetChild (1).gameObject.SetActive(true);
			xpCoins.transform.GetChild (2).gameObject.SetActive(true);
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 80f, 0);
			popupCoin(xpCoins.transform.GetChild (1).GetComponent<RectTransform> (), xpCoins.transform.GetChild (1).GetComponent<Image> (), xpCoins.transform.GetChild (1).GetComponent<AudioSource> (), 90f, 0.1f);
			popupCoin(xpCoins.transform.GetChild (2).GetComponent<RectTransform> (), xpCoins.transform.GetChild (2).GetComponent<Image> (), xpCoins.transform.GetChild (2).GetComponent<AudioSource> (), 90f, 0.2f);
			break;
		case 4:
			// Tween 4 coins
			xpCoins.transform.GetChild (0).gameObject.SetActive(true);
			xpCoins.transform.GetChild (1).gameObject.SetActive(true);
			xpCoins.transform.GetChild (2).gameObject.SetActive(true);
			xpCoins.transform.GetChild (3).gameObject.SetActive(true);
			popupCoin(xpCoins.transform.GetChild (0).GetComponent<RectTransform> (), xpCoins.transform.GetChild (0).GetComponent<Image> (), xpCoins.transform.GetChild (0).GetComponent<AudioSource> (), 80f, 0);
			popupCoin(xpCoins.transform.GetChild (1).GetComponent<RectTransform> (), xpCoins.transform.GetChild (1).GetComponent<Image> (), xpCoins.transform.GetChild (1).GetComponent<AudioSource> (), 90f, 0.1f);
			popupCoin(xpCoins.transform.GetChild (2).GetComponent<RectTransform> (), xpCoins.transform.GetChild (2).GetComponent<Image> (), xpCoins.transform.GetChild (2).GetComponent<AudioSource> (), 90f, 0.2f);
			popupCoin(xpCoins.transform.GetChild (3).GetComponent<RectTransform> (), xpCoins.transform.GetChild (3).GetComponent<Image> (), xpCoins.transform.GetChild (3).GetComponent<AudioSource> (), 80f, 0.3f);
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