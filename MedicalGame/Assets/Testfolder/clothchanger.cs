using UnityEngine;
using System.Collections;

public class clothchanger : MonoBehaviour {

	public Mesh[] shirts;
	public SkinnedMeshRenderer RootShirt;
	private Mesh firstMesh;
	int currentShirt = 1;

	// Use this for initialization
	void Start () {
		firstMesh = RootShirt.sharedMesh;
		StartCoroutine(changeShirtAfterTime(2f));
	}
	
	public IEnumerator changeShirtAfterTime(float time) {
		yield return new WaitForSeconds (time);
		RootShirt.sharedMesh = shirts[0];

	}

	public void changeShirt() {
		SetNextShirt ();
		RootShirt.sharedMesh = shirts[currentShirt];
		RootShirt.sharedMaterial = (Material)Resources.Load("Materials/"+shirts[currentShirt].name, typeof(Material));
	}

	private void SetNextShirt() {
		int allShirts = shirts.Length;
		Debug.Log (allShirts);
		if (currentShirt + 1 == allShirts) {
			Debug.Log ("t");
			currentShirt = 0;
		} else {
			Debug.Log ("test");
			currentShirt++;
		}
	}


}