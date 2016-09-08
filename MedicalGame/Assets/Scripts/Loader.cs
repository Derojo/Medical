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
	public GameObject nolives;
	public GameObject nointernet;
	public Sprite wonSprite;
	public Sprite loseSprite;
	public Sprite tieSprite;
	public float timeUntillMessage = 0;
	private bool checkedInternet = false;
	public bool noInternet = false;
	private float processTime = 0;

	public bool Load() {return true;}

	void OnLevelWasLoaded(int level) {
		disableLoader ();
	}

	public void LoadScene(string scene, bool loadWithAds = false) {
		processTime = 0;
		if(loadWithAds) {
			AdManager.I.ShowAd("video");
			Loader.I.enableLoader ();
		}
		CheckInternetConnection();
		processTime = processTime+Time.deltaTime;
		StartCoroutine(LoadSceneIE(scene, true));
	}

	private IEnumerator LoadSceneIE(string scene, bool loadWithAds = false) {
		while(!checkedInternet) {
			yield return new WaitForSeconds(1);
		}
		
		if(!noInternet) {
			if (!gameObject.activeSelf) {
				if(!loadWithAds) {
					enableLoader ();	
				}
			}
			AsyncOperation async = SceneManager.LoadSceneAsync(scene);
			while (!async.isDone) {
				processTime = processTime+Time.deltaTime;
				yield return null;
			}
			if(processTime >= timeUntillMessage) {
				ShowNoInternetPopup(true);
			}
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
	
	
	public void showLivesPopup() {
		gameObject.GetComponent<Canvas> ().enabled = true;
		nolives.SetActive (true);
		
		nolives.GetComponent<Image> ().DOFade (1, 0.5f);
		nolives.transform.GetChild (0).GetComponent<Image> ().DOFade (1, 0.5f);
		nolives.transform.GetComponentInChildren<Image> ().DOFade (1, 0.5f);
		foreach (Text text in nolives.transform.GetComponentsInChildren<Text>()) {
			text.DOFade (1, 0.5f);
		}
		
		nolives.transform.DOScale(1.1f, 0.5f);
		nolives.transform.DOScale(1, 0.2f).SetDelay(0.5f);
		StartCoroutine (HideLivesPopup (3f));
	}
	
	public IEnumerator HideLivesPopup(float time) {
		yield return new WaitForSeconds(time);
		nolives.transform.DOScale (1.1f, 0.4f).SetEase (Ease.InFlash);
		nolives.transform.DOScale (0f, 0.1f).SetEase (Ease.OutSine).SetDelay(0.5f);;
		yield return new WaitForSeconds(0.5f);
		nolives.SetActive (false);
		gameObject.GetComponent<Canvas> ().enabled = false;
	}
	
	public void CheckInternetConnection() {
		checkedInternet = false;
		noInternet = false;
		GamedoniaBackend.isInternetConnectionAvailable(delegate (bool success) { 
			if (success) { 
				checkedInternet = true;
			} else {
				checkedInternet = true;
				noInternet = true;
				ShowNoInternetPopup();
			}
		});
		
	}
	public void ShowNoInternetPopup(bool slowConnection = false) {
		gameObject.GetComponent<Canvas> ().enabled = true;
		nointernet.SetActive (true);
		if(slowConnection) {
			nointernet.transform.GetChild (1).GetComponent<Text> ().text = "Trage verbinding!";
			nointernet.transform.GetChild (2).GetComponent<Text> ().text = "De verbinding is erg traag, check uw wifi of mobiele data verbinding";
		}
		nointernet.GetComponent<Image> ().DOFade (1, 0.5f);
		nointernet.transform.GetChild (0).GetComponent<Text> ().DOFade (1, 0.5f);
		nointernet.transform.GetComponentInChildren<Image> ().DOFade (1, 0.5f);
		foreach (Text text in nointernet.transform.GetComponentsInChildren<Text>()) {
			text.DOFade (1, 0.5f);
		}
		
		nointernet.transform.DOScale(1.1f, 0.5f);
		nointernet.transform.DOScale(1, 0.2f).SetDelay(0.5f);
		StartCoroutine (HideNoInternetPopup (3f));
	}
	
	public IEnumerator HideNoInternetPopup(float time) {
		yield return new WaitForSeconds(time);
		nointernet.transform.DOScale (1.1f, 0.4f).SetEase (Ease.InFlash);
		nointernet.transform.DOScale (0f, 0.1f).SetEase (Ease.OutSine).SetDelay(0.5f);;
		yield return new WaitForSeconds(0.5f);
		nointernet.SetActive (false);
		gameObject.GetComponent<Canvas> ().enabled = false;
	}
}
