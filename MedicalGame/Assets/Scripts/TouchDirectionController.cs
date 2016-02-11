using UnityEngine;
using System.Collections;

public class TouchDirectionController : MonoBehaviour {
    public Transform lookAt;

    private Vector3 desiredPosition;
    private Vector3 offset;

    public float smoothSpeed = 7.5f;

    public float yOffset = 0.0f;
    public float distance = 5.0f;
	// Use this for initialization
	private void Start () {
        offset = new Vector3(0, yOffset, -1f * distance);
	}
	
    private void FixedUpdate()
    {
        desiredPosition = lookAt.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(lookAt.position + Vector3.up);
    }

	// Update is called once per frame
	void Update ()
    {
	    if(SwipeManager.Instance.IsSwiping(SwipeDirection.Left))
        {
            SlideCamera(Vector3.up * 90);
        }
        else if (SwipeManager.Instance.IsSwiping(SwipeDirection.Right))
        {
            SlideCamera(Vector3.down * 90);
        }
         if (SwipeManager.Instance.IsSwiping(SwipeDirection.Up))
        {
            SlideCamera(Vector3.back * 90);
        }
        else if (SwipeManager.Instance.IsSwiping(SwipeDirection.Down))
        {
            SlideCamera(Vector3.forward * 90);
        }
    }

    public void SlideCamera (Vector3 rotation)
    {
        offset = Quaternion.Euler(rotation) * offset;
    }

}
