using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;



[Prefab("AchievementManager", true, "")]
public class AchievementManager : Singleton<AchievementManager> {

    public GameObject achievementPrefab;

    //public Sprite[] sprites;
    private float fadeTime = 1.5f;
    public GameObject visualAchievement;
    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();
    public List<Achievement> achievementList = new List<Achievement>();     

    public Sprite unlockedSprite;
    public Sprite lockedSprite;

    public bool Load() { return true; }


    void OnEnable() {

            achievements = new Dictionary<string, Achievement>();
            CreateAllAchievements();
        
   
    }
    // Use this for initialization
    void Start ()
    {

        if (achievementList == null) {
            achievementList = new List<Achievement>();
        }
        //Deleting playerprefs for testing REMEMBER TO REMOVE
       // PlayerPrefs.DeleteAll();
     

    }
	
	// Update is called once per frame
	void Update ()
    {

     

    }//end of update

    public void EarnAchievement(string title)
    {
        
        if( achievements[title].EarnAchievement())
        {
            GameObject achievement = (GameObject)Instantiate(visualAchievement);
            SetAchievementInfo("EarnCanvas", achievement, title);
            PlayerManager.I.player.playerXP = PlayerManager.I.player.playerXP + achievements[title].Points;
            PlayerManager.I.CheckLevelUp();

            //Tweening in/out
            foreach (Text text in achievement.GetComponentsInChildren<Text>())
            {
                text.DOFade(1, 2f);
                text.DOFade(0, 2f).SetDelay(5f);
            }

            foreach (Image img in achievement.GetComponentsInChildren<Image>())
            {
                img.DOFade(1, 2f);
                img.DOFade(0, 2f).SetDelay(5f);
            }

            //Destroy popup
            Destroy(achievement, 8f);
        }

    }

    public void CreateAllAchievements() {
        for (int i = 0; i < achievementList.Count; i++) {

            achievementList[i].LoadAchievement();
            achievements.Add(achievementList[i].Name, achievementList[i]);
        }
    }
    
