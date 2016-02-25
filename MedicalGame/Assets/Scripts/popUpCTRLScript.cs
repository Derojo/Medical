using UnityEngine;
using System.Collections;

public class popUpCTRLScript : MonoBehaviour {

    public GameObject popUpPanel;
    public float speed;
    public void close()
    {
        popUpPanel.SetActive(false);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
