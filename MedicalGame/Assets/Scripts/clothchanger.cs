using UnityEngine;
using System.Collections;
using DG.Tweening;
public class clothchanger : MonoBehaviour {

	public GameObject poppetje;
	//  Array of available customization
	public Mesh[] hairstyles;
	public int[] hairstyleMaterials;
	public int[] heads;
	public Mesh[] shirts;
	public int[] shirtMaterials;
	public Mesh[] trousers;
	public int[] trouserMaterials;

	// Root meshes
	public MeshFilter HairstyleMesh;
	public MeshRenderer RootHairstyle;
	public MeshRenderer RootHead;
	public SkinnedMeshRenderer RootShirt;
	public SkinnedMeshRenderer RootTrouser;
	private Mesh firstMesh;
	// Selections
	private int skinCode = 0;
	private string sexCode = "Male";
	// Keeping track of current selection
	int currentHairstyle = 0;
	int currentHairstyleMaterial = 0;
	int currentShirt = 0;
	int currentShirtMaterial = 0;
	int currentTrouser = 0;
	int currentTrouserMaterial = 0;
	int currentHead  = 0;

	// Use this for initialization
	void Start () {
		firstMesh = RootShirt.sharedMesh;
	}


	public void changeHairStyle(string state) {

		if (state != "skip") {
			SetNextHairstyle (state);
		}

		HairstyleMesh.sharedMesh = hairstyles[currentHairstyle];

		if (hairstyles [currentHairstyle] != null) {
			Debug.Log (currentHairstyleMaterial);
			string hairstylename = hairstyles [currentHairstyle].name + "_" + currentHairstyleMaterial.ToString ();
			Debug.Log (hairstylename);
			RootHairstyle.sharedMaterial = (Material)Resources.Load ("Materials/" + sexCode + "/Hairstyles/" + hairstylename, typeof(Material));
		}
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

		RootShirt.sharedMaterial = (Material)Resources.Load("Materials/"+sexCode+"/Shirts/"+ shirtName, typeof(Material));
	}

	public void changeTrouser(string state) {
		Debug.Log ("test");
		if (state != "skip") {
			SetNextTrouser (state);
		}

		RootTrouser.sharedMesh = trousers[currentTrouser];


		string trouserName = trousers [currentTrouser].name + "_"+currentTrouserMaterial;

		RootTrouser.sharedMaterial = (Material)Resources.Load("Materials/"+sexCode+"/Trousers/"+ trouserName, typeof(Material));
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

	private void SetNextTrouser(string state) {
		int allTrousers = trousers.Length;

		if (state == "next") {
			if (currentTrouserMaterial < (trouserMaterials [currentTrouser] - 1)) {

				currentTrouserMaterial++;

			} else {
				if (currentTrouser < (allTrousers - 1)) {
					currentTrouser++;
					currentTrouserMaterial = 0;
				}
			}
		} else {
			if (currentTrouserMaterial > 0) {
				currentTrouserMaterial--;
			} else {
				if (currentTrouser > 0) {
					currentTrouser--;
					currentTrouserMaterial = (trouserMaterials [currentTrouser] - 1);
				}
			}
		}
	}

	private void SetNextHairstyle(string state) {
		int allHairstyles = hairstyles.Length;

		if (state == "next") {
			if (currentHairstyleMaterial < (hairstyleMaterials [currentHairstyle] - 1)) {

				currentHairstyleMaterial++;

			} else {
				if (currentHairstyle < (allHairstyles - 1)) {
					currentHairstyle++;
					currentHairstyleMaterial = 0;
				}
			}
		} else {
			if (currentHairstyleMaterial > 0) {
				currentHairstyleMaterial--;
			} else {
				if (currentHairstyle > 0) {
					currentHairstyle--;
					currentHairstyleMaterial = (hairstyleMaterials [currentHairstyle] - 1);
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