    //Creat achievement prefab
    public void CreateAchievement(string parent, string title, string description, int points, int spriteIndex)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab);
        SetAchievementInfo(parent, achievement, title);
       
    }
    //Destroy achievement afeter 3 seconds
    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);

    }


    //Set information about achievement
    public void SetAchievementInfo(string parent, GameObject achievement, string title)
    {
        achievement.transform.SetParent(GameObject.Find(parent).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1);
        achievement.transform.localPosition = new Vector3(0,0, 0);
        // Getting achievement childeren
        achievement.transform.GetChild(1).GetComponent<Text>().text = title;
        achievement.transform.GetChild(2).GetComponent<Text>().text = achievements[title].Description;
        achievement.transform.GetChild(3).GetComponent<Text>().text = achievements[title].Points.ToString();
        achievement.transform.GetChild(4).GetComponent<Image>().sprite = achievements[title].ASprite;

    }


    


    /************** Achievement terms*********************/

    // #1 Earning first time connect achievement
    public void checkAchievementConnect()
    {
        //Check if player logged in
        if(PlayerManager.I.player.loggedIn && PlayerManager.I.player.createdProfile)
        {
            EarnAchievement("Connecten");
        }
    }

    // #2 Earning "Vrienden Worden?" achievement

    // #3 Earning "Beginnende vriendschap" achievement

    // #4 Earning first win achievement
    public void AchievementWinner()
    {
        //check if player won his first match
        if(PlayerManager.I.player.wonMatches == 1)
        {
            EarnAchievement("Winnaar");
        }
        
    }

    // #5 Earning "Vriendenkring" achievement
    public void AchievementVriendenKring()
    {
        // Check if player is playing 5 games at the same time
        if(PlayerManager.I.player.activeGames == 5)
        {
            EarnAchievement("Vriendenkring");
        }
    }

    // #6 Earning "Populair" achievement

    // #7 Earning "Op dreef" achievement
    public void AchievementOpDreef()
    {
        //Check if player won 10 matches in total
        if (PlayerManager.I.player.wonMatches == 10)
        {
            EarnAchievement("Op dreef");
        }

    }


    // #8 Earning "Ultieme vriend" achievement

    // #9 Earning "Geleerde" achievement
    public void AchievementGeleerde()
    {
        // check if player answered 6 questions in a row 
        if (PlayerManager.I.player.rightAnswersRow == 6)
        {
            EarnAchievement("Geleerde");
        }

    }
    // #10 Earning "Nerd" achievement
    public void AchievementNerd()
    {
        // check if player answered 9 questions in a row 
        if (PlayerManager.I.player.rightAnswersRow == 9)
        {
            EarnAchievement("Nerd");
        }

    }

    // #11 Earning "Sportfanaat" achievement
    public void AchievementSportFanaat()
    {
        // check if total questions answered in the catagory sport = 10
        if (PlayerManager.I.player.sportAnswers == 10)
        {
            EarnAchievement("Sportfanaat");
        }

    }

    // #12 Earning "Beroemdheid" achievement
    public void AchievementBeroemdheid()
    {
        // check if total questions answered in the catagory TV & entertainment = 10
        if (PlayerManager.I.player.entertainmentAnswers == 10)
        {
            EarnAchievement("Beroemdheid");
        }

    }


    // #13 Earning "Fossiel"  achievement
    public void AchievementFossiel()
    {
        // check if total questions answered in the catagory History = 10
        if (PlayerManager.I.player.historyAnswers == 10)
        {
            EarnAchievement("Fossiel");
        }

    }


    // #14 Earning "Wereldreiziger" achievement
    public void AchievementWereldReiziger()
    {
        // check if total questions answered in the catagory geographics = 10
        if (PlayerManager.I.player.geographicAnswers == 10)
        {
            EarnAchievement("Wereldreiziger");
        }

    }

    // #15 Earning "Verzorger" achievement

    public void AchievementVerzorger()
    {
        // check if total questions answered in the catagory care & sience = 10
        if (PlayerManager.I.player.careAnswers == 10)
        {
            EarnAchievement("Verzorger");
        }

    }

    // #16 Earning "Stylist" achievement

    // #17 Earning "Hattrick" achievement
    public void AchievementHattrick()
    {
        // check if player won 3 matches in a ROW
        if (PlayerManager.I.player.wonMatchesRow == 3)
        {
            EarnAchievement("Hattrick");
        }

    }
    // #18 Earning "Uitdager" achievement

    // #19 Earning "Professor" achievement
    public void AchievementProfessor()
    {
        

        // check if player answered 100 questions right
        if (PlayerManager.I.player.rightAnswersTotal == 100)
        {
            
            EarnAchievement("Professor");
        }

    }

    // #20 Earning "Onverslaanbaar" achievement
    public void AchievementOnverslaanbaar()
    {
        // check if player won 10 matches in a ROW
        if (PlayerManager.I.player.wonMatchesRow == 10)
        {
            EarnAchievement("Onverslaanbaar");
        }

    }
    // #21 Earning "Afmaker" achievement
    public void AchievementAfmaker()
    {
        // check if player played 5 matches in total
        if (PlayerManager.I.player.playedMatches == 5)
        {
            EarnAchievement("Afmaker");
        }

    }

    // #22 Earning "Kampioenr" achievement

    // #23 Earning "drie op een rij" achievement

    public void AchievementDrieOpRij()
    {
        // check if player answered 3 questions in a row 
        if (PlayerManager.I.player.rightAnswersRow == 3)
        {
            EarnAchievement("3 op een rij");
        }

    }

    /************** Mulitple checks *********************/

    //Checking functions after game
    public void checkAchievementsAfterGame()
    {
        AchievementWinner();
        AchievementOpDreef();
        AchievementHattrick();
        AchievementOnverslaanbaar();
        AchievementAfmaker();
    }

    public void checkAchievementAfterAnswer()
    {
        
        //right answer in a row functions
        AchievementDrieOpRij();
        AchievementGeleerde();
        AchievementNerd();

        // 10 total in catagory functions
        AchievementSportFanaat();
        AchievementBeroemdheid();
        AchievementFossiel();
        AchievementWereldReiziger();
        AchievementVerzorger();

        //100 right answers
        AchievementProfessor();

    }
    

}
