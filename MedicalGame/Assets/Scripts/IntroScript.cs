using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{

    public GameObject hint1;
    public GameObject hint2;
    public GameObject hint3;
    public GameObject hint4;
    public GameObject hint5;
    public GameObject hint6;
    public GameObject hint7;
    public GameObject hint8;
    public GameObject menuRemove;
    public GameObject BackGround;
    public Image BackGroundOverlay;
    public int timesPressed;
   
    // Use this for initialization
    void Start ()
    {
        BackGround.SetActive(false);
        //fading hint 1
        foreach (Text text in hint1.GetComponentsInChildren<Text>())
        {
            text.DOFade(1, 1f);

        }

        foreach (Image img in hint1.GetComponentsInChildren<Image>())
        {
            img.DOFade(1f, 1f);

        }

        //fading in image
        BackGroundOverlay.GetComponent<Image>().DOFade(0.45f, 1f);
        if (PlayerManager.I.player.completedIntro)
        {
            menuRemove.SetActive(false);
        }
        else
        {
            menuRemove.SetActive(true);
        }


    }
	
	

    //counting the times the forwardbutton is clicked
    public void furtherButtonClick()
    {
        timesPressed++;
        checkClicks();
       
    }
    //counting the times the backbutton is clicked
    public void backButtonClick()
    {
        timesPressed--;
        checkClicks();
        
    }

    //click checker
    public void checkClicks()
    {
        //displaying 1e text
         if (timesPressed == 0)
        {

            BackGround.SetActive(false);
            foreach (Text text in hint1.GetComponentsInChildren<Text>())
            {
                text.DOFade(1, 1f);

            }

            foreach (Image img in hint1.GetComponentsInChildren<Image>())
            {
                img.DOFade(1f, 1f);

            }
            foreach (Text text in hint2.GetComponentsInChildren<Text>())
            {
                text.DOFade(0, 0f);

            }

            foreach (Image img in hint2.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0f);

            }

            hint1.SetActive(true);
            hint2.SetActive(false);

            BackGroundOverlay.GetComponent<Image>().DOFade(0.45f, 1f);

        }

        //displaying 2e text
        if (timesPressed == 1)
        {
            BackGroundOverlay.GetComponent<Image>().DOFade(0, 1f);
            hint1.SetActive(false);
            hint2.SetActive(true);
            hint3.SetActive(false);

            //Setting previous hint alfa to 0
            foreach (Text text in hint1.GetComponentsInChildren<Text>())
            {
                text.DOFade(0, 0.5f);

            }

            foreach (Image img in hint1.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0.5f);

            }

            //backward fade
            foreach (Text text in hint2.GetComponentsInChildren<Text>())
            {
                text.DOFade(1f, 0.5f);

            }
            
            foreach (Image img in hint2.GetComponentsInChildren<Image>())
            {
                img.DOFade(1f, 0.5f);

            }
            //Setting next hint alfa to 0
            foreach (Text text in hint3.GetComponentsInChildren<Text>())
            {
                text.DOFade(0f, 0f);

            }

            foreach (Image img in hint3.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0f);

            }


        }

        //displaying 3e text
        if (timesPressed == 2)
        {
            BackGround.SetActive(true);
            //foward fade
            foreach (Text text in hint2.GetComponentsInChildren<Text>())
            {
                text.DOFade(0, 0.5f);

            }

            foreach (Image img in hint2.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0.5f);

            }
            //backward fade
            foreach (Text text in hint3.GetComponentsInChildren<Text>())
            {
                text.DOFade(1f, 0.5f);

            }

            foreach (Image img in hint3.GetComponentsInChildren<Image>())
            {
                img.DOFade(1f, 0.5f);

            }
            //Setting next hint alfa to 0
            foreach (Text text in hint4.GetComponentsInChildren<Text>())
            {
                text.DOFade(0f, 0f);

            }

            foreach (Image img in hint4.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0f);

            }
            hint2.SetActive(false);
            hint3.SetActive(true);
            hint4.SetActive(false);
           // gameBackground.SetActive(true);
        }

        //displaying 2e text
        if (timesPressed == 3)
        {
            //foward fade
            foreach (Text text in hint3.GetComponentsInChildren<Text>())
            {
                text.DOFade(0, 0.5f);

            }

            foreach (Image img in hint3.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0.5f);

            }
            //backward fade
            foreach (Text text in hint4.GetComponentsInChildren<Text>())
            {
                text.DOFade(1, 0.5f);

            }

            foreach (Image img in hint4.GetComponentsInChildren<Image>())
            {
                img.DOFade(1f, 2f);

            }
            //Setting next hint alfa to 0
            foreach (Text text in hint5.GetComponentsInChildren<Text>())
            {
                text.DOFade(0f, 0f);

            }

            foreach (Image img in hint5.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0f);

            }

            hint3.SetActive(false);
            hint4.SetActive(true);
            hint5.SetActive(false);
  
        }

        if (timesPressed == 4)
        {
            //forward fade
            foreach (Text text in hint4.GetComponentsInChildren<Text>())
            {
                text.DOFade(0, 0.5f);

            }

            foreach (Image img in hint4.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0.5f);

            }
            //backward fade
            foreach (Text text in hint5.GetComponentsInChildren<Text>())
            {
                text.DOFade(1, 0.5f);

            }

            foreach (Image img in hint5.GetComponentsInChildren<Image>())
            {
                img.DOFade(1f, 0.5f);

            }
            //Setting next hint alfa to 0
            foreach (Text text in hint6.GetComponentsInChildren<Text>())
            {
                text.DOFade(0f, 0f);

            }

            foreach (Image img in hint6.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0f);

            }

            hint4.SetActive(false);
            hint5.SetActive(true);
            hint6.SetActive(false);
        }

        if (timesPressed == 5)
        {
            //fading previous hint
            foreach (Text text in hint5.GetComponentsInChildren<Text>())
            {
                text.DOFade(0, 0.5f);

            }

            foreach (Image img in hint5.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0.5f);

            }
            //backward fade
            foreach (Text text in hint6.GetComponentsInChildren<Text>())
            {
                text.DOFade(1f, 0.5f);

            }

            foreach (Image img in hint6.GetComponentsInChildren<Image>())
            {
                img.DOFade(1f, 0.5f);

            }
            //Setting next hint alfa to 0
            foreach (Text text in hint7.GetComponentsInChildren<Text>())
            {
                text.DOFade(0f, 0f);

            }

            foreach (Image img in hint7.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0f);

            }

            hint5.SetActive(false);
            hint6.SetActive(true);
            hint7.SetActive(false);
           
           
        }

        if (timesPressed == 6)
        {
            //fading previous hint
            foreach (Text text in hint6.GetComponentsInChildren<Text>())
            {
                text.DOFade(0, 0.5f);

            }

            foreach (Image img in hint6.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0.5f);

            }
            //backward fade
            foreach (Text text in hint7.GetComponentsInChildren<Text>())
            {
                text.DOFade(1, 0.5f);

            }

            foreach (Image img in hint7.GetComponentsInChildren<Image>())
            {
                img.DOFade(1f, 0.5f);

            }
            //Setting next hint alfa to 0
            foreach (Text text in hint8.GetComponentsInChildren<Text>())
            {
                text.DOFade(0f, 0f);

            }

            foreach (Image img in hint8.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0f);

            }
            BackGround.SetActive(true);
            hint6.SetActive(false);
            hint7.SetActive(true);
            hint8.SetActive(false);
           
        }
        if (timesPressed == 7)
        {
            //fading previous hint
            foreach (Text text in hint7.GetComponentsInChildren<Text>())
            {
                text.DOFade(0, 0.5f);

            }

            foreach (Image img in hint7.GetComponentsInChildren<Image>())
            {
                img.DOFade(0f, 0.5f);

            }

            //backward fade
            foreach (Text text in hint8.GetComponentsInChildren<Text>())
            {
                text.DOFade(1, 0f);

            }

            foreach (Image img in hint8.GetComponentsInChildren<Image>())
            {
                img.DOFade(1f, 0f);

            }


            BackGround.SetActive(false);
            hint7.SetActive(false);
            hint8.SetActive(true);
        }

        if (timesPressed == 8)
        {
            SceneManager.LoadScene("Home");
            PlayerManager.I.player.completedIntro = true;
        }

    }

}
