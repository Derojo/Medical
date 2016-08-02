using UnityEngine;
using System.Collections;
using Gamedonia.Backend;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class rankingList : MonoBehaviour {

	public GameObject avatarWon;
	public GameObject spinningWheel;
	// Use this for initialization
	void Start () {
		showTopTenPlayers();
		spinningWheel.GetComponent<Image>().DOFade(1,3);
		spinningWheel.transform.DORotate(new Vector3(0,0,360), 15f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.OutElastic);
	}
	
	
	private void showTopTenPlayers() {
		float delayRow = 0;
		float minus = 0;
		// Get the top 10 players of the game
		GamedoniaUsers.Search("{}", 10, "{profile.wonAttr:-1}", 0, delegate (bool success, IList data) {
			if (success) {
				if (data != null) {
					for(int i = 0; i < data.Count; i++) {
						
						Dictionary<string, object> resultUser = (Dictionary<string, object>)data[i];
						Dictionary<string, object> resultProfile = (Dictionary<string, object>)resultUser["profile"];
						
						if(i == 0) {
							if(resultUser["_id"].ToString() == PlayerManager.I.player.playerID) {
								AchievementManager.I.EarnAchievement("Kampioen");
							}
							avatarWon.GetComponent<buildAvatar>().setCustomAvatarByString(resultProfile["avatar"].ToString());
							avatarWon.SetActive(true);
							avatarWon.transform.DOScale(30,2).SetEase(Ease.OutExpo).SetDelay(1);
						}
						GameObject row = gameObject.transform.GetChild(i).gameObject;
						row.SetActive(true);
						if(i <= 2) {

							row.transform.GetChild(5).GetComponent<Text>().text = resultProfile["wonAttr"].ToString();
							row.GetComponent<RectTransform> ().DOScale (1.2f-minus, .5f).SetEase (Ease.InFlash).SetDelay (delayRow);
							row.GetComponent<RectTransform> ().DOScale (1, 1f).SetEase (Ease.OutExpo).SetDelay ((.5f + delayRow));
							delayRow += .2f;
							minus += 0.05f;
						} else {
							
							row.transform.GetChild(6).GetComponent<Text>().text = resultProfile["wonAttr"].ToString();
						}
						row.transform.GetChild(0).GetComponent<Image>().sprite = PlayerManager.I.GetRankSprite (int.Parse(resultProfile["lvl"].ToString()));
						row.transform.GetChild(1).GetComponent<Text>().text = resultProfile["name"].ToString();
		


						// 

					}
				}
			}
			else {

			}
		});
	}
}
