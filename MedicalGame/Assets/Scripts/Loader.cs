using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

[Prefab("Loader", true, "")]
public class Loader : Singleton<Loader> {

	public GameObject overlay;
	public GameObject icon;
	public GameObject mini;
	public GameObject finished;

	public bool Load() {return true;}

	void OnLevelWasLoaded(int level) {
		disableLoader ();
	}

	public void LoadScene(string scene) {

		StartCoroutine(LoadSceneIE(scene));
	}

	private IEnumerator LoadSceneIE(string scene) {
		if (!gameObject.activeSelf) {
			enableLoader ();
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

	public void showFinishedPopup(string playerName) {
		finished.SetActive (true);
//		enableBackground ();

		finished.transform.GetChild (2).GetComponent<Text> ().text = "Helaas je hebt verloren van " + playerName;
		finished.GetComponent<Image> ().DOFade (1, 0.5f);
		finished.transform.GetComponentInChildren<Image> ().DOFade (1, 0.5f);
		foreach (Text text in finished.transform.GetComponentsInChildren<Text>()) {
			text.DOFade (1, 0.5f);
		}
		finished.transform.DOScale(1.1f, 0.5f);
		finished.transform.DOScale(1, 0.2f).SetDelay(0.5f);
	}

	public void HidePopups() {
		Debug.Log ("moii");
		finished.SetActive (false);
	}

}
