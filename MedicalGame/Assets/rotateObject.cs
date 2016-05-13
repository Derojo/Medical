using UnityEngine;
using System.Collections;

public class rotateObject : MonoBehaviour {
	
	private float baseAngle = 0.0f;

	void OnMouseDown(){
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		pos = Input.mousePosition - pos;
		baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		baseAngle += Mathf.Atan2(transform.up.y, 0) *Mathf.Rad2Deg;
	}

	void OnMouseDrag(){
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		pos = Input.mousePosition - pos;
		float ang = Mathf.Atan2(pos.y, pos.x) *Mathf.Rad2Deg;
		transform.localRotation = Quaternion.AngleAxis(ang, Vector3.up);
	}
}