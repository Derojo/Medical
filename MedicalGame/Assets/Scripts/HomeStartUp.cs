using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;  
public class HomeStartUp : MonoBehaviour {

    public Image rankingImg;
    public Text currentRanktxt;
    public Text currentLVL;
    public GameObject lvlPopUp;
    private bool soundPlay = false;

    // Use this for initialization
    void Start ()
    {
        //Earning first achievement
        AchievementManager.I.checkAchievementConnect();
        //Getting rank image
        rankingImg.sprite = PlayerManager.I.GetRankSprite(PlayerManager.I.player.playerLvl);
        //Getting rank name
        currentRanktxt.text = PlayerManager.I.player.playerRank;
        currentLVL.text = "Je bent nu level " + PlayerManager.I.player.playerLvl.ToString() + " van de rank ";
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(PlayerManager.I.lvlUp);
        if (PlayerManager.I.lvlUp)
        {
            StartCoroutine(hideLvlUp());
        }
	}

    //Playing sound function
    public void onClick()
    {
        Debug.Log("test");
        lvlPopUp.SetActive(false);
        PlayerManager.I.lvlUp = false;
        soundPlay = false;  
    }

    //hiding popup after x secs
    IEnumerator hideLvlUp()
    {
        if (PlayerManager.I.lvlUp)
        {
            //Tweening in/out
            foreach (Text text in lvlPopUp.GetComponentsInChildren<Text>())
            {
                text.DOFade(1, 1f);
                //text.DOFade(0, 1f).SetDelay(3f);
            }

            foreach (Image img in lvlPopUp.GetComponentsInChildren<Image>())
            {
                if (img.transform.name != "LvlUpPopUp")
                {
                    img.DOFade(1, 1f);
                    //img.DOFade(0, 1f).SetDelay(3f);
                }
            }

            //show popup
            lvlPopUp.SetActive(true);     
            if(!soundPlay)
            {
                AudioManagerScript.I.lvlUpSound.Play();
                //setting soundbool
                soundPlay = true;
            }

            //starting timer for hiding popup
            yield return new WaitForSeconds(5);
            lvlPopUp.SetActive(false);
            PlayerManager.I.lvlUp = false;
            soundPlay = false;
        }
       

    }

}
