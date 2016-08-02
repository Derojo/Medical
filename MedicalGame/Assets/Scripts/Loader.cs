using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Gamedonia.Backend;
using System.Collections.Generic;

[Prefab("Loader", true, "")]
public class Loader : Singleton<Loader> {

	public GameObject overlay;
	public GameObject icon;
	public GameObject mini;
	public GameObject finished;
	public Sprite wonSprite;
	public Sprite loseSprite;
	public Sprite tieSprite;

	public bool Load() {return true;}

	void OnLevelWasLoaded(int level) {
		disableLoader ();
	}

	public void LoadScene(string scene, bool loadWithAds = false) {
		if(loadWithAds) {
			AdManager.I.ShowAd("video");
			Loader.I.enableLoader ();
		}
		StartCoroutine(LoadSceneIE(scene, true));
	}

	private IEnumerator LoadSceneIE(string scene, bool loadWithAds = false) {
		if (!gameObject.activeSelf) {
			if(!loadWithAds) {
				enableLoader ();	
			}
		}
		AsyncOperation async = SceneManager.LoadSceneAsync(scene);
		while (!async.isDone) {
			yield return null;
		}
	}

	public void enableLoader() {
		gameObject.GetComponent<Canvas> ().enabled = true;
		overlay.SetActive (true);
		icon.SetActive (true);
		icon.GetComponent<Animator>().SetBool ("Loading", true);
		StartCoroutine (activeMiniMan ());
	}

	public void enableBackground() {
		gameObject.GetComponent<Canvas> ().enabled = true;
		overlay.SetActive (true);
		icon.GetComponent<Animator>().SetBool ("Loading_Background", true);
	}

	public void disableBackground() {
		overlay.SetActive (false);
		icon.GetComponent<Animator>().SetBool ("Loading_Background", true);
	}

	public void disableLoader() {
		gameObject.GetComponent<Canvas> ().enabled = false;
		overlay.SetActive (false);
		icon.SetActive (false);
		mini.SetActive (false);
		icon.GetComponent<Animator>().SetBool ("Loading", false);
	}

	private IEnumerator activeMiniMan() {
		yield return new WaitForSeconds (0.2f);
		mini.SetActive (true);
	}

	public void showFinishedPopup(string playerName, string matchWon, string oppId = "") {
		if(oppId != "") {
				GamedoniaUsers.GetUser (oppId, delegate (bool success, GDUserProfile data) { 
					if (success) {
						Dictionary<string, object> oppProfile = new Dictionary<string, object> ();
						oppProfile = data.profile;
						playerName = oppProfile ["name"].ToString ();
					} else {
						
					}
				});
		}
		string infoText = "";
		string infoTitle = "";
		gameObject.GetComponent<Canvas> ().enabled = true;
		Scene currentScene = SceneManager.GetActiveScene();
		if(currentScene.name == "Home") {
			GameObject.FindObjectOfType<CurrentMatches>().updateMatches();
		}


		finished.SetActive (true);
//		enableBackground ();
		if(matchWon == PlayerManager.I.player.playerID) {
			infoTitle = "Gewonnen";
			infoText = "Gefeliciteerd je hebt gewonnen van " + playerName+"!"; 
			finished.transform.GetChild (0).GetComponent<Image> ().sprite = wonSprite;
		} else {
			if(matchWon == "tie") {
				infoTitle = "Gelijkspel";
				infoText = "Je hebt een gelijkspel met " + playerName+"!"; 
				finished.transform.GetChild (0).GetComponent<Image> ().sprite = tieSprite;
			} else {
				infoTitle = "Verloren";
				infoText = "Helaas je hebt verloren van " + playerName+"!"; 
				finished.transform.GetChild (0).GetComponent<Image> ().sprite = loseSprite;
			}
		}
		
		finished.transform.GetChild (1).GetComponent<Text> ().text = infoTitle;
		finished.transform.GetChild (2).GetComponent<Text> ().text = infoText;
		

		finished.GetComponent<Image> ().DOFade (1, 0.5f);
		finished.transform.GetChild (0).GetComponent<Image> ().DOFade (1, 0.5f);
		finished.transform.GetComponentInChildren<Image> ().DOFade (1, 0.5f);
		foreach (Text text in finished.transform.GetComponentsInChildren<Text>()) {
			text.DOFade (1, 0.5f);
		}
		
		finished.transform.DOScale(1.1f, 0.5f);
		finished.transform.DOScale(1, 0.2f).SetDelay(0.5f);
		StartCoroutine (HideFinishedPopup (2f));
	}

	public IEnumerator HideFinishedPopup(float time) {
		yield return new WaitForSeconds(time);
		finished.transform.DOScale (1.1f, 0.4f).SetEase (Ease.InFlash);
		finished.transform.DOScale (0f, 0.1f).SetEase (Ease.OutSine).SetDelay(0.5f);;
		yield return new WaitForSeconds(0.5f);
		finished.SetActive (false);
		gameObject.GetComponent<Canvas> ().enabled = false;
	}

}
