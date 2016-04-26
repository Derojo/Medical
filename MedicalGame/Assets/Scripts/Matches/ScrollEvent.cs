using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollEvent : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

	public ScrollRect scrollrect;
	public GameObject loadicon;
	public RectTransform content;
	private bool update = false;


	public virtual void OnBeginDrag (PointerEventData data) 
	{
		Debug.Log(" Start dragging " + this.name + "!");
		Debug.Log (data.delta);
		if (data.delta.y < 0.0f) {
			update = true;
		} else {
			update = false;
		}
	}

	public virtual void OnEndDrag (PointerEventData data) 
	{
		if (update) {
			// Enable small load icon
			loadicon.SetActive (true);
			loadicon.GetComponent<Animator>().SetBool ("Loading", true);
			// Stop the scroll movement
			scrollrect.StopMovement ();

			content.DOLocalMoveY (-50f, 1f).SetEase(Ease.OutFlash);
//			content.localPosition = new Vector3(2.7f, -50f, 0);
			scrollrect.enabled = false;
			// Check for updates
			StartCoroutine(checkForUpdates());

		}

	}

	private IEnumerator checkForUpdates() {
		MatchManager.I.checkForUpdateMatches ();
		while(MatchManager.I.checkUpdates == false) {
			yield return null;
		}
		yield return new WaitForSeconds (1);
		GameObject.FindObjectOfType<CurrentMatches> ().updateMatches ();
		loadicon.GetComponent<Animator>().SetBool ("Loading", false);
		loadicon.SetActive (false);
		scrollrect.enabled = true;
	}

	private void elastBack() {
		
	}
}
