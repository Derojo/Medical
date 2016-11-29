using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public class rotateObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	

	private float rotSpeed = 20;

	public void OnDrag(PointerEventData data)
	{
		float rotateX = Input.GetAxis("Mouse X")*rotSpeed*Mathf.Deg2Rad;
		transform.RotateAround(Vector3.down, rotateX);
		
	}
	
	public void OnBeginDrag (PointerEventData data) 
	{
	   

	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
	
	}
}