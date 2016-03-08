using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loader : MonoBehaviour {

	public GameObject overlay;
	public GameObject icon;
	public GameObject mini;

	void Start() {
		disableLoader ();
	}
	public void enableLoader() {
		gameObject.SetActive (true);
		overlay.SetActive (true);
		icon.SetActive (true);
//		overlay.GetComponent<Animator>().SetBool ("Loading", true);
		icon.GetComponent<Animator>().SetBool ("Loading", true);
		StartCoroutine (activeMiniMan ());
	}

	public void enableBackground() {
		overlay.SetActive (true);
		icon.GetComponent<Animator>().SetBool ("Loading_Background", true);
	}

	public void disableBackground() {
		overlay.SetActive (false);
		icon.GetComponent<Animator>().SetBool ("Loading_Background", true);
	}

	public void disableLoader() {
		gameObject.SetActive (false);
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
