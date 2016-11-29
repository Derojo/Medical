using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class Friend : MonoBehaviour {

    public Text friendName;
    public Text friendLvl;
    public Text friendRank;
    public Image bigRank;
    public Image smallRank;
    public GameObject friendAvatar;
    public Button startMatch;
    public int livesLeft = 5;
    public GameObject response;
    public Text responseText;
    public GameObject attributeContent;
    public Text attributesTotal;

    // Use this for initialization
    void Start () {
        /*
        foreach (KeyValuePair<string, object> data in PlayerManager.I.currentFriendInfo)
        {
            Debug.Log(data.Key+"-"+data.Value);
        }*/
        if (MatchManager.I.matches != null && MatchManager.I.matches.Count > 0)
        {
            livesLeft = RuntimeData.I.livesAmount - MatchManager.I.getTotalActiveMatches();
            if (livesLeft < 0)
            {
                livesLeft = 0;
            }
        }

        friendName.text = PlayerManager.I.currentFriendInfo["name"].ToString();
        friendLvl.text = "Level "+PlayerManager.I.currentFriendInfo["lvl"].ToString();
        friendRank.text = PlayerManager.I.GetRankName(int.Parse(PlayerManager.I.currentFriendInfo["lvl"].ToString()));
        bigRank.sprite = PlayerManager.I.GetRankSprite(int.Parse(PlayerManager.I.currentFriendInfo["lvl"].ToString()));
        smallRank.sprite = bigRank.sprite;
        friendAvatar.GetComponent<buildAvatar>().setCustomAvatarByString(PlayerManager.I.currentFriendInfo["avatar"].ToString());
        startMatch.onClick.AddListener(delegate { startFriendMatch();});

        // Attributes
        List<int> attributesList = PlayerManager.I.GetFriendAttributes(PlayerManager.I.currentFriendId);
        int ammountOfAttributes = attributesList.Count;
        attributesTotal.text = ammountOfAttributes.ToString();
        if (ammountOfAttributes > 0)
        {
            attributeContent.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            attributeContent.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = PlayerManager.I.currentFriendInfo["age"].ToString();
        }
        if (ammountOfAttributes > 1)
        {
            attributeContent.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
            attributeContent.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = PlayerManager.I.currentFriendInfo["color"].ToString();
        }

        if (ammountOfAttributes > 2)
        {
            attributeContent.transform.GetChild(5).GetChild(1).gameObject.SetActive(false);
            attributeContent.transform.GetChild(5).GetChild(0).GetComponent<Text>().text = PlayerManager.I.currentFriendInfo["hobby"].ToString();
        }
        if (ammountOfAttributes > 3)
        {
            attributeContent.transform.GetChild(7).GetChild(1).gameObject.SetActive(false);
            attributeContent.transform.GetChild(7).GetChild(0).GetComponent<Text>().text = PlayerManager.I.currentFriendInfo["film"].ToString();
        }
        if (ammountOfAttributes > 4)
        {
            attributeContent.transform.GetChild(9).GetChild(1).gameObject.SetActive(false);
            attributeContent.transform.GetChild(9).GetChild(0).GetComponent<Text>().text = PlayerManager.I.currentFriendInfo["instelling"].ToString();
        }
    }

    private void startFriendMatch()
    {
        if (livesLeft > 0)
        {
            if (!MatchManager.I.checkForPlayingWithFriend(PlayerManager.I.currentFriendId))
            {
                MatchManager.I.StartFriendMatch(PlayerManager.I.currentFriendId);
            }
            else
            {
                responseText.text = "Je kunt maar 1 potje tegelijk tegen iemand spelen!";
                StartCoroutine(showResponse(2f));
            }
        }
        else
        {
            responseText.text = "Je hebt niet genoeg levens om nog een potje te starten!";
            StartCoroutine(showResponse(2f));
        }
    }

    private IEnumerator showResponse(float time)
    {
        response.SetActive(true);
        response.GetComponentInChildren<Text>().DOFade(1, 1f);
        responseText.DOFade(1, 1f);
        response.GetComponentInChildren<Image>().DOFade(1, 1f);
        yield return new WaitForSeconds(time);
        response.GetComponentInChildren<Text>().DOFade(0, 1f);
        responseText.DOFade(0, 1f);
        response.GetComponentInChildren<Image>().DOFade(0, 1f);
        yield return new WaitForSeconds(0.5f);
        response.SetActive(false);
    }
}
