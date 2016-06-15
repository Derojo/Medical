using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollEventProfile : MonoBehaviour, IBeginDragHandler {

	public ScrollRect scrollrect;
	public Scrollbar scrollbar;
	public GameObject player;
	public GameObject top;
	private float startX = 0;
	private float bvalue = 0;

	void Start() {
		startX = player.transform.position.x;
	}
	
	void Update() {
		bvalue = scrollbar.value;
		if (bvalue < 0.9) {
			player.transform.DOMoveX((startX-(Screen.width/10)),2);
		}
		else if (bvalue > 0.9f) {
			player.transform.DOMoveX((startX),2);
		}
	}
	

	public virtual void OnBeginDrag (PointerEventData data) 
	{

	
	
	}

}
