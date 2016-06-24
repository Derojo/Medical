using UnityEngine;
using System.Collections;
using Gamedonia.Backend;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class VerificationCode : MonoBehaviour
{

    public InputField verificationCode;
    public Color eColor;
    public Color dColor;
    public GameObject wrongLogin;
    public GameObject errorPopup;
    public Image lockImg;
    private bool i_access = false;
    public float waitForConnectionTime = 10;
    private string errorMsg = "";
    public static bool hideInput;

    void Awake()
    {
        //check for internet connection
        checkInternet();
    }
    // Use this for initialization
    void Start ()
    {

    }// end start
	
	// Update is called once per frame
	void Update ()
    {
      
    }//end update

    public void verificationCheck()
    {
        Loader.I.enableLoader();
        if (verificationCode.text == "")
        {
            lockImg.DOColor(eColor, 1);
            wrongLogin.SetActive(true);
            wrongLogin.GetComponent<Text>().text = "Je hebt niks ingevoerd!";
            Loader.I.disableLoader();
        }
       
       
        //if internet connection, start verificaiton
        if (i_access)
        {
            if(verificationCode.text != "")
            {
                string code = verificationCode.text;
                Debug.Log(code);
               GamedoniaData.Count("staticdata", "{\"playCode\":\""+code+"\"}", delegate (bool success, int count) 
               {
                    if (success)
                    {
                       if(count == 1)
                       {
                           //TODO Your success processing 
                            // setting bool 
                           PlayerManager.I.player.verificationComplete = true;
                           //loading login screen
                           SceneManager.LoadScene("Login");
                           //saving player files
                           PlayerManager.I.Save();
                           lockImg.DOColor(dColor, 1);
                           Debug.Log(count);
                       }
                       else
                       {
                           //when password is wrong
                           lockImg.DOColor (eColor, 1);
                           wrongLogin.SetActive(true);
                           wrongLogin.GetComponent<Text>().text = "Wachtwoord onjuist";
                           Loader.I.disableLoader();
                       }
                   }     
                    else
                   {

                       Debug.Log(count);
                       //TODO Your fail processing 
                   }
                });
            }
        }
        else
        {
            //start waiting for connection void
            StartCoroutine(waitForConnection(waitForConnectionTime));
        }
    }

    /** CHECK FOR INTERNET CONNECTION **/
    private void checkInternet()
    {
        GamedoniaBackend.isInternetConnectionAvailable(delegate (bool success) {
            if (success)
            {
                i_access = true;
            }
            else {
                i_access = false;
                errorMsg = "No internet access";
                Debug.Log(errorMsg);
            }
        });
    }

    private IEnumerator waitForConnection(float time)
    {
        Debug.Log("test");
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            if (!i_access)
            {
                checkInternet();
                yield return null;
            }
            else {
                verificationCheck();
            }
        }
        Loader.I.disableLoader();
        Loader.I.enableBackground();
        errorPopup.SetActive(true);
    }

}// end class
