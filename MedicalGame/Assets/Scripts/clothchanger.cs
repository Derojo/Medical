using UnityEngine;
using System.Collections;

public class clothchanger : MonoBehaviour {

	public Mesh[] shirts;
	public int[] shirtMaterials;
	public int[] heads;
	public SkinnedMeshRenderer RootShirt;
	public MeshRenderer RootHead;
	private Mesh firstMesh;
	private int skinCode = 0;
	private string sexCode = "Male";

	int currentShirt = 0;
	int currentShirtMaterial = 0;
	int currentHead  = 0;

	// Use this for initialization
	void Start () {
		firstMesh = RootShirt.sharedMesh;
	}

	public void changeHead(string state) {

		if (state != "skip") {
			SetNextHead (state);
		}

		string headName = "skin_"+skinCode.ToString() + "_"+currentHead.ToString();

		RootHead.sharedMaterial = (Material)Resources.Load("Materials/"+sexCode+"/Heads/"+ headName, typeof(Material));
	}

	public void changeShirt(string state) {

		if (state != "skip") {
			SetNextShirt (state);
		}

		RootShirt.sharedMesh = shirts[currentShirt];
		string[] splitShirtName;
		splitShirtName = shirts [currentShirt].name.Split (new string[] {"_"}, System.StringSplitOptions.None);

		string shirtName = splitShirtName [0] + "_"+skinCode.ToString() + "_"+splitShirtName [1] + "_"+currentShirtMaterial;

		RootShirt.sharedMaterial = (Material)Resources.Load("Materials/Male/Shirts/"+ shirtName, typeof(Material));
	}

	private void SetNextShirt(string state) {
		int allShirts = shirts.Length;

		if (state == "next") {
			if (currentShirtMaterial < (shirtMaterials [currentShirt] - 1)) {

				currentShirtMaterial++;

			} else {
				if (currentShirt < (allShirts - 1)) {
					currentShirt++;
					currentShirtMaterial = 0;
				}
			}
		} else {
			if (currentShirtMaterial > 0) {
				currentShirtMaterial--;
			} else {
				if (currentShirt > 0) {
					currentShirt--;
					currentShirtMaterial = (shirtMaterials [currentShirt] - 1);
				}
			}
		}
	}

	private void SetNextHead(string state) {
		int allHeads = heads [skinCode];

		if (state == "next") {
			if (currentHead < (allHeads-1)) {
				currentHead++;
			}
		} else {
			if (currentHead > 0) {
				currentHead--;
			}
		}
	}


	public void changeSkinCode(int code) {
		skinCode = code;
		currentHead = 0;
		currentShirt = 0;
		changeShirt ("skip");
		changeHead ("skip");
	}

	public void changeSexCode(string code) {
		sexCode = code;
		currentHead = 0;
		currentShirt = 0;
		changeShirt ("skip");
		changeHead ("skip");
	}


} 
