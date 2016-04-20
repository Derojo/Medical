using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[Prefab("Loader", true, "")]
public class Loader : Singleton<Loader> {

	public GameObject overlay;
	public GameObject icon;
	public GameObject mini;

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

}
